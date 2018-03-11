#region Namespace Sonata.Core.Extensions
//	TODO: comment
#endregion

namespace Sonata.Core.Extensions
{
	public static class ByteExtension
	{
		public static int TripleShift(this byte instance, int shift)
		{
			return instance >> shift;
		}
	}
}
