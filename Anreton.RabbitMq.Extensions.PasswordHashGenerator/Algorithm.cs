namespace Anreton.RabbitMq.Extensions.PasswordHashGenerator
{
	/// <summary>
	/// The hashing algorithm used.
	/// </summary>
	public enum Algorithm
	{
		/// <summary>
		/// Sha256.
		/// </summary>
		Sha256 = 0,

		/// <summary>
		/// Sha512.
		/// </summary>
		Sha512 = 1,

		/// <summary>
		/// Md5.
		/// </summary>
		Md5 = 2
	}
}
