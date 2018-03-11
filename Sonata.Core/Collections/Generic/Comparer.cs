#region Namespace Sonata.Core.Collections.Generic
//	TODO
#endregion

using System;
using System.Collections.Generic;

namespace Sonata.Core.Collections.Generic
{
	/// <inheritdoc />
	/// <summary>
	/// Defines a method that a type implements to compare two objects by a specific property.
	/// </summary>
	/// <typeparam name="T">The type of objects to compare.</typeparam>
	public class Comparer<T> : IComparer<T>
	{
		#region Members

		private readonly Comparison<T> _comparisonExpression;

		#endregion

		/// <summary>
		/// Initialize a new instance of <see cref="Comparer{T}"/>
		/// </summary>
		/// <param name="comparisonExpression">The <see cref="System.Linq.Expressions.Expression"/> used to compare <see cref="object"/>.</param>
		public Comparer(Comparison<T> comparisonExpression)
		{
			_comparisonExpression = comparisonExpression ?? throw new ArgumentNullException(nameof(comparisonExpression));
		}

		/// <summary>
		/// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>A signed integer that indicates the relative values of x and y, as shown in the following table.</returns>
		public int Compare(T x, T y)
		{
			return _comparisonExpression(x, y);
		}
	}

	/// <inheritdoc />
	/// <summary>
	/// Defines a method that a type implements to compare two objects by a specific property.
	/// </summary>
	/// <typeparam name="T">The type of objects to compare.</typeparam>
	/// <typeparam name="TProperty">The type of the property on the objects to compare.</typeparam>
	public class ComparerWithProperty<T, TProperty> : IComparer<T>
		where TProperty : IComparable
	{
		#region Members

		private readonly Func<T, TProperty> _comparisonExpression;

		#endregion

		#region Constructors

		/// <summary>
		/// Initialize a new instance of <see cref="ComparerWithProperty{T, TProperty}"/>
		/// </summary>
		/// <param name="comparisonExpression">The <see cref="System.Linq.Expressions.Expression"/> used to compare <see cref="object"/>.</param>
		public ComparerWithProperty(Func<T, TProperty> comparisonExpression)
		{
			_comparisonExpression = comparisonExpression;
		}

		#endregion

		#region Methods

		#region IComparer<T> Members

		/// <summary>
		/// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>A signed integer that indicates the relative values of x and y, as shown in the following table.</returns>
		public int Compare(T x, T y)
		{
			var px = _comparisonExpression(x);
			var py = _comparisonExpression(y);

			return px.CompareTo(py);
		} 

		#endregion

		#endregion
	}
}
