using System;

namespace EncryptStringSample
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Please enter a password to use:");
			string passPhrase = Console.ReadLine();
			Console.WriteLine("Please enter a string to encrypt:");
			string plainText = Console.ReadLine();
			Console.WriteLine(string.Empty);
			Console.WriteLine("Your encrypted string is:");
			string text = StringCipher.Encrypt(plainText, passPhrase);
			Console.WriteLine(text);
			Console.WriteLine(string.Empty);
			Console.WriteLine("Your decrypted string is:");
			string value = StringCipher.Decrypt(text, passPhrase);
			Console.WriteLine(value);
			Console.WriteLine(string.Empty);
			Console.WriteLine("Press any key to exit...");
			Console.ReadLine();
		}
	}
}
