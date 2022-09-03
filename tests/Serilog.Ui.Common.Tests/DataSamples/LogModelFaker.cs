using Bogus;
using Newtonsoft.Json;
using Serilog.Ui.Core;
using System.Collections.Generic;

namespace Serilog.Ui.Common.Tests.DataSamples
{
    public static class LogModelFaker
    {
        private static readonly IEnumerable<string> LogLevels = new List<string>
        {
            "Verbose", "Debug", "Information", "Warning", "Error", "Fatal"
        };
        private static readonly Faker<LogModel> logsRules = new Faker<LogModel>()
            .RuleFor(p => p.Level, f => f.PickRandom(LogLevels))
            .RuleFor(p => p.Properties, PropertiesFaker.SerializedProperties)
            .RuleFor(p => p.Exception, (f) => JsonConvert.SerializeObject(f.System.Exception()))
            .RuleFor(p => p.Message, f => f.System.Random.Words(5))
            .RuleFor(p => p.PropertyType, f => f.System.CommonFileType())
            .RuleFor(p => p.Timestamp, f => f.Date.Recent())
            .RuleFor(p => p.RowNo, f => f.Database.Random.Int());

        public static IEnumerable<LogModel> Logs(int generationCount) => logsRules.Generate(generationCount);
        public static IEnumerable<LogModel> Logs() => Logs(20);
    }
}
