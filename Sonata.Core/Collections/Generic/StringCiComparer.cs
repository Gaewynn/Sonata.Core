#region Namespace Sonata.Core.Collections.Generic
//	TODO
#endregion

using System;
using System.Collections.Generic;

namespace Sonata.Core.Collections.Generic
{
	public class StringCiComparer : IEqualityComparer<string>
	{
		#region Constants

		private const string Space = " ";
		private const string DoubleSpaces = "  ";

		#endregion

		#region Members

		private static StringCiComparer _instance;
		private readonly bool _removeDoubleSpaces;
		private readonly bool _trim;
		private readonly char[] _trimChar;

		#endregion

		#region Properties

		public static StringCiComparer Instance => _instance ?? (_instance = new StringCiComparer(false, false));

		#endregion

		#region Constructors

		public StringCiComparer(bool trim = false, bool removeDoubleSpaces = false)
		{
			_removeDoubleSpaces = removeDoubleSpaces;
			_trim = trim;
		}

		public StringCiComparer(bool trim = false, char[] trimChar = null, bool removeDoubleSpaces = false)
		{
			_removeDoubleSpaces = removeDoubleSpaces;
			_trim = trim;
			_trimChar = trimChar;
		}

		#endregion

		#region Methods

		#region IEqualityComparer<string> Members

		public bool Equals(string x, string y)
		{
			if (_trim)
			{
				x = _trimChar == null ? x.Trim() : x.Trim(_trimChar);
				y = _trimChar == null ? y.Trim() : y.Trim(_trimChar);
			}

			if (!_removeDoubleSpaces)
				return String.Compare(x, y, StringComparison.CurrentCultureIgnoreCase) == 0;

			while (x.Contains(DoubleSpaces))
				x = x.Replace(DoubleSpaces, Space);

			while (y.Contains(DoubleSpaces))
				y = y.Replace(DoubleSpaces, Space);

			return String.Compare(x, y, StringComparison.CurrentCultureIgnoreCase) == 0;
		}

		public int GetHashCode(string obj)
		{
			return obj.Trim().ToLower().GetHashCode();
		}

		#endregion

		#endregion
	}
}
