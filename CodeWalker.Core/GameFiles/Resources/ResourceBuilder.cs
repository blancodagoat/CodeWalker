using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWalker.GameFiles
{
    public class ResourceBuilder
    {
        protected const int RESOURCE_IDENT = 0x37435352;
        protected const int BASE_SIZE = 0x2000;
        private const int SKIP_SIZE = 16;//512;//256;//64;
        private const int ALIGN_SIZE = 16;//512;//64;

        public class ResourceBuilderBlock
        {
            public IResourceBlock Block;
            public long Length;

            public ResourceBuilderBlock(IResourceBlock block)
            {
                Block = block;
                Length = block?.BlockLength ?? 0;
            }
        }
        public class ResourceBuilderBlockSet
        {
            public bool IsSystemSet = false;
            public ResourceBuilderBlock RootBlock = null;
            public LinkedList<ResourceBuilderBlock> BlockList = new();
            public Dictionary<ResourceBuilderBlock, LinkedListNode<ResourceBuilderBlock>> BlockDict = new Dictionary<ResourceBuilderBlock, LinkedListNode<ResourceBuilderBlock>>();

            public int Count => BlockList.Count;

            public ResourceBuilderBlockSet(IList<IResourceBlock> blocks, bool sys)
            {
                IsSystemSet = sys;
                if (sys && (blocks.Count > 0))
                {
                    RootBlock = new ResourceBuilderBlock(blocks[0]);
                }
                var list = new List<ResourceBuilderBlock>();
                int start = sys ? 1 : 0;
                for (int i = start; i < blocks.Count; i++)
                {
                    var bb = new ResourceBuilderBlock(blocks[i]);
                    list.Add(bb);
                }
                list.Sort((a, b) => b.Length.CompareTo(a.Length));
                foreach (var bb in list)
                {
                    var ln = BlockList.AddLast(bb);
                    BlockDict[bb] = ln;
                }
            }

            public ResourceBuilderBlock FindBestBlock(long maxSize)
            {
                var n = BlockList.First;
                while ((n != null) && (n.Value.Length > maxSize))
                {
                    n = n.Next;
                }
                return n?.Value;
            }

            public ResourceBuilderBlock TakeBestBlock(long maxSize)
            {
                var r = FindBestBlock(maxSize);
                if (r != null)
                {
                    if (BlockDict.TryGetValue(r, out LinkedListNode<ResourceBuilderBlock> ln))
                    {
                        BlockList.Remove(ln);
                        BlockDict.Remove(r);
                    }
                }
                return r;
            }

        }

        public static void GetBlocks(IResourceBlock rootBlock, out IList<IResourceBlock> sys, out IList<IResourceBlock> gfx)
        {
            var systemBlocks = new HashSet<IResourceBlock>();
            var graphicBlocks = new HashSet<IResourceBlock>();
            var processed = new HashSet<IResourceBlock>();


            void addBlock(IResourceBlock block)
            {
                if (block is IResourceSystemBlock)
                {
                    if (!systemBlocks.Contains(block)) systemBlocks.Add(block);
                }
                else if(block is IResourceGraphicsBlock)
                {
                    if (!graphicBlocks.Contains(block)) graphicBlocks.Add(block);
                }
            }
            void addChildren(IResourceBlock block)
            {
                if (block is IResourceSystemBlock sblock)
                {
                    var references = sblock.GetReferences();
                    foreach (var reference in references)
                    {
                        if (!processed.Contains(reference))
                        {
                            processed.Add(reference);
                            addBlock(reference);
                            addChildren(reference);
                        }
                    }
                    var parts = sblock.GetParts();
                    foreach (var part in parts)
                    {
                        addChildren(part.Item2);
                    }
                }
            }

            addBlock(rootBlock);
            addChildren(rootBlock);


            sys = new List<IResourceBlock>();
            foreach (var s in systemBlocks)
            {
                sys.Add(s);
            }
            gfx = new List<IResourceBlock>();
            foreach (var s in graphicBlocks)
            {
                gfx.Add(s);
            }
        }

        public static void AssignPositions(IList<IResourceBlock> blocks, uint basePosition, out RpfResourcePageFlags pageFlags, uint maxPageCount, bool gen9 = false)
        {
            if ((blocks.Count > 0) && (blocks[0] is Meta))
            {
                //use naive packing strategy for Meta resources, due to crashes caused by the improved packing
                AssignPositionsForMeta(blocks, basePosition, out pageFlags);
                return;
            }

            if (blocks.Count == 0)
            {
                pageFlags = new RpfResourcePageFlags();
                return;
            }

            var sys = (basePosition == 0x50000000);
            var leafSize = BASE_SIZE; // 0x2000 (8192)

            uint CeilPow2(uint x)
            {
                x--;
                x |= x >> 1;
                x |= x >> 2;
                x |= x >> 4;
                x |= x >> 8;
                x |= x >> 16;
                return x + 1;
            }

            uint CountOnes(uint x)
            {
                x -= ((x >> 1) & 0x55555555);
                x = (((x >> 2) & 0x33333333) + (x & 0x33333333));
                x = (((x >> 4) + x) & 0x0f0f0f0f);
                x += (x >> 8);
                x += (x >> 16);
                return x & 0x3f;
            }

            uint Log2OfPow2(uint x)
            {
                return CountOnes(x - 1);
            }

            // Process blocks in original order (don't sort)
            var blockList = new List<(IResourceBlock block, long size, uint alignMask)>();

            for (int i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];
                long size = gen9 ? block.BlockLength_Gen9 : block.BlockLength;
                uint alignMask = ALIGN_SIZE - 1;
                size = (size + alignMask) & ~alignMask; // Round up to alignment
                blockList.Add((block, size, alignMask));
            }

            // Find largest allocation
            long largestAllocBytes = 0;
            foreach (var (block, size, alignMask) in blockList)
            {
                if (size > largestAllocBytes)
                    largestAllocBytes = size;
            }

            // For system heap, account for potential root block + largest allocation alignment
            if (sys && blockList.Count > 1)
            {
                long combinedSize = blockList[0].size;
                for (int i = 1; i < blockList.Count; i++)
                {
                    long testSize = blockList[0].size;
                    testSize = (testSize + blockList[i].alignMask) & ~blockList[i].alignMask;
                    testSize += blockList[i].size;
                    if (testSize > combinedSize)
                        combinedSize = testSize;
                }
                if (combinedSize > largestAllocBytes)
                    largestAllocBytes = combinedSize;
            }

            uint largestAlloc = CeilPow2((uint)((largestAllocBytes + leafSize - 1) / leafSize));
            if (largestAlloc < 16)
                largestAlloc = 16;

            // [0]=16x(Head16,1bit), [1]=8x(Head8,2bit), [2]=4x(Head4,4bit), [3]=2x(Head2,6bit), [4]=1x(Base,7bit), [5-8]=tails(1bit each)
            uint[] bucketMax = { 0x1, 0x3, 0xF, 0x3F, 0x7F, 1, 1, 1, 1 };

            uint baseAlloc = 0;
            uint iterationCount = 0;
            uint[]? finalBucketCounts = null;
            Dictionary<IResourceBlock, (int bucketIdx, long offset)>? finalAssignments = null;
            List<(long size, long offset, uint shift)>? finalBuckets = null;

            // Try different base allocations until we find one that works
            while (true)
            {
                baseAlloc = (largestAlloc << (int)iterationCount) >> 4;
                iterationCount++;

                if (baseAlloc == 0)
                    baseAlloc = 1;

                var buckets = new List<(long size, long offset, uint shift, uint splitAlloc, uint allocCount)>();
                var bucketCounts = new uint[9];
                var assignments = new Dictionary<IResourceBlock, (int bucketIdx, long offset)>();

                // Pack allocations into buckets
                bool success = true;
                for (int a = 0; a < blockList.Count; a++)
                {
                    var (block, size, alignMask) = blockList[a];

                    // Find first bucket that fits
                    int b = 0;
                    for (b = 0; b < buckets.Count; b++)
                    {
                        long newOffset = (buckets[b].offset + alignMask) & ~alignMask;
                        newOffset += size;
                        if (newOffset <= buckets[b].size)
                            break;
                    }

                    // Need new bucket?
                    if (b == buckets.Count)
                    {
                        if (buckets.Count >= maxPageCount)
                        {
                            success = false;
                            break;
                        }

                        long bucketSize = (a == 0) ? largestAllocBytes : size;
                        uint leafCount = (uint)((bucketSize + leafSize - 1) / leafSize);
                        leafCount = CeilPow2(leafCount);

                        if (leafCount < baseAlloc)
                            leafCount = baseAlloc;

                        uint countIdx = (Log2OfPow2(baseAlloc) - Log2OfPow2(leafCount)) + 4;

                        if (countIdx >= bucketMax.Length || bucketCounts[countIdx] >= bucketMax[countIdx])
                        {
                            success = false;
                            break;
                        }

                        bucketCounts[countIdx]++;
                        long newSize = leafCount * leafSize;

                        // Store countIdx as shift for now (used for sorting)
                        // Smaller countIdx = larger pages (16x=0, 8x=1, 4x=2, 2x=3, 1x=4, tails=5-8)
                        buckets.Add((newSize, 0, countIdx, uint.MaxValue, 0));
                        b = buckets.Count - 1;
                    }

                    // Assign allocation to bucket
                    var bucket = buckets[b];
                    long offset = (bucket.offset + alignMask) & ~alignMask;

                    // Track split point (where bucket crosses 50% full)
                    long halfSize = bucket.size >> 1;
                    if (bucket.allocCount > 0 && bucket.offset <= halfSize && offset + size > halfSize)
                        bucket.splitAlloc = (uint)a;

                    bucket.offset = offset + size;
                    bucket.allocCount++;
                    buckets[b] = bucket;

                    assignments[block] = (b, offset);
                }

                if (!success)
                    continue;

                // Try bucket splitting to reduce fragmentation
                uint maxShift = (uint)bucketCounts.Length - 1;
                while (true)
                {
                    if (buckets.Count == 0)
                        break;

                    int lastIdx = buckets.Count - 1;
                    var lastBucket = buckets[lastIdx];

                    // Check if we can split the last bucket
                    if (lastBucket.splitAlloc != uint.MaxValue &&
                        lastBucket.offset <= (lastBucket.size * 3 / 4) &&
                        lastBucket.shift < maxShift - 1 &&
                        baseAlloc > 2 &&
                        buckets.Count < maxPageCount)
                    {
                        long newSize = lastBucket.size >> 2;
                        long newOffset = 0;
                        uint newCount = 0;

                        // Test if split allocations fit
                        bool canSplit = true;
                        for (uint a = lastBucket.splitAlloc; a < blockList.Count; a++)
                        {
                            var (block, _, _) = blockList[(int)a];
                            if (assignments.TryGetValue(block, out var assign) && assign.bucketIdx == lastIdx)
                            {
                                newCount++;
                                var (_, size, alignMask) = blockList[(int)a];
                                newOffset = (newOffset + alignMask) & ~alignMask;
                                newOffset += size;
                                if (newOffset > newSize)
                                {
                                    canSplit = false;
                                    break;
                                }
                            }
                        }

                        if (!canSplit)
                            break;

                        // Perform the split
                        newOffset = 0;
                        uint newSplit = uint.MaxValue;

                        for (uint a = lastBucket.splitAlloc; a < blockList.Count; a++)
                        {
                            var (block, size, alignMask) = blockList[(int)a];
                            if (assignments.TryGetValue(block, out var assign) && assign.bucketIdx == lastIdx)
                            {
                                long offset = (newOffset + alignMask) & ~alignMask;

                                if (a != lastBucket.splitAlloc && newOffset <= (newSize >> 1) && offset + size > (newSize >> 1))
                                    newSplit = a;

                                newOffset = offset + size;
                                assignments[block] = (lastIdx + 1, offset);
                            }
                        }

                        // Update counts
                        lastBucket.size >>= 1;
                        bucketCounts[lastBucket.shift]--;
                        lastBucket.shift++;
                        bucketCounts[lastBucket.shift]++;
                        baseAlloc >>= 2;
                        lastBucket.splitAlloc = uint.MaxValue;
                        lastBucket.allocCount -= newCount;
                        buckets[lastIdx] = lastBucket;

                        buckets.Add((newSize, newOffset, lastBucket.shift + 1, newSplit, newCount));
                        bucketCounts[lastBucket.shift + 1]++;
                    }
                    else
                    {
                        break;
                    }
                }

                // Shrink last bucket if less than half full
                if (buckets.Count > 0)
                {
                    int lastIdx = buckets.Count - 1;
                    var lastBucket = buckets[lastIdx];

                    while (lastBucket.offset <= (lastBucket.size >> 1) && lastBucket.shift < maxShift && baseAlloc > 1)
                    {
                        lastBucket.size >>= 1;
                        bucketCounts[lastBucket.shift]--;
                        lastBucket.shift++;
                        bucketCounts[lastBucket.shift]++;
                        baseAlloc >>= 1;
                    }
                    buckets[lastIdx] = lastBucket;
                }

                // Verify bucket counts fit in header format (match bucketMax limits)
                bool valid = true;
                if (bucketCounts[0] > 0x1 || bucketCounts[1] > 0x3 || bucketCounts[2] > 0xF ||
                    bucketCounts[3] > 0x3F || bucketCounts[4] > 0x7F || bucketCounts[5] > 1 ||
                    bucketCounts[6] > 1 || bucketCounts[7] > 1 || bucketCounts[8] > 1)
                {
                    valid = false;
                }

                if (valid)
                {
                    finalBucketCounts = bucketCounts;
                    finalAssignments = assignments;
                    finalBuckets = [.. buckets.Select(b => (b.size, b.offset, b.shift))];
                    break;
                }

                if (iterationCount > 20)
                    throw new Exception("Unable to pack resource blocks into valid page configuration");
            }

            // Ensure we found a valid configuration
            if (finalBuckets == null || finalAssignments == null || finalBucketCounts == null)
                throw new Exception("Failed to allocate resource pages");

            // Calculate base shift from final base allocation
            uint baseShift = Log2OfPow2(baseAlloc);

            // Create a mapping from old bucket index to new sorted position
            // Sort by size descending (largest buckets first in memory)
            var bucketOrder = Enumerable.Range(0, finalBuckets.Count)
                .OrderByDescending(i => finalBuckets[i].size)
                .ThenBy(i => i) // Stable sort by original index for same-size buckets
                .ToList();

            // Assign linear addresses to buckets in size-sorted order
            long[] bucketBases = new long[finalBuckets.Count];
            long currentBase = 0;
            foreach (int originalBucketIdx in bucketOrder)
            {
                bucketBases[originalBucketIdx] = currentBase;
                currentBase += finalBuckets[originalBucketIdx].size;
            }

            // Assign final file positions using the bucket bases
            foreach (var kvp in finalAssignments)
            {
                var block = kvp.Key;
                var (bucketIdx, offset) = kvp.Value;
                block.FilePosition = basePosition + bucketBases[bucketIdx] + offset;
            }

            // Build page flags from bucket counts
            // Bits 0-3: baseShift (LeafShift)
            // Bit 4: Head16Count (16x pages)
            // Bits 5-6: Head8Count (8x pages)
            // Bits 7-10: Head4Count (4x pages)
            // Bits 11-16: Head2Count (2x pages)
            // Bits 17-23: BaseCount (1x pages)
            // Bit 24: HasTail2 (0.5x page)
            // Bit 25: HasTail4 (0.25x page)
            // Bit 26: HasTail8 (0.125x page)
            // Bit 27: HasTail16 (0.0625x page)
            // Bits 28-31: Version (split between virtual/physical)
            uint flagsValue = baseShift & 0xF;
            flagsValue |= (finalBucketCounts[0] & 0x1) << 4;   // Head16Count
            flagsValue |= (finalBucketCounts[1] & 0x3) << 5;   // Head8Count
            flagsValue |= (finalBucketCounts[2] & 0xF) << 7;   // Head4Count
            flagsValue |= (finalBucketCounts[3] & 0x3F) << 11; // Head2Count
            flagsValue |= (finalBucketCounts[4] & 0x7F) << 17; // BaseCount
            flagsValue |= (finalBucketCounts[5] & 0x1) << 24;  // HasTail2
            flagsValue |= (finalBucketCounts[6] & 0x1) << 25;  // HasTail4
            flagsValue |= (finalBucketCounts[7] & 0x1) << 26;  // HasTail8
            flagsValue |= (finalBucketCounts[8] & 0x1) << 27;  // HasTail16

            pageFlags = new RpfResourcePageFlags(flagsValue);
        }

        public static void AssignPositionsForMeta(IList<IResourceBlock> blocks, uint basePosition, out RpfResourcePageFlags pageFlags)
        {
            // find largest structure
            long largestBlockSize = 0;
            foreach (var block in blocks)
            {
                if (largestBlockSize < block.BlockLength)
                    largestBlockSize = block.BlockLength;
            }

            // find minimum page size
            long currentPageSize = 0x2000;
            while (currentPageSize < largestBlockSize)
                currentPageSize *= 2;

            long currentPageCount;
            long currentPosition;
            while (true)
            {
                currentPageCount = 0;
                currentPosition = 0;

                // reset all positions
                foreach (var block in blocks)
                    block.FilePosition = -1;

                foreach (var block in blocks)
                {
                    if (block.FilePosition != -1)
                        throw new Exception("Block was already assigned a position!");

                    // check if new page is necessary...
                    // if yes, add a new page and align to it
                    long maxSpace = currentPageCount * currentPageSize - currentPosition;
                    if (maxSpace < (block.BlockLength + SKIP_SIZE))
                    {
                        currentPageCount++;
                        currentPosition = currentPageSize * (currentPageCount - 1);
                    }

                    // set position
                    block.FilePosition = basePosition + currentPosition;
                    currentPosition += block.BlockLength; // + SKIP_SIZE; //is padding everywhere really necessary??

                    // align...
                    if ((currentPosition % ALIGN_SIZE) != 0)
                        currentPosition += (ALIGN_SIZE - (currentPosition % ALIGN_SIZE));
                }

                // break if everything fits...
                if (currentPageCount < 128)
                    break;

                currentPageSize *= 2;
            }

            pageFlags = new RpfResourcePageFlags(RpfResourceFileEntry.GetFlagsFromBlocks((uint)currentPageCount, (uint)currentPageSize, 0));

        }


        public static byte[] Build(ResourceFileBase fileBase, int version, bool compress = true, bool gen9 = false)
        {

            fileBase.FilePagesInfo = new ResourcePagesInfo();

            IList<IResourceBlock> systemBlocks;
            IList<IResourceBlock> graphicBlocks;
            GetBlocks(fileBase, out systemBlocks, out graphicBlocks);

            AssignPositions(systemBlocks, 0x50000000, out var systemPageFlags, 128, gen9);
            AssignPositions(graphicBlocks, 0x60000000, out var graphicsPageFlags, 128 - systemPageFlags.Count, gen9);


            fileBase.FilePagesInfo.SystemPagesCount = (byte)systemPageFlags.Count;
            fileBase.FilePagesInfo.GraphicsPagesCount = (byte)graphicsPageFlags.Count;


            var systemStream = new MemoryStream();
            var graphicsStream = new MemoryStream();
            var resourceWriter = new ResourceDataWriter(systemStream, graphicsStream);
            resourceWriter.IsGen9 = gen9;

            resourceWriter.Position = 0x50000000;
            foreach (var block in systemBlocks)
            {
                resourceWriter.Position = block.FilePosition;

                var pos_before = resourceWriter.Position;
                block.Write(resourceWriter);
                var pos_after = resourceWriter.Position;
                var blen = resourceWriter.IsGen9 ? block.BlockLength_Gen9 : block.BlockLength;

                if ((pos_after - pos_before) != blen)
                {
                    throw new Exception("error in system length");
                }
            }

            resourceWriter.Position = 0x60000000;
            foreach (var block in graphicBlocks)
            {
                resourceWriter.Position = block.FilePosition;

                var pos_before = resourceWriter.Position;
                block.Write(resourceWriter);
                var pos_after = resourceWriter.Position;
                var blen = resourceWriter.IsGen9 ? block.BlockLength_Gen9 : block.BlockLength;

                if ((pos_after - pos_before) != blen)
                {
                    throw new Exception("error in graphics length");
                }
            }




            var sysDataSize = (int)systemPageFlags.Size;
            var sysData = new byte[sysDataSize];
            systemStream.Flush();

            // Pad stream to match calculated page size if needed
            if (systemStream.Length < sysDataSize)
            {
                systemStream.SetLength(sysDataSize);
            }

            systemStream.Position = 0;
            systemStream.Read(sysData, 0, sysDataSize);


            var gfxDataSize = (int)graphicsPageFlags.Size;
            var gfxData = new byte[gfxDataSize];
            graphicsStream.Flush();

            // Pad stream to match calculated page size if needed
            if (graphicsStream.Length < gfxDataSize)
            {
                graphicsStream.SetLength(gfxDataSize);
            }

            graphicsStream.Position = 0;
            graphicsStream.Read(gfxData, 0, gfxDataSize);



            uint uv = (uint)version;
            uint sv = (uv >> 4) & 0xF;
            uint gv = (uv >> 0) & 0xF;
            uint sf = systemPageFlags.Value + (sv << 28);
            uint gf = graphicsPageFlags.Value + (gv << 28);


            var tdatasize = sysDataSize + gfxDataSize;
            var tdata = new byte[tdatasize];
            Buffer.BlockCopy(sysData, 0, tdata, 0, sysDataSize);
            Buffer.BlockCopy(gfxData, 0, tdata, sysDataSize, gfxDataSize);


            var cdata = compress ? Compress(tdata) : tdata;


            var dataSize = 16 + cdata.Length;
            var data = new byte[dataSize];

            byte[] h1 = BitConverter.GetBytes((uint)0x37435352);
            byte[] h2 = BitConverter.GetBytes((int)version);
            byte[] h3 = BitConverter.GetBytes(sf);
            byte[] h4 = BitConverter.GetBytes(gf);
            Buffer.BlockCopy(h1, 0, data, 0, 4);
            Buffer.BlockCopy(h2, 0, data, 4, 4);
            Buffer.BlockCopy(h3, 0, data, 8, 4);
            Buffer.BlockCopy(h4, 0, data, 12, 4);
            Buffer.BlockCopy(cdata, 0, data, 16, cdata.Length);

            return data;
        }






        public static byte[] AddResourceHeader(RpfResourceFileEntry entry, byte[] data)
        {
            if (data == null) return null;
            byte[] newdata = new byte[data.Length + 16];
            byte[] h1 = BitConverter.GetBytes((uint)0x37435352);
            byte[] h2 = BitConverter.GetBytes(entry.Version);
            byte[] h3 = BitConverter.GetBytes(entry.SystemFlags);
            byte[] h4 = BitConverter.GetBytes(entry.GraphicsFlags);
            Buffer.BlockCopy(h1, 0, newdata, 0, 4);
            Buffer.BlockCopy(h2, 0, newdata, 4, 4);
            Buffer.BlockCopy(h3, 0, newdata, 8, 4);
            Buffer.BlockCopy(h4, 0, newdata, 12, 4);
            Buffer.BlockCopy(data, 0, newdata, 16, data.Length);
            return newdata;
        }


        public static byte[] Compress(byte[] data)
        {
            using (MemoryStream ms = new())
            {
                DeflateStream ds = new(ms, CompressionMode.Compress, true);
                ds.Write(data, 0, data.Length);
                ds.Close();
                byte[] deflated = ms.GetBuffer();
                byte[] outbuf = new byte[ms.Length]; //need to copy to the right size buffer...
                Array.Copy(deflated, outbuf, outbuf.Length);
                return outbuf;
            }
        }
        public static byte[] Decompress(byte[] data)
        {
            using (MemoryStream ms = new(data))
            {
                DeflateStream ds = new(ms, CompressionMode.Decompress);
                MemoryStream outstr = new();
                ds.CopyTo(outstr);
                byte[] deflated = outstr.GetBuffer();
                byte[] outbuf = new byte[outstr.Length]; //need to copy to the right size buffer...
                Array.Copy(deflated, outbuf, outbuf.Length);
                return outbuf;
            }
        }

    }
}
