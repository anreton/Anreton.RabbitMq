using System.Security.Cryptography;

using Anreton.RabbitMq.Extensions.PasswordHashGenerator.Generators.Abstractions;

namespace Anreton.RabbitMq.Extensions.PasswordHashGenerator.Generators
{
	/// <summary>
	/// Represents an implementation of the Md5 generator.
	/// </summary>
	public sealed class Md5Generator : Generator
	{
		private readonly MD5 md5;

		private bool disposedValue;

		public Md5Generator()
		{
			this.md5 = MD5.Create();
		}

		protected override byte[] GenerateHash(byte[] input) => this.md5.ComputeHash(input);

		protected override void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					this.md5.Dispose();
				}

				this.disposedValue = true;
			}

			base.Dispose(disposing);
		}
	}
}
