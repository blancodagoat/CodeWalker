using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeWalker
{
    public class Cache<TKey, TVal> where TKey : notnull where TVal : Cacheable<TKey>
    {
        public long MaxMemoryUsage = 536870912; //512mb
        public long CurrentMemoryUsage = 0;
        public double CacheTime = 10.0; //seconds to keep something that's not used
        public DateTime CurrentTime = DateTime.Now;

        private LinkedList<TVal> loadedList = new();
        private Dictionary<TKey, LinkedListNode<TVal>> loadedListDict = new();
        private readonly object cacheLock = new();
        private int frameCounter = 0;
        private const int CompactInterval = 60; // Compact every 60 frames (~1 second at 60fps)

        public int Count
        {
            get
            {
                lock (cacheLock)
                {
                    return loadedList.Count;
                }
            }
        }

        public Cache()
        {
        }
        public Cache(long maxMemoryUsage, double cacheTime)
        {
            MaxMemoryUsage = maxMemoryUsage;
            CacheTime = cacheTime;
        }

        public void BeginFrame()
        {
            CurrentTime = DateTime.Now;
            frameCounter++;

            // Only compact periodically or when memory pressure is high
            if (frameCounter >= CompactInterval || (CurrentMemoryUsage > MaxMemoryUsage * 0.8))
            {
                Compact();
                frameCounter = 0;
            }
        }

        public TVal? TryGet(TKey key)
        {
            lock (cacheLock)
            {
                LinkedListNode<TVal>? lln = null;
                if (loadedListDict.TryGetValue(key, out lln))
                {
                    // Only update timestamp, avoid expensive LinkedList reordering
                    lln.Value.LastUseTime = CurrentTime;
                }
                return (lln != null) ? lln.Value : null;
            }
        }
        public bool TryAdd(TKey key, TVal item)
        {
            lock (cacheLock)
            {
                if (item.MemoryUsage == 0)
                {
                }
                item.Key = key;
                if (CanAdd())
                {
                    var lln = loadedList.AddLast(item);
                    loadedListDict.Add(key, lln);
                    Interlocked.Add(ref CurrentMemoryUsage, item.MemoryUsage);
                    return true;
                }
                else
                {
                    // Cache full - batch evict old items in a single pass
                    long memoryNeeded = item.MemoryUsage;
                    long memoryToFree = memoryNeeded + (MaxMemoryUsage / 10); // Free 10% extra headroom
                    long memoryFreed = 0;
                    var cachetime = CacheTime;

                    // Collect items to remove in a single pass
                    List<LinkedListNode<TVal>> toRemove = new();
                    var oldlln = loadedList.First;

                    while (oldlln != null && memoryFreed < memoryToFree)
                    {
                        if ((CurrentTime - oldlln.Value.LastUseTime).TotalSeconds > cachetime)
                        {
                            toRemove.Add(oldlln);
                            memoryFreed += oldlln.Value.MemoryUsage;
                        }
                        oldlln = oldlln.Next;
                    }

                    // If not enough freed with current cachetime, try more aggressive eviction
                    if (memoryFreed < memoryNeeded && toRemove.Count > 0)
                    {
                        cachetime *= 0.5;
                        oldlln = loadedList.First;
                        while (oldlln != null && memoryFreed < memoryToFree)
                        {
                            if ((CurrentTime - oldlln.Value.LastUseTime).TotalSeconds > cachetime && !toRemove.Contains(oldlln))
                            {
                                toRemove.Add(oldlln);
                                memoryFreed += oldlln.Value.MemoryUsage;
                            }
                            oldlln = oldlln.Next;
                        }
                    }

                    // Batch remove all collected items
                    foreach (var node in toRemove)
                    {
                        Interlocked.Add(ref CurrentMemoryUsage, -node.Value.MemoryUsage);
                        loadedListDict.Remove(node.Value.Key);
                        loadedList.Remove(node);
                        node.Value = default!;
                    }

                    if (CanAdd())
                    {
                        var newlln = loadedList.AddLast(item);
                        loadedListDict.Add(key, newlln);
                        Interlocked.Add(ref CurrentMemoryUsage, item.MemoryUsage);
                        return true;
                    }
                }
                return false;
            }
        }

        public bool CanAdd()
        {
            return Interlocked.Read(ref CurrentMemoryUsage) < MaxMemoryUsage;
        }


        public void Clear()
        {
            lock (cacheLock)
            {
                loadedList.Clear();
                loadedListDict.Clear();
                CurrentMemoryUsage = 0;
            }
        }

        public void Remove(TKey key)
        {
            lock (cacheLock)
            {
                LinkedListNode<TVal>? n;
                if (loadedListDict.TryGetValue(key, out n))
                {
                    loadedListDict.Remove(key);
                    loadedList.Remove(n);
                    Interlocked.Add(ref CurrentMemoryUsage, -n.Value.MemoryUsage);
                }
            }
        }


        public void Compact()
        {
            lock (cacheLock)
            {
                var oldlln = loadedList.First;
                while (oldlln != null)
                {
                    if ((CurrentTime - oldlln.Value.LastUseTime).TotalSeconds < CacheTime) break;
                    var nextln = oldlln.Next;
                    Interlocked.Add(ref CurrentMemoryUsage, -oldlln.Value.MemoryUsage);
                    loadedListDict.Remove(oldlln.Value.Key);
                    loadedList.Remove(oldlln); //gc should free up memory later..
                    oldlln.Value = default!;
                    oldlln = nextln;
                }
            }
        }


    }

    public abstract class Cacheable<TKey> where TKey : notnull
    {
        public TKey Key = default!;
        public DateTime LastUseTime;
        public long MemoryUsage;
    }

}
