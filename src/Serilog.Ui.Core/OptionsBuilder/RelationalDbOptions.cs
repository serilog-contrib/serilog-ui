namespace Serilog.Ui.Core
{
    public class RelationalDbOptions
    {
        public string ConnectionString { get; set; }

        public string TableName { get; set; }

        public string Schema { get; set; }
    }
}