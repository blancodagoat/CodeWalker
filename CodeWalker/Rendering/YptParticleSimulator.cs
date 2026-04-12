using CodeWalker.GameFiles;
using CodeWalker.World;
using SharpDX;
using System;
using System.Collections.Generic;

namespace CodeWalker.Rendering
{
    // Simple runtime particle simulator for YPT files in the model viewer.
    //
    // Reads spawn rate / lifetime / speed / creation domain / acceleration / dampening
    // from the first effect rule's event emitters, maintains a pool of live particles,
    // Euler-integrates them each frame, and exposes them to the renderer as
    // per-particle drawable instances (no new shader needed).
    //
    // This is a preview aid, not a faithful reimplementation of R*'s ptfx runtime.
    public class YptParticleSimulator
    {
        public const int MaxParticles = 4096;

        public struct LiveParticle
        {
            public Vector3 Position;
            public Vector3 Velocity;
            public Vector3 Size;        // WHD
            public Vector4 Colour;      // rgba 0..1
            public float Age;
            public float Life;          // total lifetime (seconds)
            public DrawableBase Drawable;
        }

        // Per-emitter extracted parameters.
        private class EmitterParams
        {
            public ParticleEventEmitter Event;
            public ParticleEmitterRule Emitter;
            public ParticleRule Rule;
            public DrawableBase Drawable;

            public float SpawnRate;         // particles per second
            public float LifeMin;
            public float LifeMax;
            public float SpeedMin;
            public float SpeedMax;
            public Vector3 CreationCentre;
            public Vector3 CreationHalfSize;
            public ParticleDomainType DomainType;

            public Vector3 AccelMin;        // acceleration (e.g. gravity) in m/s^2
            public Vector3 AccelMax;
            public bool EnableGravity;

            public Vector3 DampMin;         // per-second velocity damping factor (0 = none)
            public Vector3 DampMax;

            public Vector3 SizeMin;
            public Vector3 SizeMax;

            public Vector4 ColourMin;
            public Vector4 ColourMax;

            public float SpawnAccumulator;
        }

        private readonly YptFile ypt;
        private readonly List<EmitterParams> emitters = new List<EmitterParams>();
        private readonly List<LiveParticle> live = new List<LiveParticle>(512);
        private readonly Random rng = new Random(12345);

        public int LiveCount => live.Count;
        public string DebugSummary { get; private set; } = "";

        public YptParticleSimulator(YptFile yptFile)
        {
            ypt = yptFile;
            BuildEmitters();
        }

        private void BuildEmitters()
        {
            if (ypt?.AllEffects == null || ypt.AllEffects.Length == 0) return;

            // Pick the first effect rule to preview. Model viewer shows one drawable at a time;
            // this gives consistent behaviour with the static preview.
            var effect = ypt.AllEffects[0];
            if (effect?.EventEmitters?.data_items == null) return;

            var drawables = ypt.PtfxList?.DrawableDictionary?.Drawables?.data_items;
            var drawHashes = ypt.PtfxList?.DrawableDictionary?.Hashes;

            foreach (var ev in effect.EventEmitters.data_items)
            {
                if (ev == null) continue;

                var ep = new EmitterParams
                {
                    Event = ev,
                    Emitter = ev.EmitterRule,
                    Rule = ev.ParticleRule,
                };

                ExtractEmitter(ep);
                ExtractRule(ep);

                // Resolve the drawable used by this particle rule (first entry).
                var pd = ep.Rule?.Drawables?.data_items;
                if (pd != null && pd.Length > 0 && pd[0]?.Drawable != null)
                {
                    ep.Drawable = pd[0].Drawable;
                }
                else if (drawables != null && drawables.Length > 0)
                {
                    // Fallback: first drawable in dictionary.
                    ep.Drawable = drawables[0];
                }

                if (ep.SpawnRate <= 0.0f) ep.SpawnRate = 30.0f; // sensible default
                if (ep.LifeMax <= 0.0f) { ep.LifeMin = 1.0f; ep.LifeMax = 2.0f; }
                if (ep.SizeMax == Vector3.Zero) { ep.SizeMin = new Vector3(0.1f); ep.SizeMax = new Vector3(0.2f); }
                if (ep.ColourMax == Vector4.Zero) { ep.ColourMin = new Vector4(1); ep.ColourMax = new Vector4(1); }

                emitters.Add(ep);
            }

            DebugSummary = $"{emitters.Count} emitter(s)";
        }

        private static Vector4 FirstValue(ParticleKeyframeProp kfp)
        {
            var items = kfp?.Values?.data_items;
            if (items == null || items.Length == 0) return Vector4.Zero;
            return items[0].KeyframeValue;
        }

        private void ExtractEmitter(EmitterParams ep)
        {
            var er = ep.Emitter;
            if (er == null) return;

            // KeyframeProps indices (from ptxEmitterRule name hash dictionary):
            //  0 spawnRateOverTime, 1 spawnRateOverDist, 2 particleLife,
            //  3 playbackRateScalar, 4 speedScalar, 5 sizeScalar,
            //  6 accnScalar, 7 dampeningScalar, 8 matrixWeightScalar, 9 inheritVelocity
            var kfps = er.KeyframeProps;
            if (kfps != null && kfps.Length >= 10)
            {
                var spawn = FirstValue(kfps[0]);
                ep.SpawnRate = Math.Max(0.0f, spawn.X);

                var life = FirstValue(kfps[2]);
                // KeyframeValue holds min in X and max in Y for ranged props.
                ep.LifeMin = Math.Max(0.05f, life.X);
                ep.LifeMax = Math.Max(ep.LifeMin, life.Y > 0 ? life.Y : life.X);

                var speed = FirstValue(kfps[4]);
                ep.SpeedMin = speed.X;
                ep.SpeedMax = speed.Y > 0 ? speed.Y : speed.X;
            }

            // Creation domain: position + outer size (half-extents for box / radius for sphere).
            var cd = er.CreationDomainObj;
            if (cd != null)
            {
                ep.DomainType = cd.DomainType;
                var pos = FirstValue(cd.PositionKFP);
                ep.CreationCentre = new Vector3(pos.X, pos.Y, pos.Z);
                var outer = FirstValue(cd.SizeOuterKFP);
                ep.CreationHalfSize = new Vector3(outer.X, outer.Y, outer.Z);
            }
        }

        private void ExtractRule(EmitterParams ep)
        {
            var rule = ep.Rule;
            if (rule?.AllBehaviours?.data_items == null) return;

            foreach (var b in rule.AllBehaviours.data_items)
            {
                switch (b)
                {
                    case ParticleBehaviourAcceleration acc:
                        {
                            var mn = FirstValue(acc.XYZMinKFP);
                            var mx = FirstValue(acc.XYZMaxKFP);
                            ep.AccelMin = new Vector3(mn.X, mn.Y, mn.Z);
                            ep.AccelMax = new Vector3(mx.X, mx.Y, mx.Z);
                            ep.EnableGravity = acc.EnableGravity != 0;
                            break;
                        }
                    case ParticleBehaviourDampening damp:
                        {
                            var mn = FirstValue(damp.XYZMinKFP);
                            var mx = FirstValue(damp.XYZMaxKFP);
                            ep.DampMin = new Vector3(mn.X, mn.Y, mn.Z);
                            ep.DampMax = new Vector3(mx.X, mx.Y, mx.Z);
                            break;
                        }
                    case ParticleBehaviourSize sz:
                        {
                            var mn = FirstValue(sz.WhdMinKFP);
                            var mx = FirstValue(sz.WhdMaxKFP);
                            ep.SizeMin = new Vector3(mn.X, mn.Y, mn.Z);
                            ep.SizeMax = new Vector3(mx.X, mx.Y, mx.Z);
                            if (ep.SizeMax.LengthSquared() < 1e-6f) ep.SizeMax = ep.SizeMin;
                            break;
                        }
                    case ParticleBehaviourColour col:
                        {
                            ep.ColourMin = FirstValue(col.RGBAMinKFP);
                            ep.ColourMax = col.RGBAMaxEnable != 0 ? FirstValue(col.RGBAMaxKFP) : ep.ColourMin;
                            if (ep.ColourMin == Vector4.Zero) ep.ColourMin = new Vector4(1);
                            if (ep.ColourMax == Vector4.Zero) ep.ColourMax = ep.ColourMin;
                            break;
                        }
                }
            }
        }

        private float Rand01() => (float)rng.NextDouble();
        private float RandRange(float a, float b) => a + (b - a) * Rand01();
        private Vector3 RandRange(Vector3 a, Vector3 b) => new Vector3(RandRange(a.X, b.X), RandRange(a.Y, b.Y), RandRange(a.Z, b.Z));
        private Vector4 RandRange(Vector4 a, Vector4 b) => new Vector4(RandRange(a.X, b.X), RandRange(a.Y, b.Y), RandRange(a.Z, b.Z), RandRange(a.W, b.W));

        private Vector3 RandUnit()
        {
            float z = Rand01() * 2.0f - 1.0f;
            float t = Rand01() * (float)(Math.PI * 2);
            float r = (float)Math.Sqrt(Math.Max(0.0f, 1.0f - z * z));
            return new Vector3(r * (float)Math.Cos(t), r * (float)Math.Sin(t), z);
        }

        public void Update(float dt)
        {
            if (dt <= 0.0f) return;
            if (dt > 0.1f) dt = 0.1f; // clamp pathological frames

            // Spawn new particles from each emitter.
            foreach (var ep in emitters)
            {
                ep.SpawnAccumulator += dt * ep.SpawnRate;
                int toSpawn = (int)ep.SpawnAccumulator;
                if (toSpawn > 0) ep.SpawnAccumulator -= toSpawn;

                int capacity = MaxParticles - live.Count;
                if (toSpawn > capacity) toSpawn = capacity;

                for (int i = 0; i < toSpawn; i++)
                {
                    // Position: centre plus random offset within creation domain.
                    Vector3 offset;
                    switch (ep.DomainType)
                    {
                        case ParticleDomainType.Sphere:
                            offset = RandUnit() * (ep.CreationHalfSize.X * Rand01());
                            break;
                        case ParticleDomainType.Cylinder:
                            {
                                var u = RandUnit();
                                offset = new Vector3(u.X * ep.CreationHalfSize.X * Rand01(),
                                                     u.Y * ep.CreationHalfSize.Y * Rand01(),
                                                     (Rand01() * 2 - 1) * ep.CreationHalfSize.Z);
                                break;
                            }
                        default: // Box
                            offset = new Vector3(
                                (Rand01() * 2 - 1) * ep.CreationHalfSize.X,
                                (Rand01() * 2 - 1) * ep.CreationHalfSize.Y,
                                (Rand01() * 2 - 1) * ep.CreationHalfSize.Z);
                            break;
                    }

                    float speed = RandRange(ep.SpeedMin, ep.SpeedMax);
                    Vector3 vel = RandUnit() * speed;
                    // Prefer a slight upward bias so typical effects look OK without real initial velocity extraction.
                    vel.Z += speed * 0.25f;

                    var p = new LiveParticle
                    {
                        Position = ep.CreationCentre + offset,
                        Velocity = vel,
                        Size = RandRange(ep.SizeMin, ep.SizeMax),
                        Colour = RandRange(ep.ColourMin, ep.ColourMax),
                        Age = 0.0f,
                        Life = RandRange(ep.LifeMin, ep.LifeMax),
                        Drawable = ep.Drawable,
                    };
                    live.Add(p);
                }
            }

            // Integrate & kill expired. Iterate back-to-front for swap-remove.
            // Use the first emitter's accel/damp since we don't track per-particle emitter refs.
            var ap = emitters.Count > 0 ? emitters[0] : null;
            Vector3 accel = Vector3.Zero;
            Vector3 damp = Vector3.Zero;
            if (ap != null)
            {
                accel = (ap.AccelMin + ap.AccelMax) * 0.5f;
                if (ap.EnableGravity) accel.Z -= 9.81f;
                damp = (ap.DampMin + ap.DampMax) * 0.5f;
            }

            for (int i = live.Count - 1; i >= 0; i--)
            {
                var p = live[i];
                p.Age += dt;
                if (p.Age >= p.Life)
                {
                    live[i] = live[live.Count - 1];
                    live.RemoveAt(live.Count - 1);
                    continue;
                }
                // Euler integration.
                p.Velocity += accel * dt;
                // Damping: velocity *= max(0, 1 - damp*dt)
                p.Velocity.X *= Math.Max(0.0f, 1.0f - damp.X * dt);
                p.Velocity.Y *= Math.Max(0.0f, 1.0f - damp.Y * dt);
                p.Velocity.Z *= Math.Max(0.0f, 1.0f - damp.Z * dt);
                p.Position += p.Velocity * dt;
                live[i] = p;
            }
        }

        public void Reset()
        {
            live.Clear();
            foreach (var ep in emitters) ep.SpawnAccumulator = 0.0f;
        }

        // Emit each live particle as a per-instance draw via the supplied callback.
        // The callback is given a throwaway YmapEntityDef set up with per-particle transform
        // and the drawable to render. Lifetime-based alpha is baked into the entity via
        // scale-at-death (collapse) so the caller can keep using the existing draw path.
        public void EnqueueDraws(Action<DrawableBase, YmapEntityDef> drawCallback)
        {
            if (drawCallback == null) return;

            for (int i = 0; i < live.Count; i++)
            {
                var p = live[i];
                if (p.Drawable == null) continue;

                float lifeFrac = (p.Life > 0.0f) ? (p.Age / p.Life) : 0.0f;
                // Fade out scale near end of life so the particle visibly disappears,
                // since we can't cheaply pass per-instance alpha through the standard shader.
                float fade = 1.0f;
                if (lifeFrac > 0.75f) fade = 1.0f - (lifeFrac - 0.75f) / 0.25f;
                if (fade < 0.0f) fade = 0.0f;

                // Map the particle drawable's bounding sphere to unit-ish, then apply size.
                float baseRadius = Math.Max(0.001f, p.Drawable.BoundingSphereRadius);
                Vector3 scale = (p.Size / baseRadius) * fade;
                // Guarantee some minimum visible size if the source size keyframes are zero.
                if (scale.LengthSquared() < 1e-8f) scale = new Vector3(0.1f) * fade;

                var ent = new YmapEntityDef
                {
                    Position = p.Position,
                    Orientation = Quaternion.Identity,
                    Scale = scale,
                };
                ent.BSCenter = p.Drawable.BoundingCenter;
                ent.BSRadius = p.Drawable.BoundingSphereRadius * Math.Max(scale.X, Math.Max(scale.Y, scale.Z));
                ent.BBMin = p.Drawable.BoundingBoxMin;
                ent.BBMax = p.Drawable.BoundingBoxMax;

                drawCallback(p.Drawable, ent);
            }
        }
    }
}
