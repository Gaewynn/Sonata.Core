#region Namespace Sonata.Core.Extensions
//	TODO: comment
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Sonata.Core.Attributes;

namespace Sonata.Core.Extensions
{
	/// <summary>
	///  Adds functionalities to the <see cref="Type"/> class.
	/// </summary>
	public static class TypeExtension
	{
		/// <summary>
		/// Gets the list of <see cref="StringValueAttribute"/> Value property on the members of the specified type.
		/// </summary>
		/// <param name="instance">The type in which look for <see cref="StringValueAttribute"/>.</param>
		/// <returns>The list of <see cref="StringValueAttribute"/> Value property on the members of the specified type.</returns>
		public static IEnumerable<string> GetAllStringValues(this Type instance)
		{
			return (from fieldInfo in instance.GetFields()
					select fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[] into attrs
					where attrs != null
					&& attrs.Length > 0
					select attrs[0].Value).ToList();
		}

		/// <summary>
		/// Gets the first underlying <see cref="Type"/> used by the specified IEnumerable{T} <see cref="Type"/>.
		/// </summary>
		/// <param name="instance">An IEnumerable{T} for which get the underlying <see cref="Type"/>.</param>
		/// <returns>The first underlying <see cref="Type"/> used by the specified IEnumerable{T} <see cref="Type"/>.</returns>
		public static Type GetEnumerableUnderlyingType(this Type instance)
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));

			Type underlyingType = null;
			foreach (var currentIinterface in instance.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
				underlyingType = currentIinterface.GetGenericArguments()[0];

			return underlyingType;
		}
	}
}
