using System.Security.Cryptography;

using Anreton.RabbitMq.PasswordHashGenerator.Abstractions;

namespace Anreton.RabbitMq.PasswordHashGenerator
{
	/// <summary>
	/// Represents an implementation of the SHA512 hash generator.
	/// </summary>
	public sealed class SHA512Generator : Generator
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SHA512Generator"/> class.
		/// </summary>
		public SHA512Generator() : base(SHA512.Create())
		{
		}
	}
}
