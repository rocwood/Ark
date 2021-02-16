using System;
using System.Collections.Generic;

namespace Ark
{
	/// <summary>
	/// Shared empty object holder
	/// </summary>
	public static class Empty
	{
		public static T Get<T>() where T:class,new() => TypeHolder<T>.Get();
		public static T[] GetArray<T>() => ArrayHolder<T>.Get();
		public static List<T> GetList<T>() => TypeHolder<List<T>>.Get();
		public static Dictionary<K, V> GetDict<K, V>() => TypeHolder<Dictionary<K, V>>.Get();
		public static SortedDictionary<K, V> GetSortedDict<K, V>() => TypeHolder<SortedDictionary<K, V>>.Get();
		public static MultiDictionary<K, V> GetMultiDict<K, V>() => TypeHolder<MultiDictionary<K, V>>.Get();

		static class ArrayHolder<T>
		{
			private static volatile T[] _instance = null;

			public static T[] Get()
			{
				if (_instance == null)
					_instance = (T[])_cache.GetOrAdd(typeof(T[]), _factoryMethod);

				return _instance;
			}

			static object FactoryMethod(Type _) => new T[0];
			static Func<Type, object> _factoryMethod = FactoryMethod;
			static ArrayHolder() {}
		}

		static class TypeHolder<T> where T:class, new()
		{
			private static volatile T _instance = null;

			public static T Get()
			{
				if (_instance == null)
					_instance = (T)_cache.GetOrAdd(typeof(T), _factoryMethod);

				return _instance;
			}

			static object FactoryMethod(Type _) => new T();
			static Func<Type, object> _factoryMethod = FactoryMethod;
			static TypeHolder() {}
		}

		private static readonly SynchronizedCache<Type, object> _cache = new SynchronizedCache<Type, object>();
	}
}
