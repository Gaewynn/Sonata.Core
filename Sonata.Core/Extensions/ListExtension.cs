#region Namespace Sonata.Core.Extensions
//	TODO: comment
#endregion

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Sonata.Core.Extensions
{
	public static class ListExtension
	{

		/// <summary>
		/// Randomizes elements inside the specified <paramref name="list"/> using the Fisher-Yates algorithm.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the specified <paramref name="list"/>.</typeparam>
		/// <param name="list">The list to randomize.</param>
		public static void Shuffle<T>(this IList<T> list)
		{
			var provider = new RNGCryptoServiceProvider();

			var n = list.Count;
			while (n > 1)
			{
				var box = new byte[1];
				do
					provider.GetBytes(box);
				while (!(box[0] < n * (Byte.MaxValue / n)));

				var k = (box[0] % n);
				n--;

				var value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
	}
}
