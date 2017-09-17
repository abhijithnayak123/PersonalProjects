using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace TCF.Zeo.Common.Util
{
    public static class CacheWrapper
    {
        static ObjectCache cache = MemoryCache.Default;

        public static object Get(string key)
        {
            if (exists(key))
                { return cache.Get(key); }

            return null;
        }

        public static void Set(string key, object value)
        {
            cache.Set(key, value, DateTimeOffset.Now.AddDays(1).AddSeconds(-1));
        }

        private static bool exists(string key)
        {
            if (cache[key] == null)
                return false;
            return true;
        }
    }
}