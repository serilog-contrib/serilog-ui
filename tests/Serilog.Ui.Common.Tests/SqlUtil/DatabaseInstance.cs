using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using Npgsql;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.Core;

namespace Serilog.Ui.Common.Tests.SqlUtil
{
#nullable enable
    public abstract class DatabaseInstance<TContainer, TConfiguration> : IIntegrationRunner
        where TContainer : TestcontainerDatabase
        where TConfiguration : TestcontainerDatabaseConfiguration, new()
    {
        private bool disposedValue;

        protected TConfiguration? Configuration = new() { Password = "#DockerFakePw#" };

        protected virtual string Name { get; } = nameof(TContainer);

        /// <summary>
        /// Gets or sets the Testcontainers container.
        /// </summary>
        protected TContainer? Container { get; set; }

        /// <summary>
        /// Gets or sets the IDataProvider.
        /// </summary>
        protected IDataProvider? Provider { get; set; }

        protected LogModelPropsCollector? Collector { get; set; }

        public IDataProvider GetDataProvider() => Guard.Against.Null(Provider)!;

        public LogModelPropsCollector GetPropsCollector() => Guard.Against.Null(Collector)!;

        public Task InitializeAsync()
        {
            Container = new TestcontainersBuilder<TContainer>()
             .WithDatabase(Configuration)
             .WithName($"Testing_{Name}_{Guid.NewGuid()}")
             .WithWaitStrategy(Wait.ForUnixContainer())
             .WithStartupCallback(async (dc, token) => await GetDbContextInstanceAsync(token))
             .Build();

            return Container.StartAsync();
        }

        /// <summary>
        /// Register an operation to check for db readiness.
        /// You can access the container connection string from this point onwards.
        /// Runs before <see cref="InitializeAdditionalAsync"/>.
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        protected abstract Task CheckDbReadinessAsync();

        /// <summary>
        /// Register operations to setup the database in a functional status.
        /// Runs after <see cref="CheckDbReadinessAsync"/>.
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        protected abstract Task InitializeAdditionalAsync();

        private async Task GetDbContextInstanceAsync(CancellationToken token)
        {
            int retry = default;

            do
            {
                try
                {
                    await CheckDbReadinessAsync();
                    break;
                }
                catch (Exception ex) when (ex is SqlException || ex is MySqlException || ex is NpgsqlException)
                {
                    retry += 1;
                    await Task.Delay(1000 * retry, token);
                }

            } while (retry < 10);

            await InitializeAdditionalAsync();
        }

        /// <inheritdoc/>
        public async Task DisposeAsync()
        {
            if (Container != null)
            {
                await Container.DisposeAsync()
                  .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Dispose any additional managed dependencies.
        /// </summary>
        /// <param name="disposing">If it's disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Configuration?.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
