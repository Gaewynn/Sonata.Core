#region Namespace
//	TODO: comment
#endregion

#if NET45
namespace QimLib.Net45.Core.Extensions
#else
namespace QimLib.Core.Extensions
#endif
{
	public static class IntExtension
	{
		public static int TripleShift(this int instance, int shift)
		{
			return instance >= 0 ? instance >> shift : (instance >> shift) + (2 << ~shift);
		}
	}
}
