using System;
using System.Collections.Generic;
using CodeWalker.GameFiles;
using SharpDX;

namespace CodeWalker.Utils
{
    // Generates missing Med/Low LOD models for drawables (ped clothing and props) from the High
    // models. Custom clothing usually ships High only, while vanilla ships all three (eg vanilla
    // jbib_000_u: 1005/400/64 verts) - the ped LOD system wants the lower models, and a High-only
    // 500k-vert jacket renders full density at every distance.
    //
    // The decimator is quadric-error-metric HALF-EDGE collapse: a vertex is only ever merged into
    // an existing neighbour, never moved or interpolated. Surviving vertices keep their original
    // bytes untouched - blend weights, bone indices, UVs, normals, whatever the layout holds -
    // which is why no skeleton or Blender round-trip is needed. Position (offset 0 of every
    // layout) is the only attribute the math reads. Vertices duplicated at UV/normal seams are
    // welded by position for the math and collapsed side-by-side, so seams do not crack open.
    //
    // Written from the standard QEM formulation (Garland-Heckbert) with the usual pass/threshold
    // scheduling and flip guard; no third-party code.
    public static class YddLodGen
    {
        // vanilla Med is ~40% of High and Low ~8%, but a 1M-vert custom mesh needs absolute caps
        private const int MedTrisCap = 60000;
        private const int LowTrisCap = 6000;
        private const float MedRatio = 0.4f;
        private const float LowRatio = 0.08f;

        // Fills in missing Med/Low models on the drawable. Returns true when anything was added.
        public static bool GenerateLods(DrawableBase dr, Action<string> log, string ctx)
        {
            var dm = dr?.DrawableModels;
            if (dm?.High == null || dm.High.Length == 0) return false;
            bool needMed = (dm.Med == null) || (dm.Med.Length == 0);
            bool needLow = (dm.Low == null) || (dm.Low.Length == 0);
            if (!needMed && !needLow) return false;

            // cloth-simulated pieces bind the sim to exact vertices; do not touch them
            if (HasClothShader(dr))
            {
                log?.Invoke($"LOD: {ctx} skipped (cloth shader)");
                return false;
            }

            int totalTris = 0;
            foreach (var m in dm.High)
            {
                if (m?.Geometries == null) continue;
                foreach (var g in m.Geometries) totalTris += (int)((g?.IndexBuffer?.Indices?.Length ?? 0) / 3);
            }
            if (totalTris < 16) return false;   // nothing worth generating

            bool added = false;
            if (needMed)
            {
                var med = BuildLodModels(dm.High, Math.Min(MedRatio, (float)MedTrisCap / totalTris));
                if (med != null) { dm.Med = med; added = true; }
            }
            if (needLow)
            {
                var low = BuildLodModels(dm.High, Math.Min(LowRatio, (float)LowTrisCap / totalTris));
                if (low != null) { dm.Low = low; added = true; }
            }
            if (added)
            {
                int mt = CountTris(dm.Med), lt = CountTris(dm.Low);
                log?.Invoke($"LOD: {ctx} high {totalTris} tris -> med {mt}, low {lt}");
            }
            return added;
        }

        private static int CountTris(DrawableModel[] models)
        {
            int n = 0;
            if (models != null)
                foreach (var m in models)
                    if (m?.Geometries != null)
                        foreach (var g in m.Geometries) n += (int)((g?.IndexBuffer?.Indices?.Length ?? 0) / 3);
            return n;
        }

        private static bool HasClothShader(DrawableBase dr)
        {
            var shaders = dr?.ShaderGroup?.Shaders?.data_items;
            if (shaders == null) return false;
            foreach (var s in shaders)
            {
                var n = s?.Name.ToString();
                if ((n != null) && n.Contains("cloth", StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        // Decimates the High models themselves to ~ratio of their triangles. Used to rescue
        // files whose graphics segment exceeds FiveM's raw-streaming limit (files past ~32 MB
        // of graphics data are a confirmed client crash). Same half-edge collapse as the LOD
        // path, so surviving vertices keep their bytes and skinning exactly. A geometry the
        // decimator cannot process is KEPT UNCHANGED - unlike LOD generation, dropping a piece
        // of the visible model is never acceptable here. Returns true when anything shrank.
        public static bool DecimateHigh(DrawableBase dr, float ratio, Action<string> log, string ctx)
        {
            var dm = dr?.DrawableModels;
            if (dm?.High == null || dm.High.Length == 0) return false;
            if (HasClothShader(dr))
            {
                log?.Invoke($"MESH: {ctx} not decimated (cloth shader)");
                return false;
            }

            bool changed = false;
            var newHigh = new DrawableModel[dm.High.Length];
            for (int mi = 0; mi < dm.High.Length; mi++)
            {
                var src = dm.High[mi];
                newHigh[mi] = src;
                if (src?.Geometries == null) continue;

                var geoms = new DrawableGeometry[src.Geometries.Length];
                bool modelChanged = false;
                for (int gi = 0; gi < src.Geometries.Length; gi++)
                {
                    var g = src.Geometries[gi];
                    geoms[gi] = g;
                    DrawableGeometry ng = null;
                    try { ng = DecimateGeometry(g, ratio); } catch { }
                    if ((ng != null) && ((ng.IndexBuffer?.Indices?.Length ?? int.MaxValue) < (g.IndexBuffer?.Indices?.Length ?? 0)))
                    {
                        geoms[gi] = ng;
                        modelChanged = true;
                    }
                }
                if (!modelChanged) continue;

                var nm = new DrawableModel();
                nm.SkeletonBinding = src.SkeletonBinding;
                nm.RenderMaskFlags = src.RenderMaskFlags;
                nm.Geometries = geoms;
                nm.GeometriesCount1 = (ushort)geoms.Length;   // BlockLength reads these before Write
                nm.GeometriesCount2 = (ushort)geoms.Length;
                nm.GeometriesCount3 = (ushort)geoms.Length;
                var aabbs = new List<AABB_s>();
                var shids = new List<ushort>();
                var min = new Vector4(float.MaxValue);
                var max = new Vector4(float.MinValue);
                foreach (var g in geoms)
                {
                    aabbs.Add(g.AABB);
                    shids.Add(g.ShaderID);
                    min = Vector4.Min(min, g.AABB.Min);
                    max = Vector4.Max(max, g.AABB.Max);
                }
                if (aabbs.Count > 1) aabbs.Insert(0, new AABB_s() { Min = min, Max = max });
                nm.BoundsData = aabbs.ToArray();
                nm.ShaderMapping = shids.ToArray();
                newHigh[mi] = nm;
                changed = true;
            }
            if (changed) dm.High = newHigh;
            return changed;
        }

        private static DrawableModel[] BuildLodModels(DrawableModel[] high, float ratio)
        {
            var models = new List<DrawableModel>();
            foreach (var src in high)
            {
                if (src?.Geometries == null) continue;
                var geoms = new List<DrawableGeometry>();
                foreach (var g in src.Geometries)
                {
                    var ng = DecimateGeometry(g, ratio);
                    if (ng != null) geoms.Add(ng);
                }
                if (geoms.Count == 0) continue;

                var nm = new DrawableModel();
                nm.SkeletonBinding = src.SkeletonBinding;
                nm.RenderMaskFlags = src.RenderMaskFlags;
                nm.Geometries = geoms.ToArray();
                // BlockLength reads these BEFORE Write() would set them; layout breaks otherwise
                nm.GeometriesCount1 = (ushort)geoms.Count;
                nm.GeometriesCount2 = (ushort)geoms.Count;
                nm.GeometriesCount3 = (ushort)geoms.Count;
                var aabbs = new List<AABB_s>();
                var shids = new List<ushort>();
                var min = new Vector4(float.MaxValue);
                var max = new Vector4(float.MinValue);
                foreach (var g in geoms)
                {
                    aabbs.Add(g.AABB);
                    shids.Add(g.ShaderID);
                    min = Vector4.Min(min, g.AABB.Min);
                    max = Vector4.Max(max, g.AABB.Max);
                }
                if (aabbs.Count > 1) aabbs.Insert(0, new AABB_s() { Min = min, Max = max });
                nm.BoundsData = aabbs.ToArray();
                nm.ShaderMapping = shids.ToArray();
                models.Add(nm);
            }
            return (models.Count > 0) ? models.ToArray() : null;
        }

        // Decimates one geometry to ~ratio of its triangles. Returns null when the geometry is
        // unsupported (leave that geometry out of the LOD) - never modifies the input.
        private static DrawableGeometry DecimateGeometry(DrawableGeometry geom, float ratio)
        {
            var vd = geom?.VertexData;
            var info = vd?.Info;
            var inds = geom?.IndexBuffer?.Indices;
            if ((vd?.VertexBytes == null) || (info == null) || (inds == null)) return null;
            if (geom.VertexBuffer?.G9_Info != null) return null;             // gen9 layout: not handled
            if ((info.Flags & 1) == 0) return null;                          // no position at offset 0
            if (geom.IndicesPerPrimitive != 3) return null;                  // not a triangle list
            int stride = vd.VertexStride;
            int vcount = vd.VertexCount;
            if ((stride < 12) || (vcount < 3) || (inds.Length < 3)) return null;

            int srcTris = inds.Length / 3;
            int targetTris = Math.Max(8, (int)(srcTris * ratio));
            var dec = new HalfEdgeDecimator(vd.VertexBytes, stride, vcount, inds);
            dec.Run(targetTris);
            var (usedVerts, newIndices) = dec.BuildResult();
            if ((usedVerts == null) || (newIndices == null) || (newIndices.Length < 3)) return null;
            if (usedVerts.Length > 65535) return null;

            var newBytes = new byte[usedVerts.Length * stride];
            for (int i = 0; i < usedVerts.Length; i++)
            {
                Buffer.BlockCopy(vd.VertexBytes, usedVerts[i] * stride, newBytes, i * stride, stride);
            }

            var declCopy = new VertexDeclaration
            {
                Flags = info.Flags,
                Stride = info.Stride,
                Count = info.Count,
                Types = info.Types,
            };
            var nvd = new VertexData
            {
                VertexStride = stride,
                VertexCount = usedVerts.Length,
                Info = declCopy,
                VertexBytes = newBytes,
            };
            var nvb = new VertexBuffer
            {
                VertexStride = (ushort)stride,
                Flags = geom.VertexBuffer?.Flags ?? 0,
                Info = declCopy,
                Data1 = nvd,
                Data2 = nvd,
                VertexCount = (uint)usedVerts.Length,
            };
            // counts preset here because DrawableGeometry.Write copies them from these blocks
            // possibly before the blocks' own Write has recomputed them
            return new DrawableGeometry
            {
                ShaderID = geom.ShaderID,
                AABB = geom.AABB,                       // superset of the surviving vertices: still valid
                BoneIds = (ushort[])geom.BoneIds?.Clone(),
                VertexData = nvd,
                VertexBuffer = nvb,
                IndexBuffer = new IndexBuffer { Indices = newIndices, IndicesCount = (uint)newIndices.Length },
            };
        }

        // QEM half-edge collapse over position-welded groups. Originals are only ever remapped to
        // existing originals; seam duplicates collapse side-by-side or not at all.
        private class HalfEdgeDecimator
        {
            private readonly byte[] bytes;
            private readonly int stride;
            private readonly int vcount;

            private readonly int[] tri;          // corner originals, 3 per triangle
            private readonly bool[] triDead;
            private int aliveTris;

            private readonly int[] vmap;         // original -> current original (union-find style chain)
            private readonly int[] groupOf;      // current group per ORIGINAL representative
            private readonly Vector3[] gpos;     // position per group
            private readonly double[][] gq;      // quadric per group (10 doubles, symmetric 4x4)
            private readonly bool[] gborder;
            private readonly bool[] gdead;
            private readonly List<int>[] vtris;  // alive-ish triangle list per original representative
            private readonly List<int>[] gmembers; // alive original representatives per group
            private readonly bool[] triDirty;

            public HalfEdgeDecimator(byte[] vertexBytes, int vertexStride, int vertexCount, ushort[] indices)
            {
                bytes = vertexBytes; stride = vertexStride; vcount = vertexCount;

                var pos = new Vector3[vcount];
                for (int v = 0; v < vcount; v++) pos[v] = ReadPos(v);

                // weld by quantised position
                var groups = new Dictionary<(int, int, int), int>();
                groupOf = new int[vcount];
                var gposList = new List<Vector3>();
                var gmemList = new List<List<int>>();
                for (int v = 0; v < vcount; v++)
                {
                    var key = ((int)MathF.Round(pos[v].X * 8192f), (int)MathF.Round(pos[v].Y * 8192f), (int)MathF.Round(pos[v].Z * 8192f));
                    if (!groups.TryGetValue(key, out int g))
                    {
                        g = gposList.Count;
                        groups[key] = g;
                        gposList.Add(pos[v]);
                        gmemList.Add(new List<int>());
                    }
                    groupOf[v] = g;
                    gmemList[g].Add(v);
                }
                gpos = gposList.ToArray();
                gmembers = gmemList.ToArray();
                int gcount = gpos.Length;
                gq = new double[gcount][];
                for (int g = 0; g < gcount; g++) gq[g] = new double[10];
                gborder = new bool[gcount];
                gdead = new bool[gcount];

                int tcount = indices.Length / 3;
                tri = new int[tcount * 3];
                triDead = new bool[tcount];
                triDirty = new bool[tcount];
                vmap = new int[vcount];
                vtris = new List<int>[vcount];
                for (int v = 0; v < vcount; v++) vmap[v] = v;

                var edgeUse = new Dictionary<(int, int), int>();
                for (int t = 0; t < tcount; t++)
                {
                    int a = indices[t * 3], b = indices[t * 3 + 1], c = indices[t * 3 + 2];
                    tri[t * 3] = a; tri[t * 3 + 1] = b; tri[t * 3 + 2] = c;
                    int ga = groupOf[a], gb = groupOf[b], gc = groupOf[c];
                    if ((ga == gb) || (gb == gc) || (ga == gc)) { triDead[t] = true; continue; }
                    aliveTris++;
                    (vtris[a] ??= new List<int>()).Add(t);
                    (vtris[b] ??= new List<int>()).Add(t);
                    (vtris[c] ??= new List<int>()).Add(t);

                    // face plane quadric added to each corner group
                    var p0 = gpos[ga]; var p1 = gpos[gb]; var p2 = gpos[gc];
                    var n = Vector3.Cross(p1 - p0, p2 - p0);
                    float len = n.Length();
                    if (len > 1e-12f)
                    {
                        n /= len;
                        double d = -Vector3.Dot(n, p0);
                        AddPlane(gq[ga], n, d); AddPlane(gq[gb], n, d); AddPlane(gq[gc], n, d);
                    }
                    CountEdge(edgeUse, ga, gb); CountEdge(edgeUse, gb, gc); CountEdge(edgeUse, ga, gc);
                }
                foreach (var kv in edgeUse)
                {
                    if (kv.Value == 1) { gborder[kv.Key.Item1] = true; gborder[kv.Key.Item2] = true; }
                }
            }

            private static void CountEdge(Dictionary<(int, int), int> d, int a, int b)
            {
                var k = (a < b) ? (a, b) : (b, a);
                d.TryGetValue(k, out int n);
                d[k] = n + 1;
            }

            private Vector3 ReadPos(int v)
            {
                int o = v * stride;
                return new Vector3(BitConverter.ToSingle(bytes, o), BitConverter.ToSingle(bytes, o + 4), BitConverter.ToSingle(bytes, o + 8));
            }

            private static void AddPlane(double[] q, Vector3 n, double d)
            {
                double a = n.X, b = n.Y, c = n.Z;
                q[0] += a * a; q[1] += a * b; q[2] += a * c; q[3] += a * d;
                q[4] += b * b; q[5] += b * c; q[6] += b * d;
                q[7] += c * c; q[8] += c * d;
                q[9] += d * d;
            }

            private static double EvalQ(double[] q, Vector3 p)
            {
                double x = p.X, y = p.Y, z = p.Z;
                return q[0] * x * x + 2 * q[1] * x * y + 2 * q[2] * x * z + 2 * q[3] * x
                     + q[4] * y * y + 2 * q[5] * y * z + 2 * q[6] * y
                     + q[7] * z * z + 2 * q[8] * z
                     + q[9];
            }

            private int Find(int v)
            {
                while (vmap[v] != v)
                {
                    vmap[v] = vmap[vmap[v]];
                    v = vmap[v];
                }
                return v;
            }

            public void Run(int targetTris)
            {
                // pass/threshold scheduling: cheap collapses first, threshold grows each pass
                for (int pass = 0; (pass < 150) && (aliveTris > targetTris); pass++)
                {
                    double threshold = 1e-9 * Math.Pow(pass + 3, 7);
                    Array.Clear(triDirty, 0, triDirty.Length);

                    for (int t = 0; (t < triDead.Length) && (aliveTris > targetTris); t++)
                    {
                        if (triDead[t] || triDirty[t]) continue;
                        for (int e = 0; e < 3; e++)
                        {
                            int u = Find(tri[t * 3 + e]);
                            int v = Find(tri[t * 3 + ((e + 1) % 3)]);
                            int gu = groupOf[u], gv = groupOf[v];
                            if (gu == gv) continue;
                            if (gborder[gu] && !gborder[gv]) continue;   // never pull a border inward

                            var qsum = new double[10];
                            for (int i = 0; i < 10; i++) qsum[i] = gq[gu][i] + gq[gv][i];
                            double cost = EvalQ(qsum, gpos[gv]);
                            if (gborder[gu]) cost *= 100.0;              // border-to-border: strongly protected
                            if (cost > threshold) continue;

                            if (TryCollapse(gu, gv, qsum)) break;        // triangle t changed; move on
                        }
                    }
                }
            }

            // collapse everything in group gu into side-matched partners in group gv
            private bool TryCollapse(int gu, int gv, double[] qsum)
            {
                var members = gmembers[gu];
                var partners = new int[members.Count];
                int mcount = 0;
                for (int i = 0; i < members.Count; i++)
                {
                    int m = members[i];
                    if (Find(m) != m) continue;                          // stale entry
                    int partner = -1;
                    var list = vtris[m];
                    if (list != null)
                    {
                        bool anyAlive = false;
                        foreach (var t in list)
                        {
                            if (triDead[t]) continue;
                            anyAlive = true;
                            for (int c = 0; c < 3; c++)
                            {
                                int w = Find(tri[t * 3 + c]);
                                if (groupOf[w] == gv) { partner = w; break; }
                            }
                            if (partner >= 0) break;
                        }
                        if (!anyAlive) continue;                         // dead vertex, ignore
                    }
                    else continue;
                    if (partner < 0) return false;                       // seam side with no twin: refuse
                    members[mcount] = m;
                    partners[mcount] = partner;
                    mcount++;
                }
                if (mcount == 0) return false;

                // flip guard: no triangle of gu may invert when its corner moves to gv's position
                for (int i = 0; i < mcount; i++)
                {
                    foreach (var t in vtris[members[i]])
                    {
                        if (triDead[t]) continue;
                        int a = Find(tri[t * 3]), b = Find(tri[t * 3 + 1]), c = Find(tri[t * 3 + 2]);
                        int ga = groupOf[a], gb = groupOf[b], gc = groupOf[c];
                        if ((ga == gv) || (gb == gv) || (gc == gv)) continue;   // will degenerate, fine
                        var p0 = (ga == gu) ? gpos[gv] : gpos[ga];
                        var p1 = (gb == gu) ? gpos[gv] : gpos[gb];
                        var p2 = (gc == gu) ? gpos[gv] : gpos[gc];
                        var n0 = Vector3.Cross(gpos[gb] - gpos[ga], gpos[gc] - gpos[ga]);
                        var n1 = Vector3.Cross(p1 - p0, p2 - p0);
                        float l0 = n0.Length(), l1 = n1.Length();
                        if ((l0 > 1e-12f) && (l1 > 1e-12f) && (Vector3.Dot(n0 / l0, n1 / l1) < 0.2f)) return false;
                    }
                }

                // commit
                for (int i = 0; i < mcount; i++)
                {
                    int m = members[i], p = partners[i];
                    vmap[m] = p;
                    var list = vtris[m];
                    foreach (var t in list)
                    {
                        if (triDead[t]) continue;
                        int a = Find(tri[t * 3]), b = Find(tri[t * 3 + 1]), c = Find(tri[t * 3 + 2]);
                        int ga = groupOf[a], gb = groupOf[b], gc = groupOf[c];
                        if ((ga == gb) || (gb == gc) || (ga == gc))
                        {
                            triDead[t] = true;
                            aliveTris--;
                        }
                        else
                        {
                            triDirty[t] = true;
                            (vtris[p] ??= new List<int>()).Add(t);
                        }
                    }
                }
                for (int i = 0; i < 10; i++) gq[gv][i] = qsum[i];
                if (gborder[gu]) gborder[gv] = true;
                gdead[gu] = true;
                gmembers[gu].Clear();
                return true;
            }

            public (int[] used, ushort[] indices) BuildResult()
            {
                var remap = new Dictionary<int, int>();
                var used = new List<int>();
                var outInds = new List<ushort>();
                for (int t = 0; t < triDead.Length; t++)
                {
                    if (triDead[t]) continue;
                    int a = Find(tri[t * 3]), b = Find(tri[t * 3 + 1]), c = Find(tri[t * 3 + 2]);
                    if ((a == b) || (b == c) || (a == c)) continue;
                    foreach (var v in new[] { a, b, c })
                    {
                        if (!remap.TryGetValue(v, out int nv))
                        {
                            nv = used.Count;
                            remap[v] = nv;
                            used.Add(v);
                        }
                        outInds.Add((ushort)nv);
                    }
                }
                if (outInds.Count < 3) return (null, null);
                if (used.Count > 65535) return (null, null);
                return (used.ToArray(), outInds.ToArray());
            }
        }
    }
}
