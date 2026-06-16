using System;
using System.Collections.Generic;

public class MultiKeyDictionary<T1, T2, T3> : Dictionary<T1, Dictionary<T2, T3>>
{
	public new Dictionary<T2, T3> this[T1 key]
	{
		get
		{
			if (!base.ContainsKey(key))
			{
				base.Add(key, new Dictionary<T2, T3>());
			}
			Dictionary<T2, T3> result;
			base.TryGetValue(key, out result);
			return result;
		}
	}

	public bool ContainsKey(T1 key1, T2 key2)
	{
		Dictionary<T2, T3> dictionary;
		base.TryGetValue(key1, out dictionary);
		return dictionary != null && dictionary.ContainsKey(key2);
	}
}
