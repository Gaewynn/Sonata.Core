#region Namespace Sonata.Core.Extensions
//	TODO: comment
#endregion

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Sonata.Core.Extensions
{
    public static class ObjectExtension
    {
        #region Members
        
        private static readonly MethodInfo CloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);

        #endregion

        #region Methods

        public static object DeepCopy(this object instance)
        {
            return InternalCopy(instance, new Dictionary<object, object>(new ReferenceEqualityComparer()));
        }

        public static T DeepCopy<T>(this T original)
        {
            return (T)DeepCopy((object)original);
        }

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
            return $"[{name}]::{instance.Dump()}";
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

        public static bool IsPrimitive(this Type type)
        {
            if (type == typeof(string)) 
                return true;

            return (type.IsValueType & type.IsPrimitive);
        }

        private static object InternalCopy(object originalObject, IDictionary<object, object> visited)
        {
            if (originalObject == null) 
                return null;

            var typeToReflect = originalObject.GetType();
            if (IsPrimitive(typeToReflect)) 
                return originalObject;

            if (visited.ContainsKey(originalObject)) 
                return visited[originalObject];

            if (typeof(Delegate).IsAssignableFrom(typeToReflect)) 
                return null;

            var cloneObject = CloneMethod.Invoke(originalObject, null);
            if (typeToReflect.IsArray)
            {
                var arrayType = typeToReflect.GetElementType();
                if (IsPrimitive(arrayType) == false)
                {
                    var clonedArray = (Array)cloneObject;
                    clonedArray.ForEach((array, indices) => array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices));
                }

            }

            visited.Add(originalObject, cloneObject);
            CopyFields(originalObject, visited, cloneObject, typeToReflect);
            RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);
            
            return cloneObject;
        }

        private static void RecursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
        {
            if (typeToReflect.BaseType == null) 
                return;

            RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
            CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
        }

        private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, IReflect typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
        {
            foreach (var fieldInfo in typeToReflect.GetFields(bindingFlags))
            {
                if (filter != null && filter(fieldInfo) == false) 
                    continue;

                if (IsPrimitive(fieldInfo.FieldType)) 
                    continue;

                var originalFieldValue = fieldInfo.GetValue(originalObject);
                var clonedFieldValue = InternalCopy(originalFieldValue, visited);

                fieldInfo.SetValue(cloneObject, clonedFieldValue);
            }
        }

        #endregion
    }
}
