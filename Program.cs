using System.Diagnostics;
using Autofac;
using CacheExamples;

var builder = new ContainerBuilder();
builder.RegisterType<StringCache>()
    .As<LRUCache<string, string>>()
    .SingleInstance()
    .WithParameter("capacity", 2);

builder.RegisterType<TestCache>()
    .As<LRUCache<string, TestCacheItem>>()
    .SingleInstance()
    .WithParameter("capacity", 2);

var container = builder.Build();

var stringCache = container.Resolve<LRUCache<string, string>>();
stringCache.CacheItemEvicted += (_, args) => { Console.WriteLine($"Cache item evicted: {args}"); };

stringCache.Set("1", "One");
stringCache.Set("2", "Two");
stringCache.Set("3", "Three");
stringCache.Set("4", "Four");

var cacheItem = stringCache.Get("3");
var nonExistentCacheItem = stringCache.Get("1");

Console.WriteLine(cacheItem);
Console.WriteLine(nonExistentCacheItem);



var testCache = container.Resolve<LRUCache<string, TestCacheItem>>();
testCache.CacheItemEvicted += (_, args) => { Console.WriteLine($"Cache item evicted: {args}"); };

testCache.Set("1", new TestCacheItem {Value = "One"});
testCache.Set("2", new TestCacheItem {Value = "Two"});
testCache.Set("3", new TestCacheItem {Value = "Three"});
testCache.Set("4", new TestCacheItem {Value = "Four"});

var cacheItem2 = testCache.Get("3");
var nonExistentCacheItem2 = testCache.Get("5");

Console.WriteLine(cacheItem2);
Console.WriteLine(nonExistentCacheItem2);



Console.WriteLine("Press any key to exit...");
Console.ReadLine();