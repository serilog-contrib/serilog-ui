using System;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MongoDb.Tests.Util.Builders;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.Core;
using Serilog.Ui.MongoDbProvider;
using Xunit;

namespace MongoDb.Tests.Util
{
    [CollectionDefinition(nameof(MongoDbDataProvider))]
    public class MongoCollection : ICollectionFixture<BaseIntegrationTest>
    {
    }

    public class BaseIntegrationTest : IIntegrationRunner
    {
        private bool _disposedValue;

        internal MongoDbDataProviderBuilder? Builder;

        public Task DisposeAsync() => Task.CompletedTask;

        public virtual async Task InitializeAsync()
        {
            Builder = await MongoDbDataProviderBuilder.Build();
        }

        public IDataProvider GetDataProvider() => Guard.Against.Null(Builder?.Sut);

        public LogModelPropsCollector GetPropsCollector() => Guard.Against.Null(Builder?.Collector);

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing && Builder != null && Builder.Runner != null)
                {
                    Builder.Runner.Dispose();
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