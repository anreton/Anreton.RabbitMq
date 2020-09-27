using System.Security.Cryptography;

using Anreton.RabbitMq.Extensions.PasswordHashGenerator.Generators.Abstractions;

namespace Anreton.RabbitMq.Extensions.PasswordHashGenerator.Generators
{
	/// <summary>
	/// Represents an implementation of the Sha512 generator.
	/// </summary>
	public class Sha512Generator : Generator
	{
		protected override byte[] GetHash(byte[] input)
		{
			using var sha512 = SHA512.Create();

			return sha512.ComputeHash(input);
		}
	}
}
