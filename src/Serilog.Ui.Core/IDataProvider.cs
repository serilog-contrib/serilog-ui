using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Serilog.Ui.Core.Models;

namespace Serilog.Ui.Core
{
    /// <summary>
    /// Data provider interface
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// Fetches the log data asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.ValueTuple&lt;IEnumerable&lt;LogModel&gt;, System.Int32&gt;&gt;.</returns>
        Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams, CancellationToken cancellationToken = default);

        /// <summary>
        /// Name of the provider, used to identify this provider when using multiple.
        /// </summary>
        string Name { get; }
    }
}