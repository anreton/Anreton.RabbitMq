using System;
using System.Security.Cryptography;

using Anreton.RabbitMq.PasswordHashGenerator.Abstractions;

namespace Anreton.RabbitMq.PasswordHashGenerator
{
	/// <summary>
	/// Represents an implementation of the Sha256 hash generator.
	/// </summary>
	public sealed class Sha256Generator : HashGenerator
	{
		/// <summary>
		/// Computes the SHA256 hash for the input data.
		/// </summary>
		private readonly SHA256 sha256;

		/// <summary>
		/// To detect redundant <see cref="IDisposable.Dispose"/> calls.
		/// </summary>
		private bool disposed;

		/// <summary>
		/// Initializes a new instance of the <see cref="Sha256Generator"/> class.
		/// </summary>
		public Sha256Generator() => this.sha256 = SHA256.Create();

		/// <inheritdoc cref="HashGenerator.ComputeHash(byte[])"/>
		protected override byte[] ComputeHash(byte[] input) => this.sha256.ComputeHash(input);

		/// <inheritdoc cref="HashGenerator.Dispose(bool)"/>
		protected override void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}

			if (disposing)
			{
				this.sha256.Dispose();
			}

			this.disposed = true;
			base.Dispose(disposing);
		}
	}
}
