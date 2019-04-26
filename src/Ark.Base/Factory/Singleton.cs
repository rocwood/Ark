using System;
using System.Collections.Generic;

namespace Ark
{
	/// <summary>
	/// Thread-safe, UnitTest-friendly, Type/Singleton holder
	/// </summary>
	public static class Singleton
	{
		public static T Get<T>(Func<Type, object> factory = null) where T:class,new()
		{
			return TypeHolder<T>.Get(factory);
		}

		public static object Get(Type type, Func<Type, object> factory = null)
		{
			return _cache.GetOrAdd(type, factory ?? Activator.CreateInstance);
		}

		public static void ClearAll()
		{
			_cache.Clear();

			foreach (var clearMethod in _clearMethods)
				clearMethod?.Invoke();
		}

		static class TypeHolder<T> where T : class, new()
		{
			private static volatile T _instance = null;

			public static T Get(Func<Type, object> factory)
			{
				if (_instance == null)
					_instance = (T)Singleton.Get(typeof(T), factory ?? _factoryMethod);

				return _instance;
			}

			static void Clear()
			{
				_instance = null;
			}

			static object DefaultFactory(Type _) => new T();
			static Func<Type, object> _factoryMethod = DefaultFactory;

			static TypeHolder() { _clearMethods.Add(Clear); }
		}

		private static readonly SynchronizedCache<Type, object> _cache = new SynchronizedCache<Type, object>();
		private static readonly List<Action> _clearMethods = new List<Action>();
	}
}
