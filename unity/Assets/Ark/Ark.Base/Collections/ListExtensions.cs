using System;
using System.Collections.Generic;

namespace Ark
{
	public static class ListExtensions
	{
		/// <summary>
		/// Add value to list if not existed
		/// </summary>
		public static void AddUnique<T>(this IList<T> list, T value, Func<T, T, bool> comparer)
		{
			if (list == null)
				return;

			for (int i = 0; i < list.Count; i++)
			{
				if (comparer(value, list[i]))
					return;
			}

			list.Add(value);
		}

		/// <summary>
		/// Add value to list if not existed
		/// </summary>
		public static void AddUnique<T>(this IList<T> list, T value, IEqualityComparer<T> comparer = null)
		{
			if (list == null)
				return;

			if (comparer == null)
				comparer = EqualityComparer<T>.Default;

			for (int i = 0; i < list.Count; i++)
			{
				if (comparer.Equals(value, list[i]))
					return;
			}

			list.Add(value);
		}

		/// <summary>
		/// Update list content with collection
		/// </summary>
		public static void Assign<T>(this IList<T> list, IEnumerable<T> collection)
		{
			if (list == null)
				return;

			list.Clear();

			if (collection == null)
				return;

			var c = collection as IReadOnlyList<T>;
			if (c != null)
			{
				for (int i = 0; i < c.Count; i++)
					list.Add(c[i]);
			}
			else
			{
				using (var e = collection.GetEnumerator())
				{
					while (e.MoveNext())
						list.Add(e.Current);
				}
			}
		}

		public static bool SequenceEqual<T>(this IList<T> list, IList<T> other, Func<T, T, bool> comparer)
		{
			if (list == null && other == null)
				return true;
			if (list == null || other == null)
				return false;
			if (list.Count != other.Count)
				return false;

			for (int i = 0; i < list.Count; ++i)
			{
				if (!comparer(list[i], other[i]))
					return false;
			}

			return true;
		}

		public static bool SequenceEqual<T>(this IList<T> list, IList<T> other, IEqualityComparer<T> comparer = null)
		{
			if (list == null && other == null)
				return true;
			if (list == null || other == null)
				return false;
			if (list.Count != other.Count)
				return false;

			if (comparer == null)
				comparer = EqualityComparer<T>.Default;

			for (int i = 0; i < list.Count; ++i)
			{
				if (!comparer.Equals(list[i], other[i]))
					return false;
			}

			return true;
		}
	}
}
