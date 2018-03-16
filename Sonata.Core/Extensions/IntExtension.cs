#region Namespace
//	TODO
#endregion

namespace Sonata.Core.Extensions
{
	public static class IntExtension
	{
		public static int TripleShift(this int instance, int shift)
		{
			return instance >= 0 ? instance >> shift : (instance >> shift) + (2 << ~shift);
		}
	}
}
