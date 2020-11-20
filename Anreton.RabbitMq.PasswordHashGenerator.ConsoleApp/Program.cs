using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;

using Anreton.RabbitMq.PasswordHashGenerator;
using Anreton.RabbitMq.PasswordHashGenerator.Abstractions;

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
			var passwordsOption = new Option<IEnumerable<string>>
			(
				alias: "--passwords",
				description: "Passwords to get a hashes."
			);
			passwordsOption.AddAlias("-p");

			var algorithmOption = new Option<HashingAlgorithm>
			(
				alias: "--algorithm",
				getDefaultValue: () => HashingAlgorithm.Sha256,
				description: "The hashing algorithm used."
			);
			algorithmOption.AddAlias("-a");

			var importOption = new Option<string>
			(
				alias: "--import",
				description: "Path to the file with passwords."
			);
			importOption.AddAlias("-i");

			var exportOption = new Option<string>
			(
				alias: "--export",
				description: "Path to the file where to export results."
			);
			exportOption.AddAlias("-e");

			var rootCommand = new RootCommand()
			{
				passwordsOption,
				algorithmOption,
				importOption,
				exportOption
			};
			rootCommand.Handler = CommandHandler.Create<IEnumerable<string>, HashingAlgorithm, string, string>(Handler);

			_ = rootCommand.Invoke(args);
		}

		/// <summary>
		/// Handles input arguments and prints the output.
		/// </summary>
		/// <param name="passwords">
		/// Passwords to get a hashes.
		/// </param>
		/// <param name="algorithm">
		/// The hashing algorithm used.
		/// </param>
		/// <param name="import">
		/// Path to the file with passwords.
		/// </param>
		/// <param name="export">
		/// Path to the file where to export results.
		/// </param>
		/// <exception cref="NotSupportedException">
		/// Unknown hashing algorithm used.
		/// </exception>
		private static void Handler
		(
			IEnumerable<string> passwords,
			HashingAlgorithm algorithm,
			string import,
			string export
		)
		{
			var passwordsForProcessing = new List<string>();

			if (!(passwords is null) && passwords.Any())
			{
				passwordsForProcessing.AddRange(passwords);
				Console.WriteLine($"Received {passwordsForProcessing.Count} passwords from arguments.");
			}

			if (!string.IsNullOrEmpty(import) && File.Exists(import))
			{
				var lines = File
					.ReadAllLines(import)
					.Select(line => line.Trim())
					.Where(line => !string.IsNullOrEmpty(line))
					.ToList();
				passwordsForProcessing.AddRange(lines);
				Console.WriteLine($"Received {lines.Count} passwords from file.");
			}

			Console.WriteLine($"Received {passwordsForProcessing.Count} passwords in total.");
			using HashGenerator hashGenerator = algorithm switch
			{
				HashingAlgorithm.Sha256 => new Sha256Generator(),
				HashingAlgorithm.Sha512 => new Sha512Generator(),
				HashingAlgorithm.Md5 => new Md5Generator(),
				_ => throw new NotSupportedException("Unknown hashing algorithm."),
			};
			var passwordHashMap = passwordsForProcessing
				.OrderBy(password => password)
				.ToDictionary
				(
					password => password,
					password => hashGenerator.Generate(password)
				);

			var output = $"{{{Environment.NewLine}{string.Join($",{Environment.NewLine}", passwordHashMap.Select(pair => $"\t\"{pair.Key}\": \"{pair.Value}\""))}{Environment.NewLine}}}";

			if (!string.IsNullOrEmpty(export))
			{
				File.WriteAllText(export, output);
				Console.WriteLine($"Hash generation is complete. Check the \"{export}\".");
			}
			else
			{
				Console.WriteLine(output);
			}
		}
	}
}
