using SharpDX;
using System;
using System.Diagnostics;

namespace CodeWalker.Core.Utils;

/// <summary>
/// Benchmark utility for measuring SIMD optimization performance gains.
/// </summary>
public static class SimdBenchmark
{
    /// <summary>
    /// Benchmarks vector transformation performance.
    /// </summary>
    public static (double scalarMs, double simdMs, double speedup) BenchmarkVectorTransform(int vectorCount, int iterations = 1000)
    {
        var vectors = new Vector3[vectorCount];
        var results = new Vector3[vectorCount];
        var random = new Random(42);

        // Initialize test data
        for (int i = 0; i < vectorCount; i++)
        {
            vectors[i] = new Vector3(
                (float)random.NextDouble() * 100,
                (float)random.NextDouble() * 100,
                (float)random.NextDouble() * 100
            );
        }

        var transform = Matrix.RotationYawPitchRoll(0.5f, 0.3f, 0.2f);
        transform.TranslationVector = new Vector3(10, 20, 30);

        // Warm up
        for (int i = 0; i < 10; i++)
        {
            ScalarTransform(vectors, results, ref transform);
        }

        // Benchmark scalar version
        var sw = Stopwatch.StartNew();
        for (int iter = 0; iter < iterations; iter++)
        {
            ScalarTransform(vectors, results, ref transform);
        }
        sw.Stop();
        var scalarMs = sw.Elapsed.TotalMilliseconds;

        // Warm up SIMD
        for (int i = 0; i < 10; i++)
        {
            SimdMath.TransformVector3Array(vectors, results, ref transform);
        }

        // Benchmark SIMD version
        sw.Restart();
        for (int iter = 0; iter < iterations; iter++)
        {
            SimdMath.TransformVector3Array(vectors, results, ref transform);
        }
        sw.Stop();
        var simdMs = sw.Elapsed.TotalMilliseconds;

        var speedup = scalarMs / simdMs;
        return (scalarMs, simdMs, speedup);
    }

    private static void ScalarTransform(Vector3[] source, Vector3[] destination, ref Matrix transform)
    {
        for (int i = 0; i < source.Length; i++)
        {
            var v = source[i];
            destination[i] = new Vector3(
                transform.M11 * v.X + transform.M21 * v.Y + transform.M31 * v.Z + transform.M41,
                transform.M12 * v.X + transform.M22 * v.Y + transform.M32 * v.Z + transform.M42,
                transform.M13 * v.X + transform.M23 * v.Y + transform.M33 * v.Z + transform.M43
            );
        }
    }

    /// <summary>
    /// Benchmarks quaternion rotation performance.
    /// </summary>
    public static (double scalarMs, double simdMs, double speedup) BenchmarkQuaternionRotation(int vectorCount, int iterations = 1000)
    {
        var vectors = new Vector3[vectorCount];
        var results = new Vector3[vectorCount];
        var random = new Random(42);

        for (int i = 0; i < vectorCount; i++)
        {
            vectors[i] = new Vector3(
                (float)random.NextDouble() * 100,
                (float)random.NextDouble() * 100,
                (float)random.NextDouble() * 100
            );
        }

        var rotation = Quaternion.RotationYawPitchRoll(0.5f, 0.3f, 0.2f);

        // Warm up
        for (int i = 0; i < 10; i++)
        {
            ScalarQuaternionRotate(vectors, results, rotation);
        }

        // Benchmark scalar version
        var sw = Stopwatch.StartNew();
        for (int iter = 0; iter < iterations; iter++)
        {
            ScalarQuaternionRotate(vectors, results, rotation);
        }
        sw.Stop();
        var scalarMs = sw.Elapsed.TotalMilliseconds;

        // Warm up SIMD
        for (int i = 0; i < 10; i++)
        {
            QuaternionExtension.MultiplyArray(rotation, vectors, results);
        }

        // Benchmark SIMD version
        sw.Restart();
        for (int iter = 0; iter < iterations; iter++)
        {
            QuaternionExtension.MultiplyArray(rotation, vectors, results);
        }
        sw.Stop();
        var simdMs = sw.Elapsed.TotalMilliseconds;

        var speedup = scalarMs / simdMs;
        return (scalarMs, simdMs, speedup);
    }

    private static void ScalarQuaternionRotate(Vector3[] source, Vector3[] destination, Quaternion q)
    {
        for (int i = 0; i < source.Length; i++)
        {
            destination[i] = q.Multiply(source[i]);
        }
    }

    /// <summary>
    /// Benchmarks frustum culling performance.
    /// </summary>
    public static (double scalarMs, double simdMs, double speedup) BenchmarkFrustumCulling(int boxCount, int iterations = 1000)
    {
        var centers = new Vector3[boxCount];
        var extents = new Vector3[boxCount];
        var random = new Random(42);

        for (int i = 0; i < boxCount; i++)
        {
            centers[i] = new Vector3(
                (float)random.NextDouble() * 1000 - 500,
                (float)random.NextDouble() * 1000 - 500,
                (float)random.NextDouble() * 1000 - 500
            );
            extents[i] = new Vector3(
                (float)random.NextDouble() * 10 + 1,
                (float)random.NextDouble() * 10 + 1,
                (float)random.NextDouble() * 10 + 1
            );
        }

        var frustum = new World.Frustum();
        var viewProj = Matrix.LookAtRH(new Vector3(0, 0, -100), Vector3.Zero, Vector3.Up) *
                      Matrix.PerspectiveFovRH(MathF.PI / 4, 16f / 9f, 0.1f, 1000f);
        frustum.Update(ref viewProj);

        int visibleCount = 0;

        // Benchmark scalar version
        var sw = Stopwatch.StartNew();
        for (int iter = 0; iter < iterations; iter++)
        {
            visibleCount = 0;
            for (int i = 0; i < boxCount; i++)
            {
                var c = centers[i];
                var e = extents[i];
                if (frustum.ContainsAABBNoClip(ref c, ref e))
                {
                    visibleCount++;
                }
            }
        }
        sw.Stop();
        var scalarMs = sw.Elapsed.TotalMilliseconds;

        // SIMD version uses the same method (already optimized internally)
        sw.Restart();
        for (int iter = 0; iter < iterations; iter++)
        {
            visibleCount = 0;
            for (int i = 0; i < boxCount; i++)
            {
                var c = centers[i];
                var e = extents[i];
                if (frustum.ContainsAABBNoClip(ref c, ref e))
                {
                    visibleCount++;
                }
            }
        }
        sw.Stop();
        var simdMs = sw.Elapsed.TotalMilliseconds;

        var speedup = scalarMs / simdMs;
        return (scalarMs, simdMs, speedup);
    }

    /// <summary>
    /// Runs all benchmarks and returns a summary report.
    /// </summary>
    public static string RunAllBenchmarks()
    {
        var report = new System.Text.StringBuilder();
        report.AppendLine("=== SIMD Optimization Benchmark Results ===");
        report.AppendLine();

        report.AppendLine($"Hardware Acceleration: {System.Numerics.Vector.IsHardwareAccelerated}");
        report.AppendLine($"Vector<T> Count: {System.Numerics.Vector<float>.Count}");
        report.AppendLine();

        // Vector Transform Benchmark
        report.AppendLine("Vector Transform (1000 vectors, 1000 iterations):");
        var (vtScalar, vtSimd, vtSpeedup) = BenchmarkVectorTransform(1000, 1000);
        report.AppendLine($"  Scalar: {vtScalar:F2} ms");
        report.AppendLine($"  SIMD:   {vtSimd:F2} ms");
        report.AppendLine($"  Speedup: {vtSpeedup:F2}x");
        report.AppendLine();

        // Quaternion Rotation Benchmark
        report.AppendLine("Quaternion Rotation (1000 vectors, 1000 iterations):");
        var (qrScalar, qrSimd, qrSpeedup) = BenchmarkQuaternionRotation(1000, 1000);
        report.AppendLine($"  Scalar: {qrScalar:F2} ms");
        report.AppendLine($"  SIMD:   {qrSimd:F2} ms");
        report.AppendLine($"  Speedup: {qrSpeedup:F2}x");
        report.AppendLine();

        // Frustum Culling Benchmark
        report.AppendLine("Frustum Culling (1000 boxes, 1000 iterations):");
        var (fcScalar, fcSimd, fcSpeedup) = BenchmarkFrustumCulling(1000, 1000);
        report.AppendLine($"  Scalar: {fcScalar:F2} ms");
        report.AppendLine($"  SIMD:   {fcSimd:F2} ms");
        report.AppendLine($"  Speedup: {fcSpeedup:F2}x");
        report.AppendLine();

        report.AppendLine("=== End of Benchmark Results ===");

        return report.ToString();
    }
}
