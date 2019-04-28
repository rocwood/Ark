using Ark;
using NUnit.Framework;

class TestSingleton
{
	[SetUp]
	public void Init()
	{
		Singleton.ClearAll();
	}

	[Test]
	public void Test1()
	{
		var d1 = Singleton.Get<MySingletonClass>();
		var d2 = Singleton.Get(typeof(MySingletonClass));

		Assert.NotNull(d1);
		Assert.AreSame(d1, d2);

		var d3 = MySingletonClass.Instance3();
		Assert.NotNull(d3);
		Assert.AreNotSame(d1, d3);
	}

	class MySingletonClass
	{
		public static MySingletonClass Instance() => Singleton.Get<MySingletonClass>();

		private static MySingletonClass _instance3 = new MySingletonClass();
		public static MySingletonClass Instance3() => _instance3;

		static MySingletonClass() { }
	}
}
