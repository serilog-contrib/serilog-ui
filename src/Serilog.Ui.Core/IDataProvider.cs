using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// <param name="page">The page.</param>
        /// <param name="count">The count.</param>
        /// <param name="level">The log level to filter log.</param>
        /// <param name="searchCriteria">The search criteria to filter log.</param>
        /// <param name="startDate">The start date to filter log.</param>
        /// <param name="endDate">The end date to filter log.</param>
        /// <returns>Task&lt;System.ValueTuple&lt;IEnumerable&lt;LogModel&gt;, System.Int32&gt;&gt;.</returns>
        Task<(IEnumerable<LogModel> Logs, int Count)> FetchDataAsync(
            int page,
            int count,
            string level = null,
            string searchCriteria = null,
            DateTime? startDate = null,
            DateTime? endDate = null
        );
    }
}