using Microsoft.Extensions.DependencyInjection;

namespace Serilog.Ui.Core
{
    public class DataProviderOptionsBuilder : IDataProviderOptionsBuilder
    {
        private readonly IServiceCollection _services;

        public DataProviderOptionsBuilder(IServiceCollection services)
        {
            _services = services;
        }

        IServiceCollection IDataProviderOptionsBuilder.Services => _services;
    }
}