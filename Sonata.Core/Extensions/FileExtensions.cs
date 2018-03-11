#region Namespace Sonata.Core.Extensions
//	TODO: comment
#endregion

using System;
using System.IO;

namespace Sonata.Core.Extensions
{
	public static class FileExtensions
	{
		public static string Write(this FileInfo instance, Stream fileContent, int nbMaxOfRetries = 10)
		{
			if (fileContent == null)
				throw new ArgumentNullException(nameof(fileContent));
			if (instance.DirectoryName == null)
				throw new InvalidOperationException("Directory Name can not be null");

			var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(instance.Name);
			var fileNameExtension = Path.GetExtension(instance.Name);
			var fileNamePattern = String.Format("{0}-({{0}}){1}", fileNameWithoutExtension, fileNameExtension);

			//	Check if a file exists with the same name
			var retryIndex = 1;
			var newFilename = instance.Name;

			while (File.Exists(Path.Combine(instance.DirectoryName, newFilename)) && retryIndex <= nbMaxOfRetries)
			{
				newFilename = String.Format(fileNamePattern, retryIndex);
				retryIndex++;
			}

			if (File.Exists(Path.Combine(instance.DirectoryName, newFilename)))
				throw new InvalidOperationException("Can not create file " + instance.FullName + " after " + nbMaxOfRetries + " retries : a file with the same name already exists.");

			using (var openedFileStream = File.OpenWrite(Path.Combine(instance.DirectoryName, newFilename)))
				fileContent.CopyTo(openedFileStream);

			return Path.Combine(instance.DirectoryName, newFilename);
		}
	}
}
