namespace System.Collections.Generic
{
	public static class ListExtensions
	{
		#region AddRange
		public static void AddRange<T>(this IList<T> list, IEnumerable<T> collection, int limit = -1)
		{
			if (collection == null)
				return;

			foreach (var value in collection)
			{
				list.Add(value);

				if (limit == -1)
					continue;

				if (list.Count == limit)
					break;
			}
		}
		#endregion

		public static List<TOutput> ConvertAll<T, TOutput>(this IEnumerable<T> list, Func<T, TOutput> converter)
		{
			var outputs = new List<TOutput>();

			foreach (var value in list)
			{
				outputs.Add(converter.Invoke(value));
			}

			return outputs;
		}

		public static List<TOutput> ConvertAll<T, TOutput>(this IEnumerable<T> list, Func<T, int, TOutput> converter)
		{
			var outputs = new List<TOutput>();
			var index = 0;

			foreach (var value in list)
			{
				outputs.Add(converter.Invoke(value, index));
				index++;
			}

			return outputs;
		}


		#region ForEach
		public static void ForEach<T>(this IList<T> list, Action<T, int> action)
		{
			var index = 0;

			foreach (var value in list)
			{
				if (action != null)
					action.Invoke(value, index);

				index++;
			}
		}
		#endregion

		#region Find
		public static T Find<T>(this IList<T> list, Enum type)
		{
			return list.Find(type.GetHashCode());
		}

		public static T Find<T>(this IList<T> list, int index)
		{
			if (index < 0 || list.Count <= index)
				return default;

			return list[index];
		}
		#endregion


		public static void Shuffle<T>(this IList<T> list)
		{
			// var random = new Random();
			// int index = list.Count;
			//
			// while (1 < index)
			// {
			// 	var dest = random.Next(index + 1);
			// 	if (dest == index)
			// 		continue;
			// 	
			// 	list.Swap(index, dest);
			// 	index--;
			// }


			var random = new Random();
			int index = list.Count;
			int dest;

			while (1 < index)
			{
				--index;

				dest = random.Next(index + 1);
				list.Swap(index, dest);
			}
		}

		public static void Swap<T>(this IList<T> list, int src, int dest)
		{
			if (src == dest)
				return;

			T value = list[src];
			list[src] = list[dest];
			list[dest] = value;
		}

		public static T Random<T>(this IList<T> list)
		{
			var random = new Random();
			var index = random.Next(list.Count);

			return list.Find(index);
		}
	}
}