#region Namespace Sonata.Core.Collections.Generic
//	TODO
#endregion

using System;
using System.Collections.Generic;

namespace Sonata.Core.Collections.Generic
{
	/// <inheritdoc />
	/// <summary>
	/// Represents a generic <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> allowing to define a specific comparison with a specific <see cref="T:System.Linq.Expressions.Expression" />
	/// </summary>
	/// <typeparam name="T">The <see cref="T:System.Type" /> of <see cref="T:System.Object" /> to compare.</typeparam>
	public class EqualityComparer<T> : IEqualityComparer<T> where T : class
	{
		#region Members

		private readonly Func<T, object> _comparisonExpression;

		#endregion

		#region Constructors

		/// <summary>
		/// Initialize a new instance of <see cref="EqualityComparer{T}"/>
		/// </summary>
		/// <param name="comparisonExpression">The <see cref="System.Linq.Expressions.Expression"/> used to compare <see cref="object"/>.</param>
		public EqualityComparer(Func<T, object> comparisonExpression)
		{
			_comparisonExpression = comparisonExpression;
		}

		#endregion

		#region Methods

		#region IEqualityComparer<T> Members

		/// <inheritdoc />
		/// <summary>
		/// Determines whether the specified objects are equal.
		/// </summary>
		/// <param name="x">The first object of type T to compare.</param>
		/// <param name="y">The second object of type T to compare.</param>
		/// <returns>true if the specified objects are equal; otherwise, false.</returns>
		public bool Equals(T x, T y)
		{
			var first = _comparisonExpression.Invoke(x);
			var second = _comparisonExpression.Invoke(y);

			return first != null && first.Equals(second);
		}

		/// <inheritdoc />
		/// <summary>
		/// Returns a hash code for the specified object.
		/// </summary>
		/// <param name="obj">The Object for which a hash code is to be returned.</param>
		/// <returns>A hash code for the specified object.</returns>
		public int GetHashCode(T obj)
		{
			return _comparisonExpression.Invoke(obj).GetHashCode();
		}

		#endregion

		#endregion
	}
}
