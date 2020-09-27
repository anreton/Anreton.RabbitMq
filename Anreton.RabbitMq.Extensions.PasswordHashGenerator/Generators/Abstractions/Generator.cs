using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Anreton.RabbitMq.Extensions.PasswordHashGenerator.Generators.Abstractions
{
	/// <summary>
	/// Represents a base implementation of the generator.
	/// </summary>
	public abstract class Generator
	{
		/// <summary>
		/// Generates a hash by the password.
		/// </summary>
		/// <remarks>
		/// Implements `Template Method` pattern.
		/// </remarks>
		/// <param name="password">
		/// The password to get a hash.
		/// </param>
		/// <returns>
		/// The hash by the password.
		/// </returns>
		public string GenerateHash(string password)
		{
			if (String.IsNullOrEmpty(password))
			{
				throw new ArgumentException($"'{nameof(password)}' must not be empty.", nameof(password));
			}

			var salt = GetSalt();
			var passwordBytes = GetUTF8Bytes(password);
			var saltWithPasswordBytes = MergeArrays(salt, passwordBytes);
			var hash = this.GetHash(saltWithPasswordBytes);
			var saltWithHash = MergeArrays(salt, hash);

			return ConvertToBase64(saltWithHash);
		}

		/// <summary>
		/// Generates a salt.
		/// </summary>
		/// <returns>
		/// The salt.
		/// </returns>
		private static byte[] GetSalt()
		{
			const int saltLength = 4;
			var salt = new byte[saltLength];
			using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
			rngCryptoServiceProvider.GetBytes(salt);

			return salt;
		}

		/// <summary>
		/// Generates UTF-8 bytes representation of the string.
		/// </summary>
		/// <param name="string">
		/// The string.
		/// </param>
		/// <returns>
		/// The UTF-8 bytes representation of the string.
		/// </returns>
		private static byte[] GetUTF8Bytes(string @string) => Encoding.UTF8.GetBytes(@string);

		/// <summary>
		/// Merges two byte arrays.
		/// </summary>
		/// <param name="first">
		/// The first byte array.
		/// </param>
		/// <param name="second">
		/// The second byte array.
		/// </param>
		/// <returns>
		/// The merged byte array.
		/// </returns>
		private static byte[] MergeArrays(byte[] first, byte[] second) => first
			.Concat(second)
			.ToArray();

		/// <summary>
		/// Generates Base64 string by byte array.
		/// </summary>
		/// <param name="input">
		/// The byte array.
		/// </param>
		/// <returns>
		/// The Base64 string.
		/// </returns>
		private static string ConvertToBase64(byte[] input) => Convert.ToBase64String(input);

		/// <summary>
		/// Generates a hash of the byte array.
		/// </summary>
		/// <remarks>
		/// Overrides in derived classes.
		/// </remarks>
		/// <param name="input">
		/// The byte array.
		/// </param>
		/// <returns>
		/// The hash of the byte array.
		/// </returns>
		protected abstract byte[] GetHash(byte[] input);
	}
}
