namespace Serilog.Ui.MsSqlServerProvider
{
    public class SqlServerOptions
    {
        public string ConnectionString { get; set; }

        public string TableName { get; set; }

        public string SchemaName { get; set; } = "dbo";
    }
}