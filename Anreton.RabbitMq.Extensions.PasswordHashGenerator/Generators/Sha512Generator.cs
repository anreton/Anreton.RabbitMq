using System.Security.Cryptography;

using Anreton.RabbitMq.Extensions.PasswordHashGenerator.Generators.Abstractions;

namespace Anreton.RabbitMq.Extensions.PasswordHashGenerator.Generators
{
	/// <summary>
	/// Represents an implementation of the Sha512 generator.
	/// </summary>
	public class Sha512Generator : Generator
	{
		private readonly SHA512 sha512;

		private bool disposedValue;

		public Sha512Generator()
		{
			this.sha512 = SHA512.Create();
		}

		protected override byte[] GenerateHash(byte[] input) => this.sha512.ComputeHash(input);

		protected override void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					this.sha512.Dispose();
				}

				this.disposedValue = true;
			}

			base.Dispose(disposing);
		}
	}
}
