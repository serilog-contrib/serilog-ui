namespace Serilog.Ui.Core.Models
{
    public class SearchOptions
    {
        public enum SortProperty
        {
            Timestamp,
            Level,
            Message
        }
        public enum SortDirection
        {
            Asc,
            Desc
        }
    }
}
