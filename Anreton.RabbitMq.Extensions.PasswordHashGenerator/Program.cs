using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.ComponentModel;

using Anreton.RabbitMq.Extensions.PasswordHashGenerator.Generators;
using Anreton.RabbitMq.Extensions.PasswordHashGenerator.Generators.Abstractions;

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
			_ = GetRootCommand().Invoke(args);

			static RootCommand GetRootCommand()
			{
				var rootCommand = new RootCommand()
				{
					GetPasswordsOption(),
					GetAlgorithmOption()
				};
				rootCommand.Handler = CommandHandler.Create<IEnumerable<string>, Algorithm>(Handle);

				return rootCommand;
			}

			static Option<IEnumerable<string>> GetPasswordsOption()
			{
				var passwordsOption = new Option<IEnumerable<string>>
				(
					alias: "--passwords",
					description: "Passwords to get a hashes."
				)
				{
					IsRequired = true
				};
				passwordsOption.AddAlias("-p");

				return passwordsOption;
			}

			static Option<Algorithm> GetAlgorithmOption()
			{
				var algorithmOption = new Option<Algorithm>
				(
					alias: "--algorithm",
					getDefaultValue: () => Algorithm.Sha256,
					description: "The hashing algorithm used."
				);
				algorithmOption.AddAlias("-a");

				return algorithmOption;
			}

			static void Handle(IEnumerable<string> passwords, Algorithm algorithm)
			{
				using Generator generator = algorithm switch
				{
					Algorithm.Sha256 => new Sha256Generator(),
					Algorithm.Sha512 => new Sha512Generator(),
					Algorithm.Md5 => new Md5Generator(),
					_ => throw new InvalidEnumArgumentException(message: "Unknown hashing algorithm."),
				};

				foreach (var password in passwords)
				{
					var hash = generator.GenerateHash(password);
					Console.WriteLine($"{{Password: \"{password}\", Hash: \"{hash}\"}}");
				}
			}
		}
	}
}
