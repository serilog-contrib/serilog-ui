using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Serilog.Ui.Common.Tests.DataSamples
{
    public class LogModelPropsCollector
    {
        public LogModelPropsCollector(IEnumerable<LogModel> models)
            : this(models.ToList())
        {
        }

        public LogModelPropsCollector(ICollection<LogModel> models)
        {
            DataSet = models.ToList();
            Collect(models);
        }

        public IReadOnlyCollection<LogModel> DataSet { get; }
        public LogModel Example { get; private set; }
        public Dictionary<string, int> CountByLevel { get; private set; }
        public IEnumerable<DateTime> TimesSamples { get; private set; }
        public IEnumerable<string> MessagePiecesSamples { get; private set; }

        private void Collect(ICollection<LogModel> models)
        {
            Example = models.First();

            CountByLevel = LogModelFaker.LogLevels.ToDictionary(p => p, lvl => models.Count(m => m.Level == lvl));

            if (models.Count() < 3) return;

            MessagePiecesSamples = new List<string>
            {
                models.ElementAt(0).Message,
                models.ElementAt(1).Message.Substring(1, models.ElementAt(1).Message.Length / 2),
                models.ElementAt(2).Message.Substring(1, models.ElementAt(2).Message.Length / 2),
            };
            TimesSamples = new List<DateTime>
            {
                models.ElementAt(0).Timestamp,
                models.ElementAt(1).Timestamp,
                models.ElementAt(2).Timestamp,
            }.OrderBy(p => p.Ticks);
        }
    }
}
