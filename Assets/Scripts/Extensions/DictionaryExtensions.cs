
namespace System.Collections.Generic
{
	public static class DictionaryExtensions
	{
		#region Any
		public static bool Any<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Predicate<TValue> match)
		{
			if (match != null)
			{
				foreach (var pair in dictionary)
				{
					var isValid = match.Invoke(pair.Value);
					if (isValid)
						return true;
				}
			}
			
			return false;
		}
		#endregion

		#region ForEach
		public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Action<TValue> action)
		{
			if (action != null)
			{
				foreach (var pair in dictionary)
				{
					if (pair.Value == null)
						continue;

					if (action != null)
						action.Invoke(pair.Value);
				}
			}
		}

		public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Action<TKey, TValue> action)
		{
			if (action != null)
			{
				foreach (var pair in dictionary)
				{
					if (pair.Value == null)
						continue;

					if (action != null)
						action.Invoke(pair.Key, pair.Value);
				}
			}
		}
		#endregion

		#region Find
		public static TValue Find<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			if (dictionary.TryGetValue(key, out TValue value))
				return value;

			return default;
		}
		
		public static TValue Find<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<TValue, bool> match)
		{
			if (match != null)
			{
				foreach (var pair in dictionary)
				{
					if (pair.Value == null)
						continue;

					var isValid = match.Invoke(pair.Value);
					if (isValid)
						return pair.Value;
				}
			}

			return default;
		}

		public static TValue Find<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, bool> match)
		{
			if (match != null)
			{
				foreach (var pair in dictionary)
				{
					if (pair.Value == null)
						continue;

					var isValid = match.Invoke(pair);
					if (isValid)
						return pair.Value;
				}
			}

			return default;
		}
		#endregion

		#region FindKey
		public static TKey FindKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<TValue, bool> match)
		{
			if (match != null)
			{
				foreach (var pair in dictionary)
				{
					if (pair.Value == null)
						continue;

					var isValid = match.Invoke(pair.Value);
					if (isValid)
						return pair.Key;
				}
			}

			return default;
		}

		public static TKey FindKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, bool> match)
		{
			if (match != null)
			{
				foreach (var pair in dictionary)
				{
					if (pair.Value == null)
						continue;

					var isValid = match.Invoke(pair);
					if (isValid)
						return pair.Key;
				}
			}

			return default;
		}
		#endregion
	}
}