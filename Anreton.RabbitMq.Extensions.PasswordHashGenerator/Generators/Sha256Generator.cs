using System.Security.Cryptography;

using Anreton.RabbitMq.Extensions.PasswordHashGenerator.Generators.Abstractions;

namespace Anreton.RabbitMq.Extensions.PasswordHashGenerator.Generators
{
	/// <summary>
	/// Represents an implementation of the Sha256 generator.
	/// </summary>
	public sealed class Sha256Generator : Generator
	{
		protected override byte[] GetHash(byte[] input)
		{
			using var sha256 = SHA256.Create();

			return sha256.ComputeHash(input);
		}
	}
}
