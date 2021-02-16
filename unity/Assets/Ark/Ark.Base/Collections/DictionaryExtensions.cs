using System;
using System.Collections.Generic;

namespace Ark
{
	public static class DictionaryExtensions
	{
		public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
		{
			var result = default(TValue);
			dict.TryGetValue(key, out result);
			return result;
		}

		public static TValue GetValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dict, TKey key)
		{
			var result = default(TValue);
			dict.TryGetValue(key, out result);
			return result;
		}

		public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
		{
			return ((IReadOnlyDictionary<TKey, TValue>)dict).GetValue(key);
		}

		public static TValue GetValue<TKey, TValue>(this SortedDictionary<TKey, TValue> dict, TKey key)
		{
			return ((IReadOnlyDictionary<TKey, TValue>)dict).GetValue(key);
		}

		public static void Add<TKey, TValue>(this IDictionary<TKey, List<TValue>> dict, TKey key, TValue value)
		{
			if (dict == null)
				return;

			List<TValue> list = null;
			dict.TryGetValue(key, out list);

			if (list == null)
				dict[key] = list = new List<TValue>();

			list.Add(value);
		}

		public static void AddUnique<TKey, TValue>(this IDictionary<TKey, List<TValue>> dict, TKey key, TValue value, Func<TValue, TValue, bool> comparer)
		{
			if (dict == null)
				return;

			List<TValue> list = null;
			dict.TryGetValue(key, out list);

			if (list == null)
				dict[key] = list = new List<TValue>();

			list.AddUnique(value, comparer);
		}

		public static void AddUnique<TKey, TValue>(this IDictionary<TKey, List<TValue>> dict, TKey key, TValue value, IEqualityComparer<TValue> comparer = null)
		{
			if (dict == null)
				return;

			List<TValue> list = null;
			dict.TryGetValue(key, out list);

			if (list == null)
				dict[key] = list = new List<TValue>();

			list.AddUnique(value, comparer);
		}

		public static bool Remove<TKey, TValue>(this IDictionary<TKey, List<TValue>> dict, TKey key, TValue value)
		{
			if (dict == null)
				return false;

			List<TValue> list = null;
			dict.TryGetValue(key, out list);

			if (list == null)
				return false;

			return list.Remove(value);
		}
	}
}
