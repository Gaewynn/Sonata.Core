#region Namespace Sonata.Core.Extensions
//	TODO: comment
#endregion

using System;
using Sonata.Core.Attributes;

namespace Sonata.Core.Extensions
{
	/// <summary>
	/// Adds functionalities to the <see cref="Enum"/> class.
	/// </summary>
	public static class EnumExtension
	{
		/// <summary>
		/// Gets the string value associated to the current enum member using a <see cref="StringValueAttribute"/>.
		/// </summary>
		/// <param name="instance">The enum member for which get the string value.</param>
		/// <returns>The string value associated to the current enum member using a <see cref="StringValueAttribute"/>. String.Empty if the member has no <see cref="StringValueAttribute"/> defined.</returns>
		///	<exception cref="T:System.ArgumentNullException"><param ref="instance"></param> is NULL.</exception>
		public static string GetStringValue(this Enum instance)
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));

			var value = String.Empty;
			var type = instance.GetType();
			var fieldInfo = type.GetField(instance.ToString());

			value = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) is StringValueAttribute[] attrs 
					&& attrs.Length > 0 
				? attrs[0].Value 
				: value;

			return value;
		}
	}
}
