using System;
using System.Runtime.Serialization;

namespace Sonata.Core.Attributes
{
	/// <inheritdoc />
	/// <summary>
	/// Represents an <see cref="T:System.Attribute" /> allowing to give a <see cref="T:System.String" /> value to a field of an <see cref="T:System.Enum" />.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	[DataContract]
    public sealed class StringValueAttribute : Attribute
	{
		#region Properties

		/// <summary>
		/// Gets or sets the <see cref="String"/> value of the current <see cref="Enum"/> member.
		/// </summary>
		public string Value { get; set; }

		#endregion

		#region Constructors

		/// <inheritdoc />
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Sonata.Core.Attributes.StringValueAttribute" /> class.
		/// </summary>
		/// <param name="value">The <see cref="T:System.String" /> value or the current <see cref="T:System.Enum" /> member.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="value" /> is NULL.</exception>
		public StringValueAttribute(string value)
		{
			Value = value ?? throw new ArgumentNullException(nameof(value));
		}

		#endregion
	}
}
