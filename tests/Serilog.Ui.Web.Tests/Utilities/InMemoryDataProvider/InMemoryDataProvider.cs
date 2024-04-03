using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog.Sinks.InMemory;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Models;

namespace Serilog.Ui.Web.Tests.Utilities.InMemoryDataProvider;

public class SerilogInMemoryDataProvider : IDataProvider
{
    public string Name => nameof(SerilogInMemoryDataProvider);

    public Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams, CancellationToken cancellationToken = default)
    {
        var events = InMemorySink.Instance.LogEvents;

        if (queryParams.SearchCriteria != null)
            events = events.Where(l => l.RenderMessage().Contains(queryParams.SearchCriteria, StringComparison.CurrentCultureIgnoreCase));

        if (queryParams.Level != null && Enum.TryParse("Active", out LogEventLevel logLevel))
        {
            events = events.Where(l => l.Level == logLevel);
        }

        var logs = events
            .Skip(queryParams.Page * queryParams.Count)
            .Take(queryParams.Count)
            .Select(l => new LogModel
            {
                Level = l.Level.ToString(),
                Exception = l.Exception?.ToString(),
                Message = l.RenderMessage(),
                Properties = JsonSerializer.Serialize(l.Properties),
                PropertyType = "Json",
                Timestamp = l.Timestamp.DateTime
            });

        return Task.FromResult((logs, events.Count()));
    }
}