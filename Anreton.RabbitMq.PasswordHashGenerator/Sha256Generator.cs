using System.Security.Cryptography;

using Anreton.RabbitMq.PasswordHashGenerator.Abstractions;

namespace Anreton.RabbitMq.PasswordHashGenerator
{
	/// <summary>
	/// Represents an implementation of the SHA256 hash generator.
	/// </summary>
	public sealed class SHA256Generator : Generator
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SHA256Generator"/> class.
		/// </summary>
		public SHA256Generator() : base(SHA256.Create())
		{
		}
	}
}
