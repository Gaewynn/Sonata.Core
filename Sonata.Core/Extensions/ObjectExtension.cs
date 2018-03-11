#region Namespace Sonata.Core.Extensions
//	TODO: comment
#endregion

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Sonata.Core.Extensions
{
	public static class ObjectExtension
	{
		public static string Dump(this object instance)
		{
			return instance == null ?
				"NULL" :
				JsonConvert.SerializeObject(instance, Formatting.Indented, new JsonSerializerSettings
				{
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
					ContractResolver = new CamelCasePropertyNamesContractResolver()
				});
		}

		public static string Dump(this object instance, string name)
		{
			return String.Format("[{0}]::{1}", name, instance.Dump());
		}

		/// <summary>
		/// Get the value of the specified property in the specified <see cref="Object"/> instance.
		/// </summary>
		/// <param name="instance">The instance of the <see cref="Object"/> containing the property for which get the value.</param>
		/// <param name="propertyType">The property type of the property for which get the value.</param>
		/// <param name="propertyName">The name of the property of the property for which get the value.</param>
		/// <returns>The value of the specified property in the specified <see cref="Object"/> instance; NULL if the property has not been found.</returns>
		public static object GetPropertyValue(this object instance, Type propertyType, string propertyName)
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));
			if (propertyType == null)
				throw new ArgumentNullException(nameof(propertyType));
			if (propertyName == null)
				throw new ArgumentNullException(nameof(propertyName));
			if (String.IsNullOrWhiteSpace(propertyName))
				throw new ArgumentException("propertyName can not be empty or whitespace.");

			var property = propertyType.GetProperty(propertyName);
			return property == null 
				? null 
				: property.GetValue(instance, null)?.ToString();
		}
	}
}
