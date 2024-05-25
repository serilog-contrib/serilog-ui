using System;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Core;
using Xunit;

namespace Serilog.Ui.Common.Tests.TestSuites
{
    public interface IIntegrationRunner : IAsyncLifetime, IDisposable
    {
        IDataProvider GetDataProvider();
        LogModelPropsCollector GetPropsCollector();
    }
}