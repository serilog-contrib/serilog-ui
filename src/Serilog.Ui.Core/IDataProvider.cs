using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Serilog.Ui.Core
{
    public interface IDataProvider
    {
        Task<(IEnumerable<LogModel>, int)> FetchDataAsync(
            int page,
            int count,
            string level = null,
            string searchCriteria = null,
            DateTime? startDate = null,
            DateTime? endDate = null
        );
    }
}