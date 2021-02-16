using System;
using System.Threading;
using Ark;
using NUnit.Framework;

class TestWeakReferenceCache
{
	private WeakReferenceCache<int, string> _cache;

	[SetUp]
	public void Init()
	{
		_cache = new WeakReferenceCache<int, string>();
		_cache.SetCheckInterval(1);
		_cache.SetLifeTime(5);
	}

	[TearDown]
	public void Cleanup()
	{
		_cache.Clear();
		_cache = null;
	}

	[Test]
	public void Test1()
	{
		var refer1 = new object();
		var refer2 = new object();

		var d1 = "tom";
		_cache.Set(1, d1, refer1);

		var d2 = _cache.Get(1, refer2);
		Assert.AreEqual("tom", d2);
		Assert.AreSame(d1, d2);
		_cache.Update(10);

		var d22 = _cache.Get(1, null);
		Assert.AreSame(d1, d22);

		_cache.Derefer(refer1);
		_cache.Update(10);
		d22 = _cache.Get(1, null);
		Assert.AreSame(d1, d22);

		_cache.Derefer(refer2);
		_cache.Update(10);
		d22 = _cache.Get(1, null);
		Assert.Null(d22);

		_cache.Set(3, d1, null);
		var d33 = _cache.Get(3, null);
		Assert.AreSame(d1, d33);

		_cache.Update(10);
		d33 = _cache.Get(3, null);
		Assert.Null(d33);
	}

	[Test]
	public void Test2()
	{
		var d1 = "hello," + DateTime.Now.ToString();
		AddWeakRefer(1, d1);

		var d2 = _cache.Get(1, null);
		Assert.AreSame(d1, d2);
		//_cache.Update(10); // bug here ?

		//GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
		//GC.WaitForPendingFinalizers();
		//GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
		GC.Collect();
		_cache.Update(10);

		var d3 = _cache.Get(1, null);
		Assert.Null(d3);
	}

	private void AddWeakRefer(int key, string value)
	{
		_cache.Set(key, value, new object());
	}

	[Test]
	public void Test3()
	{
		var d1 = "hello," + DateTime.Now.ToString();
		AddWeakRefer(1, d1);

		var d2 = _cache.Get(1, null);
		Assert.AreSame(d1, d2);

		_cache.Remove(1);

		var d3 = _cache.Get(1, null);
		Assert.Null(d3);
	}

	[Test]
	public void Test4()
	{
		var d1 = "hello," + DateTime.Now.ToString();
		AddWeakRefer(1, d1);

		var d2 = "world";
		AddWeakRefer(1, d2);

		var d3 = _cache.Get(1, null);
		Assert.AreSame(d2, d3);
	}
}
