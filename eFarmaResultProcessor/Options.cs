using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFarmaResultProcessor
{
	internal class Options
	{
		public string OutputSuffix { get; set; }
		public string SourceNodes { get; set; }
		public string ResultNodes { get; set; }
		public string DocumentIdNode { get; set; }
		public string StatusNode { get; set; }
		public string RemoveStatuses { get; set; }
		public bool AutoCloseOnError { get; set; }

		public HashSet<string> RemoveStatusesList => System.Text.Json.JsonSerializer.Deserialize<string[]>(RemoveStatuses).ToHashSet();
	}
}
