using Microsoft.Extensions.Configuration;

namespace eFarmaResultProcessor
{
	internal class Program
	{
		private static ISet<string> GetFiles(IEnumerable<string> sources) =>
			sources
				.Where(s => Directory.Exists(s))
				.SelectMany(s => Directory.EnumerateFiles(s, "*.*", SearchOption.AllDirectories))
				.Concat(sources
					.Where(s => File.Exists(s)))
				.Where(s => string.Equals(Path.GetExtension(s), ".xml", StringComparison.OrdinalIgnoreCase))
				.ToHashSet()
			;

		private static int ProcessReturn(int code, bool autoClose)
		{
			if (!autoClose)
			{
				Console.WriteLine("Нажмите ENTER для выхода");
				Console.Read();
			}

			return code;
		}

		private static int Main(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				.AddIniFile("config.ini")
				.AddCommandLine(args)
				.Build()
			;

			var options = configuration.GetSection("Main").Get<Options>();

			if (!args.Any())
			{
				Console.Error.WriteLine("Не указаны файлы/папки для обработки");
				return ProcessReturn(1, options.AutoCloseOnError);
			}

			var sources = args
				.Where(a => File.Exists(a) || Directory.Exists(a))
			;

			var files = GetFiles(sources);
			if (!files.Any())
			{
				Console.Error.WriteLine("Указанные файлы недоступны или не в том формате");
				return ProcessReturn(2, options.AutoCloseOnError);
			}

			return 0;
		}
	}
}