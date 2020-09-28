using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Anreton.RabbitMq.Extensions.PasswordHashGenerator.Generators.Abstractions
{
	/// <summary>
	/// Represents a base implementation of the generator.
	/// </summary>
	public abstract class Generator : IDisposable
	{
		private readonly RNGCryptoServiceProvider rngCryptoServiceProvider;

		private bool disposedValue;

		protected Generator()
		{
			this.rngCryptoServiceProvider = new RNGCryptoServiceProvider();
		}

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

			var salt = this.GenerateSalt();
			var passwordBytes = this.GetUTF8Bytes(password);
			var saltWithPasswordBytes = this.ConcatArrays(salt, passwordBytes);
			var hash = this.GenerateHash(saltWithPasswordBytes);
			var saltWithHash = this.ConcatArrays(salt, hash);

			return this.ConvertToBase64(saltWithHash);
		}

		/// <summary>
		/// Generates a salt.
		/// </summary>
		/// <returns>
		/// The salt.
		/// </returns>
		protected virtual byte[] GenerateSalt()
		{
			const int saltLength = 4;
			var salt = new byte[saltLength];
			this.rngCryptoServiceProvider.GetBytes(salt);

			return salt;
		}

		/// <summary>
		/// Gets an UTF-8 bytes representation of the string.
		/// </summary>
		/// <param name="string">
		/// The string.
		/// </param>
		/// <returns>
		/// The UTF-8 bytes representation of the string.
		/// </returns>
		protected virtual byte[] GetUTF8Bytes(string @string) => Encoding.UTF8.GetBytes(@string);

		/// <summary>
		/// Concates two byte arrays.
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
		protected virtual byte[] ConcatArrays(byte[] first, byte[] second) => first
			.Concat(second)
			.ToArray();

		/// <summary>
		/// Converts an byte array to the Base64 string.
		/// </summary>
		/// <param name="input">
		/// The byte array.
		/// </param>
		/// <returns>
		/// The Base64 string.
		/// </returns>
		protected virtual string ConvertToBase64(byte[] input) => Convert.ToBase64String(input);

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
		protected abstract byte[] GenerateHash(byte[] input);

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					this.rngCryptoServiceProvider.Dispose();
				}

				this.disposedValue = true;
			}
		}

		public void Dispose()
		{
			this.Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
