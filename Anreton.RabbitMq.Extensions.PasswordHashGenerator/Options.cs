using CommandLine;

namespace Anreton.RabbitMq.Extensions.PasswordHashGenerator
{
	/// <summary>
	/// Represents strongly typed command-line arguments.
	/// </summary>
	public sealed class Options
	{
		/// <summary>
		/// The password to get a hash.
		/// </summary>
		[Option(shortName: 'p', longName: "password", Required = true, HelpText = "The password to get a hash.")]
		public string Password { get; set; }

		/// <summary>
		/// The hashing algorithm used.
		/// </summary>
		/// <remarks>
		/// Valid values: 'sha256', 'sha512', 'md5'.
		/// </remarks>
		[Option(shortName: 'h', longName: "hashingAlgorithm", Default = "sha256", HelpText = "The hashing algorithm used. Valid values: 'sha256', 'sha512', 'md5'.")]
		public string HashingAlgorithm { get; set; }
	}
}
