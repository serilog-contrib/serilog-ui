using System.Text.Json;
using Bogus;
using Microsoft.Extensions.Logging;
using Serilog.Ui.Common.Tests.FakeObjectModels;

namespace Serilog.Ui.Common.Tests.DataSamples;

public static class PropertiesFaker
{
    public static readonly Faker<Properties> Properties = new Faker<Properties>()
        .RuleFor(p => p.EventId, f => new EventId(f.Random.Int(), f.Hacker.Noun()))
        .RuleForType(typeof(string), p => p.System.Random.Words(3));

    internal static readonly string SerializedProperties = JsonSerializer.Serialize(Properties.Generate());
}