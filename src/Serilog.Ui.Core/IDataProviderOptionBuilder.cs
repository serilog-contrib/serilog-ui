using Microsoft.Extensions.DependencyInjection;

namespace Serilog.Ui.Core
{
    public interface IDataProviderOptionBuilderInfrastructure
    {
    }

    public class DataProviderOptionBuilder
    {
        //private DataProviderOption _options;

        //public DataProviderOptionBuilder(DataProviderOption options)
        //{
        //    Options = options;
        //}

        /// <summary>
        ///     Gets the options being configured.
        /// </summary>
        // public virtual DataProviderOption Options { get; }

        public DataProviderOptionBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }

    public class DataProviderOption
    {
        //private RelationalDbOptions _relationalDbOptions;

        //public DataProviderOption(RelationalDbOptions relationalDbOptions)
        //{
        //    _relationalDbOptions = relationalDbOptions;
        //}
    }

    public class RelationalDbOptions
    {
        public string ConnectionString { get; set; }

        public string TableName { get; set; }

        public string Schema { get; set; }
    }
}