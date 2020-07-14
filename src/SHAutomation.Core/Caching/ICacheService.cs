namespace SHAutomation.Core.Caching
{
    public interface ICacheService
    {
        string GetCacheValue(string key, string testName);
        void SetCacheValue(string key, string value);
        string GenerateCacheKey(string testName);
    }
}
