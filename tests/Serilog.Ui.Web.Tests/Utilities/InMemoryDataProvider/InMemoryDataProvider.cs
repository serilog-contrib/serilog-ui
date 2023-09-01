using Newtonsoft.Json;
using Serilog.Events;
using Serilog.Sinks.InMemory;
using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ui.Web.Tests.Utilities.InMemoryDataProvider;

public class SerilogInMemoryDataProvider : IDataProvider
{
    public string Name => nameof(SerilogInMemoryDataProvider);

    public Task<(IEnumerable<LogModel>, int)> FetchDataAsync(int page, int count, string? level = null, string? searchCriteria = null, DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var events = InMemorySink.Instance.LogEvents;

        if (searchCriteria != null)
            events = events.Where(l => l.RenderMessage().Contains(searchCriteria, StringComparison.CurrentCultureIgnoreCase));

        if (level != null)
        {
            Enum.TryParse("Active", out LogEventLevel logLevel);
            events = events.Where(l => l.Level == logLevel);
        }

        var logs = events.Skip((page - 1) * count).Take(count).Select(l => new LogModel
        {
            Level = l.Level.ToString(),
            Exception = l.Exception?.ToString(),
            Message = l.RenderMessage(),
            Properties = JsonConvert.SerializeObject(l.Properties),
            PropertyType = "Json",
            Timestamp = l.Timestamp.DateTime
        });

        return Task.FromResult((logs, events.Count()));
    }
}