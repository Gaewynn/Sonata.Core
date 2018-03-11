#region Namespace Sonata.Core.Extensions
//	TODO: comment
#endregion

using System.Collections.Generic;

namespace Sonata.Core.Extensions
{
	public static class DictionaryExtension
	{
		public static void AddIfNotExist<TKey, TValue>(this Dictionary<TKey, TValue> instance, TKey key, TValue value)
		{
			if (instance == null)
				return;

			if (!instance.ContainsKey(key))
				instance.Add(key, value);
		}
	}
}
