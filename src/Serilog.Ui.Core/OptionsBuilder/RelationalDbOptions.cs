namespace Serilog.Ui.Core
{
    /// <summary>
    /// RelationDbOptions class
    /// </summary>
    public class RelationalDbOptions
    {
        /// <summary>
        /// It gets or sets ConnectionString.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// It gets or sets TableName.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// It gets or sets Schema.
        /// </summary>
        public string Schema { get; set; }
    }
}