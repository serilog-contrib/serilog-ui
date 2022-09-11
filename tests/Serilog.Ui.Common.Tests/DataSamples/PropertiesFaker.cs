using Bogus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Ui.Common.Tests.FakeObjectModels;

namespace Serilog.Ui.Common.Tests.DataSamples
{
    internal static class PropertiesFaker
    {
        internal static Faker<Properties> Properties = new Faker<Properties>()
            .RuleFor(p => p.EventId, f => JsonConvert.SerializeObject(new EventId(f.Random.Int(), f.Hacker.Noun())))
            .RuleForType(typeof(string), p => p.System.Random.Words(3));

        internal static string SerializedProperties = JsonConvert.SerializeObject(Properties.Generate());
    }
}
