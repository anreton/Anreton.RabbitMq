using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Anreton.RabbitMq.HashGenerator.Abstractions
{
	/// <summary>
	/// Represents a base implementation of the hash generator.
	/// </summary>
	public class Generator : IDisposable
	{
		/// <summary>
		/// The hash algorithm.
		/// </summary>
		private readonly HashAlgorithm hashAlgorithm;

		/// <summary>
		/// The random number generator.
		/// </summary>
		private readonly RNGCryptoServiceProvider rngCryptoServiceProvider;

		/// <summary>
		/// To detect redundant <see cref="IDisposable.Dispose"/> calls.
		/// </summary>
		private bool disposed;

		/// <summary>
		/// Initializes a new instance of the <see cref="Generator"/> class.
		/// </summary>
		/// <param name="hashAlgorithm">
		/// The hash algorithm.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="hashAlgorithm"/> is <see langword="null"/>.
		/// </exception>
		protected Generator(HashAlgorithm hashAlgorithm)
		{
			this.hashAlgorithm = hashAlgorithm ?? throw new ArgumentNullException(nameof(hashAlgorithm));
			this.rngCryptoServiceProvider = new RNGCryptoServiceProvider();
		}

		/// <inheritdoc cref="IDisposable.Dispose"/>
		public void Dispose()
		{
			this.Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Generates the hash for the specified string.
		/// </summary>
		/// <param name="input">
		/// The input string to generate the hash code for.
		/// </param>
		/// <returns>
		/// The generated hash.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// <paramref name="input"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="input"/> is empty <see cref="string"/>.
		/// </exception>
		public string Generate(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				throw new ArgumentException($"{nameof(input)} must not be empty.", nameof(input));
			}

			const int saltLength = 4;
			var salt = new byte[saltLength];
			this.rngCryptoServiceProvider.GetBytes(salt);
			var inputAsUTF8Bytes = Encoding
				.UTF8
				.GetBytes(input);
			var saltWithInputAsUTF8Bytes = salt
				.Concat(inputAsUTF8Bytes)
				.ToArray();
			var hashOfSaltWithInputAsUTF8Bytes = this.hashAlgorithm.ComputeHash(saltWithInputAsUTF8Bytes);
			var saltWithHashOfSaltWithInputAsUTF8Bytes = salt
				.Concat(hashOfSaltWithInputAsUTF8Bytes)
				.ToArray();

			return Convert.ToBase64String(saltWithHashOfSaltWithInputAsUTF8Bytes);
		}

		/// <summary>
		/// Performs the actual dispose.
		/// </summary>
		/// <remarks>
		/// Implement <see href="https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose">the dispose pattern</see>.
		/// </remarks>
		/// <param name="disposing">
		/// Indicates whether the method call comes from a <see cref="IDisposable.Dispose"/> method (its value is <see langword="true"/>) or from a finalizer (its value is <see langword="false"/>).
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}

			if (disposing)
			{
				this.hashAlgorithm.Dispose();
				this.rngCryptoServiceProvider.Dispose();
			}

			this.disposed = true;
		}
	}
}
