using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Serilog.Ui.Common.Tests.DataSamples
{
    public class LogModelPropsCollector
    {
        public LogModelPropsCollector(IEnumerable<LogModel> models)
        {
            DataSet = models.ToList();
            Collect(models);
        }

        public IReadOnlyCollection<LogModel> DataSet { get; }
        public LogModel Example { get; private set; }
        public Dictionary<string, int> CountByLevel { get; private set; }
        public IEnumerable<DateTime> TimesSamples { get; private set; }
        public IEnumerable<string> MessagePiecesSamples { get; private set; }

        private void Collect(IEnumerable<LogModel> models)
        {
            Example = models.First();

            CountByLevel = LogModelFaker.LogLevels.ToDictionary(p => p, lvl => models.Count(m => m.Level == lvl));

            if (models.Count() < 3) return;

            MessagePiecesSamples = new List<string>
            {
                models.ElementAtOrDefault(0).Message,
                models.ElementAtOrDefault(1).Message.Substring(1, models.ElementAtOrDefault(1).Message.Length / 2),
                models.ElementAtOrDefault(2).Message.Substring(1, models.ElementAtOrDefault(2).Message.Length / 2),
            };
            TimesSamples = new List<DateTime>
            {
                models.ElementAtOrDefault(0).Timestamp,
                models.ElementAtOrDefault(1).Timestamp,
                models.ElementAtOrDefault(2).Timestamp,
            }.OrderBy(p => p.Ticks);
        }
    }
}
