using System;
using System.Security.Cryptography;

using Anreton.RabbitMq.PasswordHashGenerator.Abstractions;

namespace Anreton.RabbitMq.PasswordHashGenerator
{
	/// <summary>
	/// Represents an implementation of the Md5 hash generator.
	/// </summary>
	public sealed class Md5Generator : HashGenerator
	{
		/// <summary>
		/// Computes the MD5 hash for the input data.
		/// </summary>
		private readonly MD5 md5;

		/// <summary>
		/// To detect redundant <see cref="IDisposable.Dispose"/> calls.
		/// </summary>
		private bool disposed;

		/// <summary>
		/// Initializes a new instance of the <see cref="Md5Generator"/> class.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5351:Do Not Use Broken Cryptographic Algorithms", Justification = "For backwards compatibility.")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Vulnerability", "S2070:SHA-1 and Message-Digest hash algorithms should not be used in secure contexts", Justification = "For backwards compatibility.")]
		public Md5Generator() => this.md5 = MD5.Create();

		/// <inheritdoc cref="HashGenerator.ComputeHash(byte[])"/>
		protected override byte[] ComputeHash(byte[] input) => this.md5.ComputeHash(input);

		/// <inheritdoc cref="HashGenerator.Dispose(bool)"/>
		protected override void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}

			if (disposing)
			{
				this.md5.Dispose();
			}

			this.disposed = true;
			base.Dispose(disposing);
		}
	}
}
