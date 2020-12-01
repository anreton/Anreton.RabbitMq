using System.Security.Cryptography;

using Anreton.RabbitMq.PasswordHashGenerator.Abstractions;

namespace Anreton.RabbitMq.PasswordHashGenerator
{
	/// <summary>
	/// Represents an implementation of the MD5 hash generator.
	/// </summary>
	public sealed class MD5Generator : Generator
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MD5Generator"/> class.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5351:Do Not Use Broken Cryptographic Algorithms", Justification = "For backwards compatibility.")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Vulnerability", "S2070:SHA-1 and Message-Digest hash algorithms should not be used in secure contexts", Justification = "For backwards compatibility.")]
		public MD5Generator() : base(MD5.Create())
		{
		}
	}
}
