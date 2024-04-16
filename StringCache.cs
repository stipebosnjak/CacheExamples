namespace CacheExamples;

public class StringCache(int capacity) : LRUCache<string, string>(capacity);

public class TestCache(int capacity) : LRUCache<string, TestCacheItem>(capacity);


