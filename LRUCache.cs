using System.Diagnostics;

namespace CacheExamples;

using System.Collections.Generic;

public abstract class LRUCache<TKey, TValue> 
{
    private readonly int _capacity;
    private readonly Dictionary<TKey, LinkedListNode<LRUCacheItem<TKey, TValue>>> _cacheItems;
    private readonly LinkedList<LRUCacheItem<TKey, TValue>> _linkedList;
    private readonly object _lock;

    public event EventHandler CacheItemEvicted;

    protected LRUCache(int capacity)
    {
        _capacity = capacity;
        _lock = new object();
        _cacheItems = new Dictionary<TKey, LinkedListNode<LRUCacheItem<TKey, TValue>>>(capacity);
        _linkedList = new LinkedList<LRUCacheItem<TKey, TValue>>();
    }

    public TValue Get(TKey key)
    {
        lock (_lock)
        {
            if (_cacheItems.TryGetValue(key, out var node))
            {
                var cacheItem = node.Value;
                _linkedList.Remove(node);
                _linkedList.AddFirst(node);

                return cacheItem.Value;
            }

            return default(TValue);
        }
    }


    public void Set(TKey key, TValue value)
    {
        lock (_lock)
        {
            if (_cacheItems.TryGetValue(key, out var node))
            {
                _linkedList.Remove(node);
            }
            else
            {
                if (_cacheItems.Count >= _capacity)
                {
                    var cacheItem = _linkedList.Last?.Value;
                    _linkedList.RemoveLast();
                    _cacheItems.Remove(cacheItem.Key);

                    var cacheItemEvictedEventArgs = new CacheItemEvictedEventArgs<TKey, TValue>
                    {
                        CacheItem = cacheItem,
                        EvictedTime = DateTime.Now
                    };
                    OnCacheItemEvicted(cacheItemEvictedEventArgs);
                }

                node = new LinkedListNode<LRUCacheItem<TKey, TValue>>(new LRUCacheItem<TKey, TValue>(key, value));
                _cacheItems.Add(key, node);
            }

            _linkedList.AddFirst(node);
        }
    }

    protected virtual void OnCacheItemEvicted(CacheItemEvictedEventArgs<TKey, TValue> e)
    {
        CacheItemEvicted?.Invoke(this, e);
    }
}


public class CacheItemEvictedEventArgs<TKey, TValue> : EventArgs
{
    public LRUCacheItem<TKey, TValue> CacheItem { get; set; }
    public DateTime EvictedTime { get; set; }

    public override string ToString()
    {
        return $"Key = {CacheItem.Key}, Value = {CacheItem.Value}, EvictedTime = {EvictedTime:g}";
    }
}