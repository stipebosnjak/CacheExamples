namespace CacheExamples;

public class TestCacheItem
{
    public string Value { get; set; }
    public override string ToString()
    {
        return $"Value: {Value};";
    }
}