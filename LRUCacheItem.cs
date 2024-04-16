namespace CacheExamples;

public class LRUCacheItem<TKey, TValue>
{
    public readonly TKey Key;
    public readonly TValue Value;

    public LRUCacheItem(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
}