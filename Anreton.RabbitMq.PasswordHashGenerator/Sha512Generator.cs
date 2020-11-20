using System;
using System.Security.Cryptography;

using Anreton.RabbitMq.PasswordHashGenerator.Abstractions;

namespace Anreton.RabbitMq.PasswordHashGenerator
{
	/// <summary>
	/// Represents an implementation of the Sha512 hash generator.
	/// </summary>
	public sealed class Sha512Generator : HashGenerator
	{
		/// <summary>
		/// Computes the SHA512 hash for the input data.
		/// </summary>
		private readonly SHA512 sha512;

		/// <summary>
		/// To detect redundant <see cref="IDisposable.Dispose"/> calls.
		/// </summary>
		private bool disposed;

		/// <summary>
		/// Initializes a new instance of the <see cref="Sha512Generator"/> class.
		/// </summary>
		public Sha512Generator() => this.sha512 = SHA512.Create();

		/// <inheritdoc cref="HashGenerator.ComputeHash(byte[])"/>
		protected override byte[] ComputeHash(byte[] input) => this.sha512.ComputeHash(input);

		/// <inheritdoc cref="HashGenerator.Dispose(bool)"/>
		protected override void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}

			if (disposing)
			{
				this.sha512.Dispose();
			}

			this.disposed = true;
			base.Dispose(disposing);
		}
	}
}
