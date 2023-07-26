namespace dgt.redis.api.Models
{
    public class CacheResponseMetadata
    {
        public string? Message { get; set; }
        public CacheLocation LoadLocation { get; set; } = CacheLocation.None;
        public bool IsCachedData { get; set; } = false;
        public string RecordKey { get; set; } = $"Users_{DateTime.Now:yyyyMMdd_hhmm}";
    }
}
