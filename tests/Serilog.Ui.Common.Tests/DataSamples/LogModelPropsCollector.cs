using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Ui.Core.Models;

namespace Serilog.Ui.Common.Tests.DataSamples
{
    public class LogModelPropsCollector
    {
        public LogModelPropsCollector(ICollection<LogModel> models)
        {
            DataSet = [.. models];
            Example = models.First();

            CountByLevel = LogModelFaker.LogLevels.ToDictionary(p => p, lvl => models.Count(m => m.Level == lvl));

            if (models.Count < 3) return;

            MessagePiecesSamples =
            [
                models.ElementAt(0).Message!,
                models.ElementAt(1).Message!.Substring(1, models.ElementAt(1).Message!.Length / 2),
                models.ElementAt(2).Message!.Substring(1, models.ElementAt(2).Message!.Length / 2)
            ];
            TimesSamples = new List<DateTime>
            {
                models.ElementAt(0).Timestamp,
                models.ElementAt(1).Timestamp,
                models.ElementAt(2).Timestamp
            }.OrderBy(p => p.Ticks);
        }

        public IReadOnlyCollection<LogModel> DataSet { get; }

        public LogModel Example { get; private set; }

        public Dictionary<string, int> CountByLevel { get; private set; }

        public IEnumerable<DateTime> TimesSamples { get; private set; } = [];

        public IEnumerable<string> MessagePiecesSamples { get; private set; } = [];
    }
}