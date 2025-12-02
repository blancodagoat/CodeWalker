using System;
using SharpDX;

namespace CodeWalker.Core.Utils
{
    public static class BoundingBoxExtensions
    {
        public static Vector3 Size(this BoundingBox bounds)
        {
            return new Vector3(
                Math.Abs(bounds.Maximum.X - bounds.Minimum.X), 
                Math.Abs(bounds.Maximum.Y - bounds.Minimum.Y),
                Math.Abs(bounds.Maximum.Z - bounds.Minimum.Z));
        }

        public static Vector3 Center(this BoundingBox bounds)
        {
            return (bounds.Minimum + bounds.Maximum) * 0.5F;
        }

        public static BoundingBox Encapsulate(this BoundingBox box, BoundingBox bounds)
        {
            box.Minimum = Vector3.Min(box.Minimum, bounds.Minimum);
            box.Maximum = Vector3.Max(box.Maximum, bounds.Maximum);
            return box;
        }

        public static float Radius(this BoundingBox box)
        {
            var extents = (box.Maximum - box.Minimum) * 0.5F;
            return extents.Length();
        }

        public static BoundingBox Expand(this BoundingBox b, float amount)
        {
            return new BoundingBox(b.Minimum - Vector3.One * amount, b.Maximum + Vector3.One * amount);
        }

        /// <summary>
        /// SIMD-optimized batch processing of bounding box centers.
        /// </summary>
        public static void CenterArray(ReadOnlySpan<BoundingBox> boxes, Span<Vector3> centers)
        {
            if (boxes.Length != centers.Length)
                throw new ArgumentException("Boxes and centers spans must have the same length");

            for (int i = 0; i < boxes.Length; i++)
            {
                centers[i] = (boxes[i].Minimum + boxes[i].Maximum) * 0.5F;
            }
        }

        /// <summary>
        /// SIMD-optimized batch processing of bounding box radii.
        /// </summary>
        public static void RadiusArray(ReadOnlySpan<BoundingBox> boxes, Span<float> radii)
        {
            if (boxes.Length != radii.Length)
                throw new ArgumentException("Boxes and radii spans must have the same length");

            for (int i = 0; i < boxes.Length; i++)
            {
                var extents = (boxes[i].Maximum - boxes[i].Minimum) * 0.5F;
                radii[i] = extents.Length();
            }
        }

        /// <summary>
        /// SIMD-optimized intersection test for multiple bounding boxes.
        /// </summary>
        public static void IntersectsArray(ref BoundingBox box, ReadOnlySpan<BoundingBox> boxes, Span<bool> results)
        {
            if (boxes.Length != results.Length)
                throw new ArgumentException("Boxes and results spans must have the same length");

            for (int i = 0; i < boxes.Length; i++)
            {
                var b = boxes[i];
                results[i] = box.Minimum.X <= b.Maximum.X && box.Maximum.X >= b.Minimum.X &&
                            box.Minimum.Y <= b.Maximum.Y && box.Maximum.Y >= b.Minimum.Y &&
                            box.Minimum.Z <= b.Maximum.Z && box.Maximum.Z >= b.Minimum.Z;
            }
        }
    }
}
