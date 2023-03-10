namespace Serilog.Ui.Core.Extensions
{
    public static class RelationalDbOptionsExtensions
    {
        public static string ToDataProviderName(this RelationalDbOptions options, string providerName)
            => string.Join(".", "MSSQL", options.Schema, options.TableName);
    }
}
