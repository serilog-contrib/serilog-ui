using Serilog.Ui.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Serilog.Ui.MsSqlServerProvider
{
    public class SqlServerDataProvider : IDataProvider
    {
        private readonly SqlServerOptions _options;

        public SqlServerDataProvider(SqlServerOptions options)
        {
            _options = options;
        }

        public Task<(IEnumerable<LogModel>, int)> FetchDataAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }
    }
}