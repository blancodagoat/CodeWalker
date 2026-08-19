using System;
using System.Collections.Generic;
using System.IO;
using CodeWalker.GameFiles;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace CodeWalker.Utils
{
    // Puts an oversized texture pack on a diet without visibly hurting it. Oversized and
    // uncompressed clothing textures exhaust the game's VRAM budget, which shows up in game as
    // "stuck on low detail / textures gone" until a restart.
    //
    // Input: a folder of loose ytd/ydd files, or an .rpf archive (eg a FiveM clothing resource).
    // Output: either an .rpf with the same internal layout, or a loose folder laid out for
    // texoverride's tex_overrides (caret names like "collection^file.ytd" become
    // "collection\file.ytd", everything else lands at the root).
    //
    // Policy (quality first):
    //   - textures already BC-compressed, within the size cap, with mips: carried over unchanged
    //   - larger than the cap: downscaled (Lanczos) to the cap by power-of-two steps
    //   - uncompressed: DXT1 (opaque) / DXT5 (has alpha), the same formats vanilla uses
    //   - ATI1/ATI2/BC7 sources keep their format family so shaders see what they expect
    //   - missing mip chains are regenerated
    //
    // Compression runs through TextureCompressor (NVTT), the same encoder the texture import
    // uses. Input files are never modified.
    public static class PackShrinker
    {
        public class ShrinkStats
        {
            public int FilesChanged;
            public int FilesCopied;
            public int FilesSkipped;
            public int FilesFailed;
            public int TexturesChanged;
            public long MemBefore;
            public long MemAfter;
        }

        private class SrcItem
        {
            public string Rel;              // path inside the source (folder rel path or rpf internal path)
            public string FilePath;         // set for folder sources
            public RpfFileEntry Entry;      // set for rpf sources
            public bool FromRpf;            // rpf-sourced items get their layout mapped for tex_overrides
            public string Name => Entry?.Name ?? Path.GetFileName(FilePath);
        }

        public static ShrinkStats ShrinkPack(string input, string output, int cap, bool outputRpf, Action<string> log, Func<bool> abort = null)
        {
            log?.Invoke("Encoder: " + (TextureCompressor.IsNvttAvailable ? "NVTT" : "managed (BCn)"));

            bool srcIsRpf = File.Exists(input) && input.EndsWith(".rpf", StringComparison.OrdinalIgnoreCase);
            var stats = new ShrinkStats();
            var items = new List<SrcItem>();

            if (srcIsRpf)
            {
                var rpf = new RpfFile(input, Path.GetFileName(input));
                rpf.ScanStructure(null, s => log?.Invoke("rpf: " + s));
                CollectRpfItems(rpf, "", items, log);
            }
            else if (Directory.Exists(input))
            {
                foreach (var f in Directory.EnumerateFiles(input, "*.*", SearchOption.AllDirectories))
                {
                    var rel = Path.GetRelativePath(input, f);
                    if (f.EndsWith(".rpf", StringComparison.OrdinalIgnoreCase))
                    {
                        // a pack inside the folder: unpack its contents into the work list,
                        // merged as if it had been extracted where it sits
                        log?.Invoke($"found {rel} - unpacking it into the output");
                        try
                        {
                            var rpf = new RpfFile(f, Path.GetFileName(f));
                            rpf.ScanStructure(null, s => log?.Invoke("rpf: " + s));
                            var dir = Path.GetDirectoryName(rel);
                            CollectRpfItems(rpf, string.IsNullOrEmpty(dir) ? "" : dir + "\\", items, log);
                        }
                        catch (Exception ex)
                        {
                            log?.Invoke($"FAILED  {rel} - could not open rpf: {ex.Message}");
                            stats.FilesFailed++;
                        }
                        continue;
                    }
                    items.Add(new SrcItem { Rel = rel, FilePath = f });
                }
            }
            else
            {
                log?.Invoke("Input not found: " + input);
                return null;
            }

            RpfFile outRpf = null;
            var outDirs = new Dictionary<string, RpfDirectoryEntry>(StringComparer.OrdinalIgnoreCase);
            if (outputRpf)
            {
                if (File.Exists(output)) File.Delete(output);
                outRpf = RpfFile.CreateNew("", output);   // output is a full path
                outDirs[""] = outRpf.Root;
            }
            else
            {
                Directory.CreateDirectory(output);
            }

            foreach (var it in items)
            {
                if (abort?.Invoke() == true)
                {
                    log?.Invoke("Aborted.");
                    return stats;
                }

                var ext = Path.GetExtension(it.Name).ToLowerInvariant();
                bool isTex = (ext == ".ytd") || (ext == ".ydd");

                // where this file lands in the output
                string outRel = outputRpf ? it.Rel : MapLoosePath(it);
                if (outRel == null)
                {
                    stats.FilesSkipped++;   // not useful in a tex_overrides folder (metas, ymts, ...)
                    continue;
                }

                string destPath = outputRpf ? null : Path.Combine(output, outRel);
                if ((destPath != null) && File.Exists(destPath))
                {
                    log?.Invoke($"SKIPPED  {it.Rel} - {outRel} already written (duplicate name)");
                    stats.FilesSkipped++;
                    continue;
                }

                long oldMem = SrcMem(it);
                stats.MemBefore += oldMem;

                byte[] shrunk = null;
                if (isTex && (oldMem > 0))
                {
                    try
                    {
                        shrunk = ShrinkTexFile(it, ext, cap, stats);
                    }
                    catch (Exception ex)
                    {
                        log?.Invoke($"FAILED  {it.Rel} - {ex.Message}; carried over unchanged");
                        stats.FilesFailed++;
                        shrunk = null;
                    }
                }

                // rebuilt file must actually be smaller - resource page rounding can eat a
                // marginal win, and then the untouched original is the better file
                if ((shrunk != null) && (RscMem(shrunk) >= oldMem))
                {
                    shrunk = null;
                }

                byte[] outData = shrunk ?? Passthrough(it);
                if (outData == null)
                {
                    log?.Invoke($"FAILED  {it.Rel} - could not read; dropped");
                    stats.FilesFailed++;
                    continue;
                }

                long newMem = (shrunk != null) ? RscMem(outData) : oldMem;
                stats.MemAfter += newMem;
                if (shrunk != null)
                {
                    stats.FilesChanged++;
                    log?.Invoke($"{oldMem / 1048576.0,7:F1} -> {newMem / 1048576.0,5:F1} MB  {outRel}");
                }
                else
                {
                    stats.FilesCopied++;
                }

                if (outputRpf)
                {
                    var dir = GetRpfDir(outRpf, outDirs, Path.GetDirectoryName(outRel));
                    RpfFile.CreateFile(dir, Path.GetFileName(outRel), outData);
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                    File.WriteAllBytes(destPath, outData);
                }
            }

            log?.Invoke("");
            log?.Invoke($"Done: {stats.FilesChanged} file(s) shrunk, {stats.FilesCopied} carried unchanged, {stats.FilesSkipped} skipped, {stats.FilesFailed} failed.");
            log?.Invoke($"Pack cost in game memory: {stats.MemBefore / 1048576.0:F1} -> {stats.MemAfter / 1048576.0:F1} MB ({stats.TexturesChanged} texture(s) re-encoded).");
            return stats;
        }

        private static void CollectRpfItems(RpfFile rpf, string prefix, List<SrcItem> items, Action<string> log)
        {
            if (rpf.AllEntries == null) return;
            foreach (var e in rpf.AllEntries)
            {
                if (e is not RpfFileEntry fe) continue;
                if (fe.NameLower.EndsWith(".rpf")) continue;   // nested rpfs handled below
                // entry.Path starts with the rpf's own name; the rel path is everything after it
                var rel = fe.Path;
                int cut = rel.IndexOf('\\');
                rel = (cut >= 0) ? rel.Substring(cut + 1) : fe.Name;
                items.Add(new SrcItem { Rel = prefix + rel, Entry = fe, FromRpf = true });
            }
            if (rpf.Children != null)
            {
                foreach (var child in rpf.Children)
                {
                    log?.Invoke($"note: nested {child.Name} is unpacked into the output, not kept as an rpf");
                    var rel = child.Path;
                    int cut = rel.IndexOf('\\');
                    rel = (cut >= 0) ? rel.Substring(cut + 1) : child.Name;
                    CollectRpfItems(child, prefix + rel + "\\", items, log);
                }
            }
        }

        // tex_overrides layout: "collection^file" carets become folders; rpf-internal folders
        // (stream etc) are flattened away; only textures are useful in a tex_overrides folder.
        private static string MapLoosePath(SrcItem it)
        {
            var name = it.Name;
            int caret = name.IndexOf('^');
            if (caret > 0)
            {
                return name.Substring(0, caret) + "\\" + name.Substring(caret + 1);
            }
            // a file directly inside a ped collection folder goes to "collection\file", however
            // deeply the collection folder itself was buried (wrapper folders flattened away)
            var parent = Path.GetFileName(Path.GetDirectoryName(it.Rel) ?? "").ToLowerInvariant();
            if (parent.StartsWith("mp_m_freemode_01") || parent.StartsWith("mp_f_freemode_01"))
            {
                return parent + "\\" + name;
            }
            var ext = Path.GetExtension(name).ToLowerInvariant();
            bool isTex = (ext == ".ytd") || (ext == ".ydd");
            if (it.FromRpf && !isTex) return null;          // metas/ymts are useless in tex_overrides
            if (it.FromRpf) return name;                    // bare overlay txd (or something texoverride will log and skip)
            if ((ext == ".ytd") && (parent.Length > 0)) return name;   // buried overlay txd -> root
            return it.Rel;                                  // root files and everything else: keep the structure
        }

        private static RpfDirectoryEntry GetRpfDir(RpfFile rpf, Dictionary<string, RpfDirectoryEntry> dirs, string relDir)
        {
            relDir ??= "";
            if (dirs.TryGetValue(relDir, out var dir)) return dir;
            var parent = GetRpfDir(rpf, dirs, Path.GetDirectoryName(relDir));
            dir = RpfFile.CreateDirectory(parent, Path.GetFileName(relDir));
            dirs[relDir] = dir;
            return dir;
        }

        // Loads a ytd/ydd from either source, shrinks its textures, and returns the rebuilt
        // loose-format file - or null when nothing in it needed touching.
        private static byte[] ShrinkTexFile(SrcItem it, string ext, int cap, ShrinkStats stats)
        {
            bool changed = false;
            if (ext == ".ytd")
            {
                YtdFile ytd;
                if (it.Entry != null) ytd = RpfFile.GetFile<YtdFile>(it.Entry);
                else { ytd = new YtdFile(); ytd.Load(File.ReadAllBytes(it.FilePath)); }   // loose-file overload
                if (ytd == null) throw new Exception("could not load");
                changed = ShrinkDict(ytd.TextureDict, cap, stats);
                return changed ? ytd.Save() : null;
            }
            YddFile ydd;
            if (it.Entry != null) ydd = RpfFile.GetFile<YddFile>(it.Entry);
            else { ydd = new YddFile(); ydd.Load(File.ReadAllBytes(it.FilePath)); }       // loose-file overload
            if (ydd == null) throw new Exception("could not load");
            if (ydd.Drawables != null)
            {
                foreach (var dr in ydd.Drawables)
                {
                    var td = dr?.ShaderGroup?.TextureDictionary;
                    if (td != null) changed |= ShrinkDict(td, cap, stats);
                }
            }
            return changed ? ydd.Save() : null;
        }

        // The file in loose (on-disk) format, unchanged: folder sources are read back as-is;
        // rpf resources get their RSC7 header rebuilt, rpf binaries come out raw.
        private static byte[] Passthrough(SrcItem it)
        {
            if (it.FilePath != null) return File.ReadAllBytes(it.FilePath);
            var data = it.Entry.File.ExtractFile(it.Entry);
            if (data == null) return null;
            if (it.Entry is RpfResourceFileEntry re)
            {
                return ResourceBuilder.AddResourceHeader(re, ResourceBuilder.Compress(data));
            }
            return data;
        }

        private static long SrcMem(SrcItem it)
        {
            if (it.Entry is RpfResourceFileEntry re)
            {
                return SizeFromFlags(re.SystemFlags) + SizeFromFlags(re.GraphicsFlags);
            }
            if (it.FilePath != null)
            {
                Span<byte> hdr = stackalloc byte[16];
                using var fs = File.OpenRead(it.FilePath);
                if (fs.Read(hdr) != 16 || BitConverter.ToUInt32(hdr.Slice(0, 4)) != 0x37435352) return 0;
                return SizeFromFlags(BitConverter.ToUInt32(hdr.Slice(8, 4))) + SizeFromFlags(BitConverter.ToUInt32(hdr.Slice(12, 4)));
            }
            return 0;
        }

        private static bool ShrinkDict(TextureDictionary dict, int cap, ShrinkStats stats)
        {
            var texs = dict?.Textures?.data_items;
            if (texs == null) return false;
            bool changed = false;
            for (int i = 0; i < texs.Length; i++)
            {
                var nt = ShrinkTexture(texs[i], cap);
                if (nt == null) continue;
                texs[i] = nt;
                if ((dict.Dict != null) && (nt.NameHash != 0)) dict.Dict[nt.NameHash] = nt;
                changed = true;
                if (stats != null) stats.TexturesChanged++;
            }
            return changed;
        }

        // Returns a re-encoded replacement, or null when the texture is already sane.
        // Throws when the texture needs work but compression fails, so callers can report it.
        public static Texture ShrinkTexture(Texture tex, int cap)
        {
            if (tex?.Data?.FullData == null) return null;
            int w = tex.Width, h = tex.Height;

            bool isBC = tex.Format is TextureFormat.D3DFMT_DXT1 or TextureFormat.D3DFMT_DXT3 or TextureFormat.D3DFMT_DXT5
                                    or TextureFormat.D3DFMT_ATI1 or TextureFormat.D3DFMT_ATI2 or TextureFormat.D3DFMT_BC7;
            bool tooBig = (w > cap) || (h > cap);
            bool noMips = (tex.Levels <= 1) && (Math.Max(w, h) >= 256);
            if (isBC && !tooBig && !noMips) return null;                                         // already sane
            if ((w < 4) || (h < 4) || ((w & (w - 1)) != 0) || ((h & (h - 1)) != 0)) return null; // odd sizes: leave alone

            var px = DDSIO.GetPixels(tex, 0); // BGRA, mip 0
            if (px == null) return null;

            int nw = w, nh = h;
            while ((nw > cap) || (nh > cap)) { nw /= 2; nh /= 2; }

            bool alpha = false;
            for (int p = 3; p < px.Length; p += 4)
            {
                if (px[p] < 250) { alpha = true; break; }
            }

            var pixels = px;
            if ((nw != w) || (nh != h))
            {
                using var img = Image.LoadPixelData<Bgra32>(px, w, h);
                img.Mutate(x => x.Resize(nw, nh, KnownResamplers.Lanczos3));
                pixels = new byte[nw * nh * 4];
                img.CopyPixelDataTo(pixels);
            }

            var fmt = tex.Format switch
            {
                TextureFormat.D3DFMT_ATI1 => TextureCompressionFormat.BC4,
                TextureFormat.D3DFMT_ATI2 => TextureCompressionFormat.BC5,
                TextureFormat.D3DFMT_BC7 => TextureCompressionFormat.BC7,
                _ => alpha ? TextureCompressionFormat.DXT5 : TextureCompressionFormat.DXT1,
            };

            var res = TextureCompressor.CompressPixels(pixels, nw, nh, fmt, TextureCompressionQuality.Normal, true, true, 4);
            if (res?.Success != true || res.Texture == null)
                throw new Exception(res?.ErrorMessage ?? "compression failed");

            var nt = res.Texture;

            // the mission is memory: when the re-encode would not actually shrink the texture
            // (mips added to a mipless one, DXT1 promoted to DXT5 over a stray alpha bit), the
            // original wins - it is both smaller and untouched
            if ((nt.Data?.FullData == null) || (nt.Data.FullData.Length >= tex.Data.FullData.Length)) return null;

            nt.Name = tex.Name;
            nt.NameHash = tex.NameHash;
            nt.Usage = tex.Usage;
            nt.UsageFlags = tex.UsageFlags;
            nt.ExtraFlags = tex.ExtraFlags;
            return nt;
        }

        // Exact in-game memory charge of a loose resource file, decoded from its RSC7 header.
        public static long RscMem(byte[] data)
        {
            if ((data == null) || (data.Length < 16) || (BitConverter.ToUInt32(data, 0) != 0x37435352)) return 0;
            return SizeFromFlags(BitConverter.ToUInt32(data, 8)) + SizeFromFlags(BitConverter.ToUInt32(data, 12));
        }
        private static long SizeFromFlags(uint f)
        {
            long pages = ((f >> 27) & 0x1) + (((f >> 26) & 0x1) << 1) + (((f >> 25) & 0x1) << 2)
                       + (((f >> 24) & 0x1) << 3) + (((f >> 17) & 0x7F) << 4) + (((f >> 11) & 0x3F) << 5)
                       + (((f >> 7) & 0xF) << 6) + (((f >> 5) & 0x3) << 7) + (((f >> 4) & 0x1) << 8);
            return (0x200L << (int)(f & 0xF)) * pages;
        }
    }
}
