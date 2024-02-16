namespace Serilog.Ui.Core.Models
{
    /// <summary>
    /// Search options class
    /// </summary>
    public class SearchOptions
    {
        /// <summary>
        /// SortProperty enum, with a list of the columns that can be used to sort by data.
        /// </summary>
        public enum SortProperty
        {
            /// <summary>
            /// Timestamp column.
            /// </summary>
            Timestamp,
            /// <summary>
            /// Level column.
            /// </summary>
            Level,
            /// <summary>
            /// Message column.
            /// </summary>
            Message
        }

        /// <summary>
        /// SortDirection enum, to set the sort direction.
        /// </summary>
        public enum SortDirection
        {
            /// <summary>
            /// Ascending sort direction.
            /// </summary>
            Asc,
            /// <summary>
            /// Descending sort direction.
            /// </summary>
            Desc
        }
    }
}