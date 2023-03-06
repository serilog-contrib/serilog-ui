using Ardalis.GuardClauses;
using MongoDb.Tests.Util.Builders;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.Core;
using Serilog.Ui.MongoDbProvider;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MongoDb.Tests.Util
{
    [CollectionDefinition(nameof(MongoDbDataProvider))]
    public class MongoCollection : ICollectionFixture<BaseIntegrationTest> { }

    public class BaseIntegrationTest : IIntegrationRunner
    {
        private bool _disposedValue;

        internal BaseServiceBuilder? _builder;

        public Task DisposeAsync() => Task.CompletedTask;

        public virtual async Task InitializeAsync()
        {
            _builder = await MongoDbDataProviderBuilder.Build(false);
        }

        public IDataProvider GetDataProvider() => Guard.Against.Null(_builder?._sut)!;

        public LogModelPropsCollector GetPropsCollector() => Guard.Against.Null(_builder?._collector)!;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing && _builder != null)
                {
                    _builder._runner.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

}
