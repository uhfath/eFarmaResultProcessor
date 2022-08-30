using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace eFarmaResultProcessor
{
	internal class Processor
	{
		private readonly IEnumerable<string> _sourceFiles;
		private readonly Options _configOptions;

		public Processor(IEnumerable<string> sources, Options options)
		{
			this._sourceFiles = sources;
			this._configOptions = options;
		}

		public string Process()
		{
			var documents = _sourceFiles
				.AsParallel()
				.Select(s => new
				{
					Path = s,
					Document = XDocument.Load(s),
				})
			;

			var sources = documents
				.Select(d => new
				{
					Path = d.Path,
					Document = d.Document,
					Elements = d.Document.XPathSelectElements(_configOptions.SourceNodes).ToArray(),
				})
				.Where(d => d.Elements.Any())
				.ToArray()
			;

			if (sources.Length == 0)
			{
				return "Не найден исходный файл для обработки.";
			}

			if (sources.Length > 1)
			{
				return "Найдено несколько исходных файлов для обработки. Поддерживается только один.";
			}

			var results = documents
				.Select(d => d.Document.XPathSelectElements(_configOptions.ResultNodes))
				.Where(d => d.Any())
				.ToArray()
			;

			if (results.Length == 0)
			{
				return "Не найден файл с результатми для обработки.";
			}

			var sourceDocument = sources.Single();
			Console.WriteLine("Всего исходных документов: {0}", sourceDocument.Elements.Length);

			var documentsToRemove = results
				.SelectMany(r => r
					.Select(e => new
					{
						Id = e.Element(_configOptions.DocumentIdNode).Value,
						Status = e.Element(_configOptions.StatusNode).Value,
					}))
				.AsParallel()
				.Where(r => _configOptions.RemoveStatusesList.Contains(r.Status))
				.Select(r => r.Id)
				.ToHashSet()
			;

			Console.WriteLine("Документов для удаления: {0}", documentsToRemove.Count);

			sourceDocument.Elements
				.AsParallel()
				.Where(d => documentsToRemove.Contains(d.Element(_configOptions.DocumentIdNode).Value))
				.ToList()
				.ForEach(e => e.Remove())
			;

			Console.WriteLine("Документов после обработки: {0}", sourceDocument.Document.XPathSelectElements(_configOptions.SourceNodes).Count());

			var destinationFile = sourceDocument.Path;
			if (!string.IsNullOrWhiteSpace(_configOptions.OutputSuffix))
			{
				destinationFile = Path.Combine(Path.GetDirectoryName(sourceDocument.Path), Path.GetFileNameWithoutExtension(sourceDocument.Path) + _configOptions.OutputSuffix + Path.GetExtension(sourceDocument.Path));
			}

			Console.WriteLine("Результат сохранён в файл: {0}", Path.GetFileName(destinationFile));
			sourceDocument.Document.Save(destinationFile);

			return string.Empty;
		}
	}
}
