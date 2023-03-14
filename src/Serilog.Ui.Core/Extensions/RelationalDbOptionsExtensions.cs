namespace Serilog.Ui.Core
{
    public static class RelationalDbOptionsExtensions
    {
        public static string ToDataProviderName(this RelationalDbOptions options, string providerName)
            => string.Join(".", providerName, options.Schema, options.TableName);
    }
}