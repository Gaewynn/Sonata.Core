#region Namespace Sonata.Core.Extensions
//	TODO: comment
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sonata.Core.Extensions
{
	/// <summary>
	/// Adds functionalities to the <see cref="Exception"/> class.
	/// </summary>
	public static class ExceptionExtension
	{
		/// <summary>
		/// Gets the whole messages of the current <see cref="Exception"/> and all inner exceptions.
		/// </summary>
		/// <param name="instance">The <see cref="Exception"/> containing messages to get.</param>
		/// <returns>The whole messages of the current <see cref="Exception"/> and all inner exceptions.</returns>
		public static string GetFullMessage(this Exception instance)
		{
			var fullMessage = new StringBuilder();
			while (instance != null)
			{
				fullMessage.AppendFormat("{0}{1}", instance.Message, Environment.NewLine);
				instance = instance.InnerException;
			}

			return fullMessage.ToString();
		}

		/// <summary>
		/// Gets the whole messages of the current <see cref="Exception"/> and all inner exceptions splitted in two parts:
		///		- the first part being the exception message;
		///		- the second part being the full message of the inner exception.
		/// </summary>
		/// <param name="instance">The <see cref="Exception"/> containing messages to get.</param>
		/// <returns>The whole messages of the current <see cref="Exception"/> and all inner exceptions splitted in two parts.</returns>
		/// <remarks>If the current exception is an <see cref="AggregateException"/>, a message will be created for each inner exception.</remarks>
		public static IList<string[]> GetFullMessages(this Exception instance)
		{
			IList<string[]> messages = new List<string[]>();
			if (instance != null)
			{
				if (instance.GetType() == typeof(AggregateException))
				{
					foreach (var exception in ((AggregateException)instance).InnerExceptions)
					{
						messages.Add(new[]
						{
							exception.Message, 
							exception.InnerException != null ? exception.InnerException.GetFullMessage() : String.Empty
						});
					}
				}
				else
				{
					messages.Add(new[]
					{
						instance.Message, 
						instance.InnerException != null ? instance.InnerException.GetFullMessage() : String.Empty
					});
				}
			}

			for (var i = 0; i < messages.Count; i++)
				messages[i] = messages[i].Where(e => !String.IsNullOrWhiteSpace(e)).ToArray();

			return messages.Where(e => e.Length > 0).ToList();
		}
	}
}
