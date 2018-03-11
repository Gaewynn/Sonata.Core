#region Namespace Sonata.Core.Extensions
//	TODO: comment
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using QimLib.Core.Extensions;

namespace Sonata.Core.Extensions
{
	public static class EnumerableExtension
	{
		public static string Dump<T>(this IEnumerable<T> instance)
		{
			return instance == null ?
				"IENUMERABLE_NULL" :
				instance.Aggregate(String.Empty, (current1, current) => current1 + (current.Dump() + Environment.NewLine));
		}

		public static List<List<T>> GetPermutations<T>(this IEnumerable<T> instance)
		{
			var items = instance as T[] ?? instance.ToArray();
			var currentPermutation = new T[items.Length];
			var inSelection = new bool[items.Length];
			var permutations = new List<List<T>>();

			Permute(items, inSelection, currentPermutation, permutations, 0);

			return permutations;
		}

		/// <summary>
		/// Recursively permute the items that are not yet in the current selection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <param name="inSelection"></param>
		/// <param name="currentPermutation"></param>
		/// <param name="permutations"></param>
		/// <param name="nextPosition"></param>
		private static void Permute<T>(IEnumerable<T> items, IList<bool> inSelection, IList<T> currentPermutation, ICollection<List<T>> permutations, int nextPosition)
		{
			// See if all of the positions are filled.
			var enumerable = items as T[] ?? items.ToArray();
			if (nextPosition == enumerable.Length)
			{
				// All of the positioned are filled.
				// Save this permutation.
				permutations.Add(currentPermutation.ToList());
			}
			else
			{
				// Try options for the next position.
				for (var i = 0; i < enumerable.Length; i++)
				{
					if (inSelection[i])
						continue;

					// Add this item to the current permutation.
					inSelection[i] = true;
					currentPermutation[nextPosition] = enumerable.ElementAt(i);

					// Recursively fill the remaining positions.
					Permute(enumerable, inSelection, currentPermutation, permutations, nextPosition + 1);

					// Remove the item from the current permutation.
					inSelection[i] = false;
				}
			}
		}
	}
}
