#region Namespace
//	TODO: comment
#endregion

using System;
using System.Data;

namespace Sonata.Core.Extensions
{
	public static class DbCommandExtension
	{
		public static void AddParameter(this IDbCommand instance, string name, object value, DbType type = DbType.Int32)
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));

			var param = instance.CreateParameter();
			param.ParameterName = name ?? throw new ArgumentNullException(nameof(name));
			param.Value = value ?? DBNull.Value;
			param.DbType = type;

			instance.AddParameter(param);
		}

		public static void AddParameter(this IDbCommand instance, IDbDataParameter parameter)
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));
			if (parameter == null)
				throw new ArgumentNullException(nameof(parameter));

			instance.Parameters.Add(parameter);
		}
	}
}