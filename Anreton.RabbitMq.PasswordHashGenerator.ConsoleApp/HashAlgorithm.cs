namespace Anreton.RabbitMq.Extensions.PasswordHashGenerator
{
	/// <summary>
	/// Represents a hash algorithm.
	/// </summary>
	public enum HashAlgorithm
	{
		/// <summary>
		/// SHA256 algorithm.
		/// </summary>
		SHA256 = 0,

		/// <summary>
		/// SHA512 algorithm.
		/// </summary>
		SHA512 = 1,

		/// <summary>
		/// MD5 algorithm.
		/// </summary>
		MD5 = 2
	}
}
