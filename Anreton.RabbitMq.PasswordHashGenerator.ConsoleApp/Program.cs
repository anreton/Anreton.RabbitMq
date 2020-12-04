using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;

using Anreton.RabbitMq.HashGenerator;
using Anreton.RabbitMq.HashGenerator.Abstractions;

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

			var hashAlgorithmOption = new Option<HashAlgorithm>
			(
				alias: "--algorithm",
				getDefaultValue: () => HashAlgorithm.SHA256,
				description: "The hash algorithm."
			);
			hashAlgorithmOption.AddAlias("-a");

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
				hashAlgorithmOption,
				importOption,
				exportOption
			};
			rootCommand.Handler = CommandHandler.Create<IEnumerable<string>, HashAlgorithm, string, string>(Handler);

			_ = rootCommand.Invoke(args);
		}

		/// <summary>
		/// Handles input arguments and prints the output.
		/// </summary>
		/// <param name="passwords">
		/// Passwords to get a hashes.
		/// </param>
		/// <param name="hashAlgorithm">
		/// The hash algorithm.
		/// </param>
		/// <param name="import">
		/// The path to the file with passwords.
		/// </param>
		/// <param name="export">
		/// The path to the file where to export results.
		/// </param>
		/// <exception cref="NotSupportedException">
		/// Unknown <see cref="HashAlgorithm"/> used.
		/// </exception>
		private static void Handler
		(
			IEnumerable<string> passwords,
			HashAlgorithm hashAlgorithm,
			string import,
			string export
		)
		{
			var passwordsForProcessing = new List<string>();

			if (passwords is not null && passwords.Any())
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
			using Generator generator = hashAlgorithm switch
			{
				HashAlgorithm.SHA256 => new SHA256Generator(),
				HashAlgorithm.SHA512 => new SHA512Generator(),
				HashAlgorithm.MD5 => new MD5Generator(),
				_ => throw new NotSupportedException($"Unknown {nameof(HashAlgorithm)}."),
			};
			var passwordHashMap = passwordsForProcessing
				.OrderBy(password => password)
				.ToDictionary
				(
					password => password,
					password => generator.Generate(password)
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
