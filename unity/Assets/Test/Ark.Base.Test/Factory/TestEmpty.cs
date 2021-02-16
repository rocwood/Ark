using System.Collections.Generic;
using Ark;
using NUnit.Framework;

class TestEmpty
{
	[SetUp]
	public void Init()
	{
	}

	[Test]
	public void Test1()
	{
		int[] arr1 = Empty.GetArray<int>();
		int[] arr2 = Empty.GetArray<int>();

		Assert.Zero(arr1.Length);
		Assert.AreSame(arr1, arr2);

		byte[] arr3 = Empty.GetArray<byte>();
		Assert.AreNotSame(arr1, arr3);

		List<int> list1 = Empty.GetList<int>();
		List<int> list2 = Empty.GetList<int>();

		Assert.Zero(list1.Count);
		Assert.AreSame(list1, list2);

		Dictionary<int, string> dict1 = Empty.GetDict<int, string>();
		Dictionary<int, string> dict2 = Empty.GetDict<int, string>();

		Assert.Zero(dict1.Count);
		Assert.AreSame(dict1, dict2);

		SortedDictionary<int, object> sdict1 = Empty.GetSortedDict<int, object>();
		SortedDictionary<int, object> sdict2 = Empty.GetSortedDict<int, object>();

		Assert.Zero(sdict1.Count);
		Assert.AreSame(sdict1, sdict2);

		MultiDictionary<int, object> mdict1 = Empty.GetMultiDict<int, object>();
		MultiDictionary<int, object> mdict2 = Empty.GetMultiDict<int, object>();

		Assert.Zero(mdict1.Count);
		Assert.AreSame(mdict1, mdict2);

		object o1 = Empty.Get<object>();
		object o2 = Empty.Get<object>();
		Assert.NotNull(o1);
		Assert.AreSame(o1, o2);
	}
}
