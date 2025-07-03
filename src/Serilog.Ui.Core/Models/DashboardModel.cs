using System.Collections.Generic;

namespace Serilog.Ui.Core.Models
{
    /// <summary>
    /// Represents dashboard statistics for log data visualization.
    /// </summary>
    public class DashboardModel
    {
        /// <summary>
        /// Gets or sets the total count of logs.
        /// </summary>
        public int TotalLogs { get; set; }

        /// <summary>
        /// Gets or sets the count of logs by level.
        /// </summary>
        public Dictionary<string, int> LogsByLevel { get; set; } = new();

        /// <summary>
        /// Gets or sets the count of logs for today.
        /// </summary>
        public int TodayLogs { get; set; }

        /// <summary>
        /// Gets or sets the count of error logs for today.
        /// </summary>
        public int TodayErrorLogs { get; set; }
    }
}