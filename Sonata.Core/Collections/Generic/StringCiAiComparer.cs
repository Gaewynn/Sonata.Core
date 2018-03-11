#region Namespace Sonata.Core.Collections.Generic
//	TODO
#endregion

using System;
using System.Collections.Generic;
using QimLib.Core.Extensions;
using Sonata.Core.Extensions;

namespace Sonata.Core.Collections.Generic
{
	public class StringCiAiComparer : IEqualityComparer<string>
	{
		#region Constants

		private const string Space = " ";
		private const string DoubleSpaces = "  ";

		#endregion

		#region Members

		private static StringCiAiComparer _instance;
		private readonly bool _removeDoubleSpaces;
		private readonly bool _trim;
		private readonly char[] _trimChar;

		#endregion

		#region Properties

		public static StringCiAiComparer Instance => _instance ?? (_instance = new StringCiAiComparer(false, false));

		#endregion

		#region Constructors

		public StringCiAiComparer(bool trim = false, bool removeDoubleSpaces = false)
		{
			_removeDoubleSpaces = removeDoubleSpaces;
			_trim = trim;
		}

		public StringCiAiComparer(bool trim = false, char[] trimChar = null, bool removeDoubleSpaces = false)
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
			x = x.RemoveDiacritics();
			y = y.RemoveDiacritics();

			if (_trim)
			{
				x = _trimChar == null ? x.Trim() : x.Trim(_trimChar);
				y = _trimChar == null ? y.Trim() : y.Trim(_trimChar);
			}

			if (_removeDoubleSpaces)
			{
				while (x.Contains(DoubleSpaces))
					x = x.Replace(DoubleSpaces, Space);

				while (y.Contains(DoubleSpaces))
					y = y.Replace(DoubleSpaces, Space);
			}

			if (x == null && y == null)
				return true;
			if (x == null)
				return false;
			if (y == null)
				return false;

			return String.Compare(x, y, StringComparison.CurrentCultureIgnoreCase) == 0;
		}

		public int GetHashCode(string obj)
		{
			return obj.Trim().ToLower().RemoveDiacritics().GetHashCode();
		}

		#endregion

		#endregion
	}
}
