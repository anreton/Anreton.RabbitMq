namespace Anreton.RabbitMq.Extensions.PasswordHashGenerator
{
	/// <summary>
	/// The hashing algorithm used.
	/// </summary>
	public enum HashingAlgorithm
	{
		/// <summary>
		/// SHA256.
		/// </summary>
		SHA256 = 0,

		/// <summary>
		/// SHA512.
		/// </summary>
		SHA512 = 1,

		/// <summary>
		/// MD5.
		/// </summary>
		MD5 = 2
	}
}
