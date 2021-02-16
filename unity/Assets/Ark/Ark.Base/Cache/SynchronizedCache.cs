using System;
using System.Collections.Generic;
using System.Threading;

namespace Ark
{
	/// <summary>
	/// Simple thread-safe Cache using Dictionary/ReaderWriterLockSlim
	/// </summary>
	public class SynchronizedCache<TKey, TValue>
	{
		private readonly Dictionary<TKey, TValue> _dict = new Dictionary<TKey, TValue>();
		private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

		public TValue Get(TKey key)
		{
			var result = default(TValue);

			_lock.EnterReadLock();
			try
			{
				_dict.TryGetValue(key, out result);
			}
			finally
			{
				_lock.ExitReadLock();
			}

			return result;
		}

		public void Set(TKey key, TValue value)
		{
			_lock.EnterWriteLock();
			try
			{
				_dict[key] = value;
			}
			finally
			{
				_lock.ExitWriteLock();
			}
		}

		public void Set(TKey key, Func<TKey, TValue> valueFactory)
		{
			_lock.EnterWriteLock();
			try
			{
				_dict[key] = valueFactory.Invoke(key);
			}
			finally
			{
				_lock.ExitWriteLock();
			}
		}

		public void Remove(TKey key)
		{
			_lock.EnterWriteLock();
			try
			{
				_dict.Remove(key);
			}
			finally
			{
				_lock.ExitWriteLock();
			}
		}

		/// <summary>
		/// Return existing value if key already exists.
		/// Otherwise, add new key/value and return the new value.
		/// </summary>
		public TValue GetOrAdd(TKey key, TValue value)
		{
			var result = default(TValue);

			_lock.EnterUpgradeableReadLock();
			try
			{

				if (!_dict.TryGetValue(key, out result))
				{
					_lock.EnterWriteLock();
					try
					{

						// double-check locking pattern
						if (!_dict.TryGetValue(key, out result))
							_dict[key] = result = value;
					}
					finally
					{
						_lock.ExitWriteLock();
					}
				}
			}
			finally
			{
				_lock.ExitUpgradeableReadLock();
			}

			return result;
		}

		/// <summary>
		/// Return existing value if key already exists.
		/// Otherwise, add new key/value and return the new value created by valueFactory.
		/// </summary>
		public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
		{
			var result = default(TValue);

			_lock.EnterUpgradeableReadLock();
			try
			{

				if (!_dict.TryGetValue(key, out result))
				{
					_lock.EnterWriteLock();
					try
					{
						// double-check locking pattern
						if (!_dict.TryGetValue(key, out result))
							_dict[key] = result = valueFactory.Invoke(key);
					}
					finally
					{
						_lock.ExitWriteLock();
					}
				}
			}
			finally
			{
				_lock.ExitUpgradeableReadLock();
			}

			return result;
		}

		public void Clear()
		{
			_lock.EnterWriteLock();
			try
			{
				_dict.Clear();
			}
			finally
			{
				_lock.ExitWriteLock();
			}
		}
	}
}
