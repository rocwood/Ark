using System;
using System.Collections.Generic;

namespace Ark
{
	public static class DictionaryUtils
	{
		public static Dictionary<TKey, TValue> Create<TKey, TValue>(IEnumerable<TValue> data, Func<TValue, TKey> keySelector)
		{
			var result = new Dictionary<TKey, TValue>();

			foreach (var d in data)
				result.Add(keySelector(d), d);

			return result;
		}

		public static MultiDictionary<TKey, TValue> CreateMulti<TKey, TValue>(IEnumerable<TValue> data, Func<TValue, TKey> keySelector)
		{
			var result = new MultiDictionary<TKey, TValue>();

			foreach (var d in data)
				result.Add(keySelector(d), d);

			return result;
		}
	}
}
