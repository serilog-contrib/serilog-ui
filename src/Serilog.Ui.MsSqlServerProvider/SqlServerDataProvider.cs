using Serilog.Ui.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Serilog.Ui.MsSqlServerProvider
{
    public class SqlServerDataProvider : IDataProvider
    {
        private readonly RelationalDbOptions _options;

        public SqlServerDataProvider(RelationalDbOptions options)
        {
            _options = options;
        }

        public Task<(IEnumerable<LogModel>, int)> FetchDataAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }
    }
}