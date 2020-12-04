using System.Security.Cryptography;

using Anreton.RabbitMq.HashGenerator.Abstractions;

namespace Anreton.RabbitMq.HashGenerator
{
	/// <summary>
	/// Represents an implementation of the SHA512 generator.
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
