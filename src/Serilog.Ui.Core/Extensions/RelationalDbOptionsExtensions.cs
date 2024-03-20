using Serilog.Ui.Core.OptionsBuilder;

namespace Serilog.Ui.Core
{
    /// <summary>
    /// Relational Db Options extensions.
    /// </summary>
    public static class RelationalDbOptionsExtensions
    {
        /// <summary>
        /// Generates a complete data provider name, by using its properties.
        /// </summary>
        /// <param name="options"><see cref="RelationalDbOptions"/></param>
        /// <param name="providerName">Data provider name.</param>
        /// <returns></returns>
        public static string ToDataProviderName(this RelationalDbOptions options, string providerName)
            => string.Join(".", providerName, options.Schema, options.TableName);
    }
}