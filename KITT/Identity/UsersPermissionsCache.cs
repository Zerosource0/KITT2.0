using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;


namespace KITTBackend.Identity
{
    public class UsersPermissionsCache
    {
        private string cachePrefix = "UsersPermissions";

        public object GetValue(string key)
        {
            //log.Debug("UsersPermissionsCache: Retrieving new object at " + key);
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get(cachePrefix + key);
        }

        public bool Add(string key, object value, DateTimeOffset absExpiration)
        {
            //log.Debug("UsersPermissionsCache: Storing new object: "  + value.GetType() + ", at " + key);
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Add(cachePrefix + key, value, absExpiration);
        }

        public void Delete(string key)
        {
            //log.Debug("UsersPermissionsCache: Deleting object at " + key);
            MemoryCache memoryCache = MemoryCache.Default;
            if (memoryCache.Contains(cachePrefix + key))
            {
                memoryCache.Remove(cachePrefix + key);
            }
        }
    }
}