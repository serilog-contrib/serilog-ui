using Microsoft.Extensions.DependencyInjection;

namespace Serilog.Ui.Core
{
    public class SerilogUiOptionsBuilder : ISerilogUiOptionsBuilder
    {
        private readonly IServiceCollection _services;

        public SerilogUiOptionsBuilder(IServiceCollection services)
        {
            _services = services;
        }

        IServiceCollection ISerilogUiOptionsBuilder.Services => _services;
    }
}