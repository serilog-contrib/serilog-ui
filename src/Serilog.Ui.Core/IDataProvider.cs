using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Serilog.Ui.Core.Models;

namespace Serilog.Ui.Core;

/// <summary>
///     Data provider interface
/// </summary>
public interface IDataProvider
{
    /// <summary>
    ///     Name of the provider, used to identify this provider when using multiple.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     Fetches the log data asynchronous.
    /// </summary>
    Task<(IEnumerable<LogModel> results, int total)> FetchDataAsync(FetchLogsQuery queryParams,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Fetches dashboard statistics asynchronous.
    /// </summary>
    Task<LogStatisticModel> FetchDashboardAsync(CancellationToken cancellationToken = default);
}