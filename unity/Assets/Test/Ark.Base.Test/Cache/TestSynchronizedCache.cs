using System;
using System.Threading;
using System.Threading.Tasks;
using Ark;
using NUnit.Framework;

class TestSynchronizedCache
{
	[SetUp]
	public void Init()
	{
	}

	[Test]
	public void Test1()
	{
		var cache = new SynchronizedCache<int, string>();

		cache.Set(1, "tom");
		cache.Set(2, "jerry");

		var s1 = cache.Get(1);
		var s2 = cache.Get(2);
		var s3 = cache.Get(3);

		Assert.AreEqual("tom", s1);
		Assert.AreEqual("jerry", s2);
		Assert.Null(s3);

		var s11 = cache.Get(1);
		Assert.AreSame(s1, s11);

		var s22 = cache.GetOrAdd(2, "jerry2");
		Assert.AreEqual("jerry", s22);
		Assert.AreSame(s2, s22);

		s3 = cache.GetOrAdd(3, "alice");
		Assert.AreEqual("alice", s3);

		var s33 = cache.Get(3);
		Assert.AreEqual("alice", s33);
		Assert.AreSame(s3, s33);

		var s4 = cache.GetOrAdd(4, (_) => "mike");
		Assert.AreEqual("mike", s4);

		var s44 = cache.Get(4);
		Assert.AreSame(s4, s44);
	}

	[Test]
	public void Test2()
	{
		var cache = new SynchronizedCache<int, string>();

		Func<string> func = () => cache.GetOrAdd(1, "john" + Thread.CurrentThread.ManagedThreadId);

		// create 10 threads accessing cache concurrently
		var results = new Task<string>[10];

		for (int i = 0; i < results.Length; i++)
			results[i] = Task.Run<string>(func);

		Task.WaitAll(results);

		for (int i = 0; i < results.Length; i++)
			Assert.AreSame(results[0].Result, results[i].Result);
	}
}
