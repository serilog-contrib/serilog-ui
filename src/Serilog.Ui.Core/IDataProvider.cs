using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Serilog.Ui.Core.Models.SearchOptions;

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
        /// <param name="sortOn">The sort column. Defaults to <see cref="SortProperty.Timestamp"/></param>
        /// <param name="sortBy">The sort direction. Defaults to <see cref="SortDirection.Desc"/></param>
        /// <returns>Task&lt;System.ValueTuple&lt;IEnumerable&lt;LogModel&gt;, System.Int32&gt;&gt;.</returns>
        Task<(IEnumerable<LogModel>, int)> FetchDataAsync(
            int page,
            int count,
            string level = null,
            string searchCriteria = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            SortProperty sortOn = SortProperty.Timestamp,
            SortDirection sortBy = SortDirection.Desc
        );

        /// <summary>
        /// Name of the provider, used to identify this provider when using multiple.
        /// </summary>
        string Name { get; }
    }
}
