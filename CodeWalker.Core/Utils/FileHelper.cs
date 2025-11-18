using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CodeWalker.Core.Utils;

/// <summary>
/// Helper class for async file I/O operations with proper ConfigureAwait usage for library code
/// </summary>
public static class FileHelper
{
    /// <summary>
    /// Asynchronously reads all bytes from a file
    /// </summary>
    /// <param name="path">The file path to read from</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Byte array containing the file contents</returns>
    public static async Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default)
    {
        return await File.ReadAllBytesAsync(path, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously writes all bytes to a file
    /// </summary>
    /// <param name="path">The file path to write to</param>
    /// <param name="bytes">The bytes to write</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public static async Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default)
    {
        await File.WriteAllBytesAsync(path, bytes, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Synchronously reads all bytes from a file (for backward compatibility)
    /// </summary>
    /// <param name="path">The file path to read from</param>
    /// <returns>Byte array containing the file contents</returns>
    public static byte[] ReadAllBytes(string path)
    {
        return File.ReadAllBytes(path);
    }

    /// <summary>
    /// Synchronously writes all bytes to a file (for backward compatibility)
    /// </summary>
    /// <param name="path">The file path to write to</param>
    /// <param name="bytes">The bytes to write</param>
    public static void WriteAllBytes(string path, byte[] bytes)
    {
        File.WriteAllBytes(path, bytes);
    }

    /// <summary>
    /// Checks if a file exists
    /// </summary>
    /// <param name="path">The file path to check</param>
    /// <returns>True if the file exists, false otherwise</returns>
    public static bool Exists(string path)
    {
        return File.Exists(path);
    }
}
