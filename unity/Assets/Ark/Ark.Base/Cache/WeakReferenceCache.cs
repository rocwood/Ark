using System;
using System.Collections.Generic;
using System.Linq;

namespace Ark
{
	/// <summary>
	/// Auto-dispose caching according to WeakReference
	/// </summary>
	public class WeakReferenceCache<TKey, TValue> where TValue:class
	{
		/// <summary>
		/// Get value associated with key, add reference, reset lifetime
		/// </summary>
		public virtual TValue Get(TKey key, object refer)
		{
			CacheItem item = null;
			_cache.TryGetValue(key, out item);

			if (item == null)
				return null;

			if (refer != null)
				item.AddRefer(refer);

			item.lifeTime = _lifeTime;

			return item.value;
		}

		/// <summary>
		/// Update key/value, and add reference, reset lifetime
		/// </summary>
		public virtual void Set(TKey key, TValue value, object refer)
		{
			CacheItem item = null;
			_cache.TryGetValue(key, out item);

			if (item == null)
			{
				item = new CacheItem();
				item.value = value;

				_cache[key] = item;
			}
			else
			{
				if (item.value != value)
				{
					item.RemoveAllRefers();
					item.Dispose(_disposer);

					item.value = value;
				}
			}

			if (refer != null)
				item.AddRefer(refer);

			item.lifeTime = _lifeTime;
		}

		/// <summary>
		/// Remove reference manually
		/// </summary>
		public virtual void Derefer(object refer)
		{
			foreach (var kv in _cache)
			{
				var item = kv.Value;
				if (item != null)
					item.RemoveRefer(refer);
			}
		}

		/// <summary>
		/// Remove key/value and all refernces manually
		/// </summary>
		/// <param name="key"></param>
		public virtual void Remove(TKey key)
		{
			CacheItem item;
			if (!_cache.TryGetValue(key, out item))
				return;

			if (item != null)
				item.Dispose(_disposer);

			_cache.Remove(key);
		}

		public virtual void Clear()
		{
			_cache.Clear();
		}

		protected float _checkElapsed = 0;
		protected float _checkInterval = 10;
		protected float _lifeTime = 60;
		protected Action<TValue> _disposer = DefaultDisposer;

		public void SetCheckInterval(float seconds)
		{
			_checkInterval = seconds;
		}

		public void SetLifeTime(float seconds)
		{
			_lifeTime = seconds;

			foreach (var kv in _cache)
			{
				var item = kv.Value;
				if (item != null &&
					item.lifeTime > _lifeTime)
					item.lifeTime = _lifeTime;
			}
		}

		public void SetDisposer(Action<TValue> disposer)
		{
			_disposer = disposer;
		}

		protected static void DefaultDisposer(TValue value)
		{
			if (value is IDisposable v)
				v.Dispose();
		}

		private readonly List<TKey> _toRemoveList = new List<TKey>();

		public virtual void Update(float deltaSeconds)
		{
			_checkElapsed += deltaSeconds;
			if (_checkElapsed < _checkInterval)
				return;

			foreach (var kv in _cache)
			{
				var item = kv.Value;
				if (item == null)
				{
					_toRemoveList.Add(kv.Key);
					continue;
				}

				if (item.HasRefer())
				{
					item.lifeTime = _lifeTime;
					continue;
				}

				item.lifeTime -= _checkElapsed;
				if (item.lifeTime <= 0)
				{
					item.Dispose(_disposer);
					_toRemoveList.Add(kv.Key);
				}
			}

			foreach (var key in _toRemoveList)
				_cache.Remove(key);

			_toRemoveList.Clear();

			_checkElapsed %= _checkInterval;
		}

		protected class CacheItem
		{
			public TValue value;
			public float lifeTime;
			public readonly List<WeakReference> refers = new List<WeakReference>();

			public void Dispose(Action<TValue> disposer)
			{
				if (value == null)
					return;

				if (disposer != null)
					disposer.Invoke(value);

				value = null;
			}

			public bool HasRefer()
			{
				return refers.Exists(x => x.Target != null);
			}

			public void AddRefer(object refer)
			{
				if (refer == null)
					return;

				if (refers.FindIndex(x => x.Target == refer) < 0)
					refers.Add(new WeakReference(refer, false));
			}

			public void RemoveRefer(object refer)
			{
				if (refer == null)
					return;

				refers.RemoveAll(x => x.Target == refer);
			}

			public void RemoveAllRefers()
			{
				refers.Clear();
			}
		}

		protected readonly Dictionary<TKey, CacheItem> _cache = new Dictionary<TKey, CacheItem>();
	}
}
