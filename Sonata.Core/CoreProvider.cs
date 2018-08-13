using System;
using System.Reflection;

namespace Sonata.Core
{
	public class CoreProvider
    {
		public static void Trace(Assembly source, string message)
		{
			Trace(source == null ? String.Empty : source.FullName, message);
		}

		public static void Trace(string source, string message)
		{

		}
	}
}
