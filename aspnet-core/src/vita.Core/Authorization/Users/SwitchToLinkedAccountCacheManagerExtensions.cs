using Abp.Runtime.Caching;

namespace vita.Authorization.Users
{
    public static class SwitchToLinkedAccountCacheManagerExtensions
    {
        public static ITypedCache<string, SwitchToLinkedAccountCacheItem> GetSwitchToLinkedAccountCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, SwitchToLinkedAccountCacheItem>(SwitchToLinkedAccountCacheItem.CacheName);
        }
    }
}