#region Namespace Sonata.Core.Extensions
//	TODO: comment
#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Sonata.Core.Attributes;
using Sonata.Core.Collections.Generic;

namespace Sonata.Core.Extensions
{
	public static class StringExtensions
	{
		#region Constants

		public static byte[] Iv = { 72, 67, 159, 106, 164, 55, 150, 22, 179, 135, 12, 246, 123, 128, 16, 201 };

		#endregion

		#region Methods

		public static string ToBase64(this string instance)
		{
			if (instance == null)
				instance = String.Empty;

			var plainTextBytes = Encoding.UTF8.GetBytes(instance);
			return Convert.ToBase64String(plainTextBytes);
		}

		public static string FromBase64(this string instance)
		{
			if (instance == null)
				instance = String.Empty;

			return Encoding.UTF8.GetString(Convert.FromBase64String(instance));
		}

		public static string Base64UrlDecode(this string instance)
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));

			instance = instance.Replace('-', '+');		// 62nd char of encoding
			instance = instance.Replace('_', '/');		// 63rd char of encoding

			switch (instance.Length % 4)				// Pad with trailing '='s
			{
				case 0: break;							// No pad chars in this case
				case 2: instance += "=="; break;		// Two pad chars
				case 3: instance += "="; break;			// One pad char
				default:
					throw new InvalidOperationException("Illegal base64url string!");
			}

			return instance;
		}

		public static string Base64UrlEncode(this string instance)
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));

			instance = instance.Split('=')[0];			// Remove any trailing '='s
			instance = instance.Replace('+', '-');		// 62nd char of encoding
			instance = instance.Replace('/', '_');		// 63rd char of encoding

			return instance;
		}

		public static string Decrypt(this string instance, string encryptionKey)
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));
			if (encryptionKey == null)
				throw new ArgumentNullException(nameof(encryptionKey));

			var cipheredData = Convert.FromBase64String(Base64UrlDecode(instance));
			var key = Encoding.UTF8.GetBytes(encryptionKey);

			//	The key size must be 128, 192, or 256 bits for the current algorithm.
			if (key.Length != 16 && key.Length != 24 && key.Length != 32)
			{
				byte[] newKey;
				if (key.Length > 32)
					newKey = key.Take(32).ToArray();
				else if (key.Length > 24)
					newKey = new byte[32];
				else if (key.Length > 16)
					newKey = new byte[24];
				else
					newKey = new byte[16];

				Buffer.BlockCopy(key, 0, newKey, 0, key.Length);
				key = newKey;
			}

			ICryptoTransform decryptor;
			using (var rijndael = new RijndaelManaged())
			{
				rijndael.Mode = CipherMode.CBC;
				decryptor = rijndael.CreateDecryptor(key, Iv);
			}

			byte[] plainTextData;
			int decryptedByteCount;
			using (var memoryStream = new MemoryStream(cipheredData))
			{
				using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
				{
					plainTextData = new byte[cipheredData.Length];
					decryptedByteCount = cryptoStream.Read(plainTextData, 0, plainTextData.Length);
					cryptoStream.Close();
				}

				memoryStream.Close();
			}

			return Encoding.UTF8.GetString(plainTextData, 0, decryptedByteCount);
		}

		public static string Encrypt(this string instance, string encryptionKey)
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));
			if (encryptionKey == null)
				throw new ArgumentNullException(nameof(encryptionKey));

			var plainText = Encoding.UTF8.GetBytes(instance);
			var key = Encoding.UTF8.GetBytes(encryptionKey);

			//	The key size must be 128, 192, or 256 bits for the current algorithm.
			if (key.Length != 16 && key.Length != 24 && key.Length != 32)
			{
				byte[] newKey;
				if (key.Length > 32)
					newKey = key.Take(32).ToArray();
				else if (key.Length > 24)
					newKey = new byte[32];
				else if (key.Length > 16)
					newKey = new byte[24];
				else
					newKey = new byte[16];

				Buffer.BlockCopy(key, 0, newKey, 0, key.Length);
				key = newKey;
			}

			ICryptoTransform aesEncryptor;
			using (var rijndael = new RijndaelManaged())
			{
				rijndael.Mode = CipherMode.CBC;
				aesEncryptor = rijndael.CreateEncryptor(key, Iv);
			}

			byte[] cipherBytes;
			using (var memoryStream = new MemoryStream())
			{
				using (var cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write))
				{
					cryptoStream.Write(plainText, 0, plainText.Length);
					cryptoStream.FlushFinalBlock();
					cipherBytes = memoryStream.ToArray();
					cryptoStream.Close();
				}

				memoryStream.Close();
			}

			return Base64UrlEncode(Convert.ToBase64String(cipherBytes));
		}

		public static bool EqualsAi(this string instance, string value, bool trim = false, char[] trimChar = null, bool removeDoubleSpaces = false)
		{
			return !trim && !removeDoubleSpaces 
				? StringAiComparer.Instance.Equals(instance, value) 
				: new StringAiComparer(trim, trimChar, removeDoubleSpaces).Equals(instance, value);
		}

		public static bool EqualsCi(this string instance, string value, bool trim = false, char[] trimChar = null, bool removeDoubleSpaces = false)
		{
			return !trim && !removeDoubleSpaces 
				? StringCiComparer.Instance.Equals(instance, value) 
				: new StringCiComparer(trim, trimChar, removeDoubleSpaces).Equals(instance, value);
		}

		public static bool EqualsCiAi(this string instance, string value, bool trim = false, char[] trimChar = null, bool removeDoubleSpaces = false)
		{
			return !trim && !removeDoubleSpaces 
				? StringCiAiComparer.Instance.Equals(instance, value) 
				: new StringCiAiComparer(trim, trimChar, removeDoubleSpaces).Equals(instance, value);
		}

		/// <summary>
		/// Gets the matching string value of a member of the <see cref="TEnum"/> enumeration.
		/// Gets a null value if no member exists with this string value.
		/// </summary>
		/// <typeparam name="TEnum">The type of the enumeration in which search for a string value.</typeparam>
		/// <param name="instance">The current string value of a enumeration member.</param>
		/// <returns>Gets the matching string value of a member of the <see cref="TEnum"/> enumeration or a null value if no member exists with this string value.</returns>
		public static TEnum GetEnumStringValue<TEnum>(this string instance) where TEnum : struct
		{
			return (TEnum)typeof(TEnum)
					.GetFields()
					.First(f => f.GetCustomAttributes(typeof(StringValueAttribute), false)
									.Cast<StringValueAttribute>()
									.Any(a => a.Value.Equals(instance, StringComparison.OrdinalIgnoreCase)))
					.GetValue(null);
		}

		public static string ToValidFileName(this string instance, Dictionary<string, string> replacers = null)
		{
			if (instance == null)
				return null;

			instance = instance.Trim();

			if (replacers == null)
				replacers = new Dictionary<string, string>();

			replacers.AddIfNotExist("<", "_LT_");
			replacers.AddIfNotExist(">", "_GT_");
			replacers.AddIfNotExist(":", "_CL_");
			replacers.AddIfNotExist("\"", "_QT_");
			replacers.AddIfNotExist("/", "_FS_");
			replacers.AddIfNotExist("\\", "_BS_");
			replacers.AddIfNotExist("|", "_PI_");
			replacers.AddIfNotExist("?", "_QM_");
			replacers.AddIfNotExist("*", "_AS_");

			instance = replacers.Aggregate(instance, (current, replacer) => current.Replace(replacer.Key, replacer.Value));

			var fileInfo = new FileInfo(instance);
			if (fileInfo.FullName.Length <= 255)
				return instance;

			instance = Path.GetFileNameWithoutExtension(fileInfo.FullName);
			instance = instance.Substring(0, 255 - fileInfo.Extension.Length);
			instance = instance.Substring(0, instance.Length - 2);
			instance = String.Format("{0}~1{1}", instance, fileInfo.Extension);

			return instance;
		}

		public static string RemoveDiacritics(this string instance)
		{
			if (instance == null)
				return null;

			var charMap = new Dictionary<string, string>
			{
				{ "ç", "c" },

				{ "à", "a" },
				{ "ä", "a" },
				{ "á", "a" },
				{ "â", "a" },
				{ "ã", "a" },
				{ "å", "a" },
				{ "À", "A" },
				{ "Ä", "A" },
				{ "Â", "A" },

				{ "è", "e" },
				{ "ë", "e" },
				{ "é", "e" },
				{ "ê", "e" },
				{ "È", "E" },
				{ "Ë", "E" },
				{ "Ê", "E" },

				{ "ì", "i" },
				{ "ï", "i" },
				{ "î", "i" },
				{ "Ì", "I" },
				{ "Ï", "I" },
				{ "Î", "I" },

				{ "ò", "o" },
				{ "ö", "o" },
				{ "ô", "o" },
				{ "õ", "o" },
				{ "ø", "o" },
				{ "Ò", "O" },
				{ "Ö", "O" },
				{ "Ô", "O" },

				{ "ù", "u" },
				{ "ü", "u" },
				{ "ú", "u" },
				{ "û", "u" },
				{ "Ù", "U" },
				{ "Ü", "U" },
				{ "Û", "U" },

				{ "ñ", "n" },
				{ "ÿ", "y" }
			};

			return charMap.Aggregate(instance, (current, replacement) => current.Replace(replacement.Key, replacement.Value));
		}

		public static string ToLowerCamelCase(this string instance)
		{
			if (instance == null)
				return null;

			instance = instance.Trim();
			if (instance.Length == 0)
				return String.Empty;

			if (instance.Length == 1)
				return instance[0].ToString(CultureInfo.InvariantCulture).ToLower();

			var indexOfSpace = instance.IndexOf(" ", StringComparison.Ordinal);
			if (indexOfSpace == -1)
				return instance[0].ToString(CultureInfo.InvariantCulture).ToLower() + instance.Substring(1);

			while (indexOfSpace > 0)
			{
				instance = instance.Substring(0, indexOfSpace) + instance[indexOfSpace + 1].ToString(CultureInfo.InvariantCulture).ToUpper() + instance.Substring(indexOfSpace + 2);
				indexOfSpace = instance.IndexOf(" ", indexOfSpace, StringComparison.Ordinal);
			}

			return instance;
		}

#endregion
	}
}
