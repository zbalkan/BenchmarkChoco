﻿using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace BenchmarkChoco
{
    public class ApplicationUninstallerFactoryCache
    {
        private ApplicationUninstallerFactoryCache(string filename, Dictionary<string, ApplicationUninstallerEntry> cacheData)
        {
            Filename = filename;
            Cache = cacheData;
        }

        public ApplicationUninstallerFactoryCache(string filename) : this(filename, new Dictionary<string, ApplicationUninstallerEntry>())
        {
        }

        public ApplicationUninstallerEntry TryGetCachedItem(ApplicationUninstallerEntry notCachedEntry)
        {
            var id = notCachedEntry?.GetCacheId();

            if (!string.IsNullOrEmpty(id) && Cache.TryGetValue(id, out var matchedEntry))
                return matchedEntry;

            return null;
        }

        public void TryCacheItem(ApplicationUninstallerEntry item)
        {
            var id = item?.GetCacheId();
            if (!string.IsNullOrEmpty(id))
                Cache[id] = item;
        }

        private Dictionary<string, ApplicationUninstallerEntry> Cache { get; }

        public string Filename { get; set; }
        public bool SerializeIcons { get; set; }

        private static byte[] SerializeIcon(Icon ic)
        {
            using (var stream = new MemoryStream())
            {
                ic.Save(stream);
                return stream.ToArray();
            }
        }

        private static Icon DeserializeIcon(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes, false))
                return new Icon(stream);
        }

        public void Read()
        {
            var result = SerializationTools.DeserializeFromXml<List<CacheEntry>>(Filename);

            Cache.Clear();

            // Ignore entries if more than 1 have the same cache id
            foreach (var group in result
                .GroupBy(x => x.Entry.GetCacheId())
                .Where(g => g.Key != null && g.CountEquals(1)))
            {
                var cacheEntry = group.Single();

                if (SerializeIcons && cacheEntry.Icon != null)
                    cacheEntry.Entry.IconBitmap = DeserializeIcon(cacheEntry.Icon);

                Cache.Add(group.Key, cacheEntry.Entry);
            }
        }

        public void Save()
        {
            SerializationTools.SerializeToXml(Filename, Cache.Select(x => new CacheEntry(
                x.Value,
                SerializeIcons && x.Value.IconBitmap != null ? SerializeIcon(x.Value.IconBitmap) : null))
                .ToList());
        }

        public class CacheEntry
        {
            public CacheEntry()
            {
            }

            public CacheEntry(ApplicationUninstallerEntry entry, byte[] icon)
            {
                Entry = entry;
                Icon = icon;
            }

            public ApplicationUninstallerEntry Entry { get; set; }
            public byte[] Icon { get; set; }
        }

        public void Delete()
        {
            File.Delete(Filename);
            Cache.Clear();
        }
    }
}