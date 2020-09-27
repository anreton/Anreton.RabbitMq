using System;

using Anreton.RabbitMq.Extensions.PasswordHashGenerator.Generators;
using Anreton.RabbitMq.Extensions.PasswordHashGenerator.Generators.Abstractions;

using CommandLine;
using CommandLine.Text;

namespace Anreton.RabbitMq.Extensions.PasswordHashGenerator
{
	/// <summary>
	/// Contains an entry point of the application.
	/// </summary>
	public static class Program
	{
		/// <summary>
		/// The entry point of the application.
		/// </summary>
		/// <param name="args">
		/// Command-line arguments.
		/// </param>
		public static void Main(string[] args)
		{
			var parser = new Parser(parserSettings => parserSettings.HelpWriter = null);
			var parserResult = parser.ParseArguments<Options>(args);
			_ = parserResult.WithParsed
				(
					options =>
					{
						try
						{
							Generator generator = options.HashingAlgorithm switch
							{
								"sha256" => new Sha256Generator(),
								"sha512" => new Sha512Generator(),
								"md5" => new Md5Generator(),
								_ => throw new ArgumentException(message: "Unknown hashing algorithm.")
							};
							var hash = generator.GenerateHash(options.Password);

							Console.WriteLine(hash);
						}
#pragma warning disable CA1031 // Do not catch general exception types
						catch (Exception exception)
						{
							Console.WriteLine(exception.Message);
						}
#pragma warning restore CA1031 // Do not catch general exception types
					}
				)
				.WithNotParsed
				(
					_ =>
					{
						var errorMessage = HelpText
							.AutoBuild
							(
								parserResult,
								helpText =>
								{
									helpText.AutoHelp = false;
									helpText.AutoVersion = false;

									return HelpText.DefaultParsingErrorsHandler(parserResult, helpText);
								},
								example => example
							)
							.ToString();

						Console.WriteLine(errorMessage);
					}
				);
		}
	}
}
