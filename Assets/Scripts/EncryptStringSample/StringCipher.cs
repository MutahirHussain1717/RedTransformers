using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EncryptStringSample
{
	public static class StringCipher
	{
		public static string Encrypt(string plainText, string passPhrase)
		{
			byte[] array = StringCipher.Generate256BitsOfRandomEntropy();
			byte[] array2 = StringCipher.Generate256BitsOfRandomEntropy();
			byte[] bytes = Encoding.UTF8.GetBytes(plainText);
			Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passPhrase, array, 1000);
			byte[] bytes2 = rfc2898DeriveBytes.GetBytes(32);
			ICryptoTransform transform = new RijndaelManaged
			{
				BlockSize = 256,
				Mode = CipherMode.CBC,
				Padding = PaddingMode.PKCS7
			}.CreateEncryptor(bytes2, array2);
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
			cryptoStream.Write(bytes, 0, bytes.Length);
			cryptoStream.FlushFinalBlock();
			byte[] array3 = array;
			array3 = array3.Concat(array2).ToArray<byte>();
			array3 = array3.Concat(memoryStream.ToArray()).ToArray<byte>();
			memoryStream.Close();
			cryptoStream.Close();
			return Convert.ToBase64String(array3);
		}

		public static string Decrypt(string cipherText, string passPhrase)
		{
			try
			{
				byte[] array = Convert.FromBase64String(cipherText);
				byte[] salt = array.Take(32).ToArray<byte>();
				byte[] rgbIV = array.Skip(32).Take(32).ToArray<byte>();
				byte[] array2 = array.Skip(64).Take(array.Length - 64).ToArray<byte>();
				Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passPhrase, salt, 1000);
				byte[] bytes = rfc2898DeriveBytes.GetBytes(32);
				ICryptoTransform transform = new RijndaelManaged
				{
					BlockSize = 256,
					Mode = CipherMode.CBC,
					Padding = PaddingMode.PKCS7
				}.CreateDecryptor(bytes, rgbIV);
				MemoryStream memoryStream = new MemoryStream(array2);
				CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
				byte[] array3 = new byte[array2.Length];
				int count = cryptoStream.Read(array3, 0, array3.Length);
				memoryStream.Close();
				cryptoStream.Close();
				cipherText = Encoding.UTF8.GetString(array3, 0, count);
			}
			catch (SystemException ex)
			{
				cipherText = string.Empty;
			}
			return cipherText;
		}

		private static byte[] Generate256BitsOfRandomEntropy()
		{
			byte[] array = new byte[32];
			RNGCryptoServiceProvider rngcryptoServiceProvider = new RNGCryptoServiceProvider();
			rngcryptoServiceProvider.GetBytes(array);
			return array;
		}

		private const int Keysize = 256;

		private const int DerivationIterations = 1000;
	}
}
