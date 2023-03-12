using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Core;
using System;
using Xunit;

namespace Serilog.Ui.Common.Tests.TestSuites
{
    public interface IIntegrationRunner : IAsyncLifetime, IDisposable
    {
        IDataProvider GetDataProvider();
        LogModelPropsCollector GetPropsCollector();
    }
}
