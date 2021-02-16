using System;
using System.Collections.Generic;

namespace Ark
{
	public static class ReadOnlyListExtensions
	{
		public static bool Contains<T>(this IReadOnlyList<T> list, T value)
		{
			return list.IndexOf(value) != -1;
		}

		public static int IndexOf<T>(this IReadOnlyList<T> list, T value)
		{
			int count = list.Count;

			if (value == null)
			{
				for (int i = 0; i < count; i++)
					if (list[i] == null)
						return i;
			}
			else
			{
				var comparer = EqualityComparer<T>.Default;
				for (int i = 0; i < count; i++)
					if (comparer.Equals(list[i], value))
						return i;
			}

			return -1;
		}

		public static bool Exists<T>(this IReadOnlyList<T> list, Predicate<T> match)
		{
			return list.FindIndex(match) != -1;
		}

		public static T Find<T>(this IReadOnlyList<T> list, Predicate<T> match)
		{
			for (int i = 0; i < list.Count; i++)
			{
				var item = list[i];

				if (match(item))
					return item;
			}

			return default(T);
		}

		public static int FindIndex<T>(this IReadOnlyList<T> list, Predicate<T> match)
		{
			for (int i = 0; i < list.Count; i++)
			{
				var item = list[i];

				if (match(item))
					return i;
			}

			return -1;
		}

		public static List<TOutput> ConvertAll<T, TOutput>(this IReadOnlyList<T> list, Converter<T, TOutput> convert)
		{
			var result = new List<TOutput>(list.Count);

			for (int i = 0; i < list.Count; i++)
				result.Add(convert(list[i]));

			return result;
		}
	}
}
