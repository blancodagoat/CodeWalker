using System;
using System.Collections.Generic;
using System.IO;

namespace CodeWalker.GameFiles
{
    public class RpfStructureCache
    {
        private const uint Magic = 0x43575243; // "CWRC" - CodeWalker RPF Cache
        private const uint CacheVersion = 1;

        private Dictionary<string, CachedRpfEntry> _entries;

        private class CachedRpfEntry
        {
            public long FileSize;
            public long LastWriteTimeBinary;
            public RpfFile RpfFile;
        }

        public bool TryLoad(string cacheFilePath, bool gen9, string gtaFolder)
        {
            _entries = new Dictionary<string, CachedRpfEntry>(StringComparer.OrdinalIgnoreCase);

            if (!File.Exists(cacheFilePath))
                return false;

            try
            {
                using var fs = File.OpenRead(cacheFilePath);
                using var br = new BinaryReader(fs);

                uint magic = br.ReadUInt32();
                if (magic != Magic)
                    return false;

                uint version = br.ReadUInt32();
                if (version != CacheVersion)
                    return false;

                bool cachedGen9 = br.ReadBoolean();
                if (cachedGen9 != gen9)
                    return false;

                string cachedFolder = br.ReadString();
                if (!string.Equals(cachedFolder, gtaFolder, StringComparison.OrdinalIgnoreCase))
                    return false;

                int rpfCount = br.ReadInt32();

                for (int i = 0; i < rpfCount; i++)
                {
                    string filePath = br.ReadString();
                    long fileSize = br.ReadInt64();
                    long lastWriteTime = br.ReadInt64();

                    RpfFile rpf = ReadRpfFile(br);
                    RebuildStructure(rpf);

                    _entries[filePath] = new CachedRpfEntry
                    {
                        FileSize = fileSize,
                        LastWriteTimeBinary = lastWriteTime,
                        RpfFile = rpf
                    };
                }

                return true;
            }
            catch
            {
                _entries.Clear();
                return false;
            }
        }

        public RpfFile TryGetCached(string filePath, long fileSize, DateTime lastWriteTimeUtc)
        {
            if (_entries == null)
                return null;

            if (!_entries.TryGetValue(filePath, out var entry))
                return null;

            if (entry.FileSize != fileSize)
                return null;

            if (entry.LastWriteTimeBinary != lastWriteTimeUtc.ToBinary())
                return null;

            return entry.RpfFile;
        }

        public void Save(string cacheFilePath, bool gen9, string gtaFolder, List<(string filePath, long fileSize, DateTime lastWriteUtc, RpfFile rpf)> rpfs)
        {
            try
            {
                using var fs = File.Create(cacheFilePath);
                using var bw = new BinaryWriter(fs);

                bw.Write(Magic);
                bw.Write(CacheVersion);
                bw.Write(gen9);
                bw.Write(gtaFolder);
                bw.Write(rpfs.Count);

                foreach (var (filePath, fileSize, lastWriteUtc, rpf) in rpfs)
                {
                    bw.Write(filePath);
                    bw.Write(fileSize);
                    bw.Write(lastWriteUtc.ToBinary());
                    WriteRpfFile(bw, rpf);
                }
            }
            catch
            {
                // If save fails, try to delete partial file
                try { File.Delete(cacheFilePath); } catch { }
            }
        }

        private static void WriteRpfFile(BinaryWriter bw, RpfFile rpf)
        {
            bw.Write(rpf.Name ?? string.Empty);
            bw.Write(rpf.Path ?? string.Empty);
            bw.Write(rpf.FilePath ?? string.Empty);
            bw.Write(rpf.FileSize);
            bw.Write(rpf.StartPos);
            bw.Write(rpf.Version);
            bw.Write(rpf.EntryCount);
            bw.Write(rpf.NamesLength);
            bw.Write((uint)rpf.Encryption);
            bw.Write(rpf.IsAESEncrypted);
            bw.Write(rpf.IsNGEncrypted);
            bw.Write(rpf.TotalFileCount);
            bw.Write(rpf.TotalFolderCount);
            bw.Write(rpf.TotalResourceCount);
            bw.Write(rpf.TotalBinaryFileCount);
            bw.Write(rpf.GrandTotalRpfCount);
            bw.Write(rpf.GrandTotalFileCount);
            bw.Write(rpf.GrandTotalFolderCount);
            bw.Write(rpf.GrandTotalResourceCount);
            bw.Write(rpf.GrandTotalBinaryFileCount);
            bw.Write(rpf.ExtractedByteCount);

            // Write entries
            int entryCount = rpf.AllEntries?.Count ?? 0;
            bw.Write(entryCount);
            if (rpf.AllEntries != null)
            {
                foreach (var entry in rpf.AllEntries)
                {
                    WriteEntry(bw, entry);
                }
            }

            // Write children recursively
            int childCount = rpf.Children?.Count ?? 0;
            bw.Write(childCount);
            if (rpf.Children != null)
            {
                foreach (var child in rpf.Children)
                {
                    WriteRpfFile(bw, child);
                }
            }
        }

        private static void WriteEntry(BinaryWriter bw, RpfEntry entry)
        {
            // Entry type
            if (entry is RpfDirectoryEntry)
                bw.Write((byte)0);
            else if (entry is RpfBinaryFileEntry)
                bw.Write((byte)1);
            else if (entry is RpfResourceFileEntry)
                bw.Write((byte)2);
            else
                bw.Write((byte)0xFF); // should not happen

            // Common fields
            bw.Write(entry.H1);
            bw.Write(entry.H2);
            bw.Write(entry.NameOffset);
            bw.Write(entry.Name ?? string.Empty);
            bw.Write(entry.NameLower ?? string.Empty);

            // Type-specific fields
            if (entry is RpfDirectoryEntry dir)
            {
                bw.Write(dir.EntriesIndex);
                bw.Write(dir.EntriesCount);
            }
            else if (entry is RpfBinaryFileEntry bin)
            {
                bw.Write(bin.FileOffset);
                bw.Write(bin.FileSize);
                bw.Write(bin.IsEncrypted);
                bw.Write(bin.FileUncompressedSize);
                bw.Write(bin.EncryptionType);
            }
            else if (entry is RpfResourceFileEntry res)
            {
                bw.Write(res.FileOffset);
                bw.Write(res.FileSize);
                bw.Write(res.IsEncrypted);
                bw.Write((uint)res.SystemFlags);
                bw.Write((uint)res.GraphicsFlags);
            }
        }

        private static RpfFile ReadRpfFile(BinaryReader br)
        {
            string name = br.ReadString();
            string path = br.ReadString();
            string filePath = br.ReadString();
            long fileSize = br.ReadInt64();

            var rpf = new RpfFile(name, path, fileSize);
            rpf.FilePath = filePath;

            rpf.StartPos = br.ReadInt64();
            rpf.Version = br.ReadUInt32();
            rpf.EntryCount = br.ReadUInt32();
            rpf.NamesLength = br.ReadUInt32();
            rpf.Encryption = (RpfEncryption)br.ReadUInt32();
            rpf.IsAESEncrypted = br.ReadBoolean();
            rpf.IsNGEncrypted = br.ReadBoolean();
            rpf.TotalFileCount = br.ReadUInt32();
            rpf.TotalFolderCount = br.ReadUInt32();
            rpf.TotalResourceCount = br.ReadUInt32();
            rpf.TotalBinaryFileCount = br.ReadUInt32();
            rpf.GrandTotalRpfCount = br.ReadUInt32();
            rpf.GrandTotalFileCount = br.ReadUInt32();
            rpf.GrandTotalFolderCount = br.ReadUInt32();
            rpf.GrandTotalResourceCount = br.ReadUInt32();
            rpf.GrandTotalBinaryFileCount = br.ReadUInt32();
            rpf.ExtractedByteCount = br.ReadInt64();

            // Read entries
            int entryCount = br.ReadInt32();
            rpf.AllEntries = new List<RpfEntry>(entryCount);
            for (int i = 0; i < entryCount; i++)
            {
                var entry = ReadEntry(br);
                entry.File = rpf;
                rpf.AllEntries.Add(entry);
            }

            // Read children recursively
            int childCount = br.ReadInt32();
            rpf.Children = new List<RpfFile>(childCount);
            for (int i = 0; i < childCount; i++)
            {
                var child = ReadRpfFile(br);
                child.Parent = rpf;
                rpf.Children.Add(child);
            }

            return rpf;
        }

        private static RpfEntry ReadEntry(BinaryReader br)
        {
            byte type = br.ReadByte();

            uint h1 = br.ReadUInt32();
            uint h2 = br.ReadUInt32();
            uint nameOffset = br.ReadUInt32();
            string name = br.ReadString();
            string nameLower = br.ReadString();

            RpfEntry entry;

            switch (type)
            {
                case 0: // Directory
                {
                    var dir = new RpfDirectoryEntry();
                    dir.EntriesIndex = br.ReadUInt32();
                    dir.EntriesCount = br.ReadUInt32();
                    entry = dir;
                    break;
                }
                case 1: // Binary
                {
                    var bin = new RpfBinaryFileEntry();
                    bin.FileOffset = br.ReadUInt32();
                    bin.FileSize = br.ReadUInt32();
                    bin.IsEncrypted = br.ReadBoolean();
                    bin.FileUncompressedSize = br.ReadUInt32();
                    bin.EncryptionType = br.ReadUInt32();
                    entry = bin;
                    break;
                }
                case 2: // Resource
                {
                    var res = new RpfResourceFileEntry();
                    res.FileOffset = br.ReadUInt32();
                    res.FileSize = br.ReadUInt32();
                    res.IsEncrypted = br.ReadBoolean();
                    res.SystemFlags = br.ReadUInt32();
                    res.GraphicsFlags = br.ReadUInt32();
                    entry = res;
                    break;
                }
                default:
                    throw new InvalidDataException($"Unknown RPF entry type: {type}");
            }

            entry.H1 = h1;
            entry.H2 = h2;
            entry.NameOffset = nameOffset;
            entry.Name = name;
            entry.NameLower = nameLower;

            return entry;
        }

        private static void RebuildStructure(RpfFile rpf)
        {
            if (rpf.AllEntries == null || rpf.AllEntries.Count == 0)
                return;

            // Set root
            rpf.Root = (RpfDirectoryEntry)rpf.AllEntries[0];
            rpf.Root.Path = rpf.Path?.ToLowerInvariant() ?? string.Empty;

            // Rebuild directory tree (same logic as RpfFile.ReadHeader lines 244-269)
            var stack = new Stack<RpfDirectoryEntry>();
            stack.Push(rpf.Root);

            while (stack.Count > 0)
            {
                var item = stack.Pop();
                item.Directories = new List<RpfDirectoryEntry>();
                item.Files = new List<RpfFileEntry>();

                int starti = (int)item.EntriesIndex;
                int endi = (int)(item.EntriesIndex + item.EntriesCount);

                for (int i = starti; i < endi; i++)
                {
                    if (i < 0 || i >= rpf.AllEntries.Count)
                        break;

                    RpfEntry e = rpf.AllEntries[i];
                    e.Parent = item;
                    e.File = rpf;

                    if (e is RpfDirectoryEntry rde)
                    {
                        rde.Path = item.Path + "\\" + rde.NameLower;
                        item.Directories.Add(rde);
                        stack.Push(rde);
                    }
                    else if (e is RpfFileEntry rfe)
                    {
                        rfe.Path = item.Path + "\\" + rfe.NameLower;
                        item.Files.Add(rfe);
                    }
                }
            }

            // Rebuild child RPF parent file entry links
            if (rpf.Children != null)
            {
                foreach (var child in rpf.Children)
                {
                    child.Parent = rpf;

                    // Find the matching BinaryFileEntry in the parent's entries
                    if (rpf.AllEntries != null && child.Name != null)
                    {
                        string childNameLower = child.Name.ToLowerInvariant();
                        foreach (var entry in rpf.AllEntries)
                        {
                            if (entry is RpfBinaryFileEntry binEntry &&
                                binEntry.NameLower == childNameLower)
                            {
                                // Verify by path if possible
                                if (child.Path != null && binEntry.Path != null &&
                                    string.Equals(child.Path, binEntry.Path, StringComparison.OrdinalIgnoreCase))
                                {
                                    child.ParentFileEntry = binEntry;
                                    break;
                                }
                            }
                        }

                        // Fallback: match by name only if path match didn't work
                        if (child.ParentFileEntry == null)
                        {
                            foreach (var entry in rpf.AllEntries)
                            {
                                if (entry is RpfBinaryFileEntry binEntry &&
                                    binEntry.NameLower == childNameLower)
                                {
                                    child.ParentFileEntry = binEntry;
                                    break;
                                }
                            }
                        }
                    }

                    // Recursively rebuild children
                    RebuildStructure(child);
                }
            }
        }
    }
}
