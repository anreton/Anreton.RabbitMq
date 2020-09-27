using System.Security.Cryptography;

using Anreton.RabbitMq.Extensions.PasswordHashGenerator.Generators.Abstractions;

namespace Anreton.RabbitMq.Extensions.PasswordHashGenerator.Generators
{
	/// <summary>
	/// Represents an implementation of the Md5 generator.
	/// </summary>
	public sealed class Md5Generator : Generator
	{
		protected override byte[] GetHash(byte[] input)
		{
			using var md5 = MD5.Create();

			return md5.ComputeHash(input);
		}
	}
}
