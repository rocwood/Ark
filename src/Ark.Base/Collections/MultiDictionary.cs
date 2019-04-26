using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ark
{
	[Serializable]
	public class MultiDictionary<TKey, TValue> : Dictionary<TKey, List<TValue>>
	{
		public MultiDictionary() {}

		public MultiDictionary(int capacity) : base(capacity) {}

		protected MultiDictionary(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}
}
