namespace MetricsConfiguration.Infrastructure.Environment
{
    public class CacheConfigs
    {
        public MemoryCache MemoryCache { get; set; }
        public RedisCache RedisCache { get; set; }
    }
    public class MemoryCache
    {
        public string Key { get; set; }
        public int Expriration_Minutes { get; set; }

    }
    public class RedisCache
    {
        public string Url { get; set; }
        public string Key { get; set; }
        public int Expriration_Minutes { get; set; }
    }

}

