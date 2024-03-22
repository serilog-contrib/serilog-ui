using Bogus;
using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Serilog.Ui.Common.Tests.DataSamples
{
    public static class LogModelFaker
    {
        internal static readonly IEnumerable<string> LogLevels = new List<string>
        {
            "Verbose", "Debug", "Information", "Warning", "Error", "Fatal"
        };

        private static Faker<LogModel> LogsRules()
        {
            var refDate = DateTime.Parse("8/8/2019 2:00 PM", CultureInfo.InvariantCulture);
            return new Faker<LogModel>()
                .UseDateTimeReference(refDate)
                .UseSeed(1910)
                .RuleFor(p => p.RowNo, f => f.Database.Random.Int())
                .RuleFor(p => p.Level, f => f.PickRandom(LogLevels))
                .RuleFor(p => p.Properties, PropertiesFaker.SerializedProperties)
                .RuleFor(p => p.Exception, (f) => f.System.Exception().ToString())
                .RuleFor(p => p.Message, f => f.System.Random.Words(6))
                .RuleFor(p => p.PropertyType, f => f.System.CommonFileType())
                .RuleFor(p => p.Timestamp, f => new DateTime(f.Date.Recent().Ticks, DateTimeKind.Utc));
        }

        public static IEnumerable<LogModel> Logs(int generationCount) => LogsRules().Generate(generationCount);

        public static IEnumerable<LogModel> Logs() => Logs(20);
    }
}