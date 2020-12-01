using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Anreton.RabbitMq.PasswordHashGenerator.Abstractions
{
	/// <summary>
	/// Represents a base implementation of the password hash generator.
	/// </summary>
	public class Generator : IDisposable
	{
		/// <summary>
		/// Hash algorithm.
		/// </summary>
		private readonly HashAlgorithm hashAlgorithm;

		/// <summary>
		/// Random number generator.
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
		/// Hash algorithm.
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
		/// Generates a hash by the password.
		/// </summary>
		/// <param name="password">
		/// The password to get a hash.
		/// </param>
		/// <returns>
		/// The hash by the password.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// <paramref name="password"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="password"/> is empty <see cref="string"/>.
		/// </exception>
		public string Generate(string password)
		{
			if (string.IsNullOrEmpty(password))
			{
				throw new ArgumentException($"{nameof(password)} must not be empty.", nameof(password));
			}

			var salt = this.GenerateSalt();
			var passwordAsUtf8 = Encoding
				.UTF8
				.GetBytes(password);
			var saltWithPasswordAsUtf8 = salt
				.Concat(passwordAsUtf8)
				.ToArray();
			var hashOfSaltWithPasswordAsUtf8 = this.hashAlgorithm.ComputeHash(saltWithPasswordAsUtf8);
			var saltWithHashOfSaltWithPasswordAsUtf8 = salt
				.Concat(hashOfSaltWithPasswordAsUtf8)
				.ToArray();

			return Convert.ToBase64String(saltWithHashOfSaltWithPasswordAsUtf8);
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

		/// <summary>
		/// Generates a salt.
		/// </summary>
		/// <returns>
		/// The salt.
		/// </returns>
		private byte[] GenerateSalt()
		{
			const int saltLength = 4;
			var salt = new byte[saltLength];
			this.rngCryptoServiceProvider.GetBytes(salt);

			return salt;
		}
	}
}
