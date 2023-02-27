using Ardalis.GuardClauses;
using FluentAssertions;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Serilog.Ui.Common.Tests.TestSuites.Impl
{
    public abstract class IntegrationPaginationTests<DbRunner> :
        IClassFixture<DbRunner>, IIntegrationPaginationTests, IDisposable
        where DbRunner : class, IIntegrationRunner
    {
        private bool disposedValue;
        private readonly DbRunner dbRunner;
        protected readonly LogModelPropsCollector logCollector;
        protected readonly IDataProvider provider;

        protected IntegrationPaginationTests(DbRunner instance)
        {
            dbRunner = instance;
            logCollector = instance.GetPropsCollector();
            provider = Guard.Against.Null(instance.GetDataProvider());
        }

        [Fact]
        public virtual async Task It_fetches_with_limit()
        {
            var (Logs, _) = await provider.FetchDataAsync(1, 5);

            Logs.Should().NotBeEmpty().And.HaveCount(5);
        }

        [Fact]
        public virtual async Task It_fetches_with_limit_and_skip()
        {
            var example = logCollector.Example;
            var (Logs, _) = await provider.FetchDataAsync(2, 1, level: example.Level);

            Logs.Should().NotBeEmpty().And.HaveCount(1);
            Logs.First().Level.Should().Be(example.Level);
            Logs.First().Message.Should().NotBe(example.Message);
        }

        [Fact]
        public virtual async Task It_fetches_with_skip()
        {
            var example = logCollector.Example;
            var (Logs, _) = await provider.FetchDataAsync(2, 1, level: example.Level);

            Logs.First().Level.Should().Be(example.Level);
            Logs.First().Message.Should().NotBe(example.Message);
        }

        public abstract Task It_throws_when_skip_is_zero();

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    dbRunner.Dispose();
                }

                disposedValue = true;
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
