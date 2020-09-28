using System.Security.Cryptography;

using Anreton.RabbitMq.Extensions.PasswordHashGenerator.Generators.Abstractions;

namespace Anreton.RabbitMq.Extensions.PasswordHashGenerator.Generators
{
	/// <summary>
	/// Represents an implementation of the Sha256 generator.
	/// </summary>
	public sealed class Sha256Generator : Generator
	{
		private readonly SHA256 sha256;

		private bool disposedValue;

		public Sha256Generator()
		{
			this.sha256 = SHA256.Create();
		}

		protected override byte[] GenerateHash(byte[] input) => this.sha256.ComputeHash(input);

		protected override void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					this.sha256.Dispose();
				}

				this.disposedValue = true;
			}

			base.Dispose(disposing);
		}
	}
}
