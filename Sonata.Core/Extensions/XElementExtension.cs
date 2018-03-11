#region Namespace Sonata.Core.Extensions
//	TODO: comment
#endregion

using System;
using System.Xml;
using System.Xml.Linq;

namespace Sonata.Core.Extensions
{
	public static class XElementExtension
	{
		public static XmlElement ToXmlElement(this XElement instance)
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));

			var xmlDocument = new XmlDocument();
			xmlDocument.Load(instance.CreateReader());

			return xmlDocument.DocumentElement;
		}
	}
}