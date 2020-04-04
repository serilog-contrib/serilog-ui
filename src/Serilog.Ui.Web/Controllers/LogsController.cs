using Microsoft.AspNetCore.Mvc;
using Serilog.Ui.Core;
using Serilog.Ui.Web.ViewModel;
using System.Threading.Tasks;

namespace Serilog.Ui.Web.Controllers
{
    public class LogsController : Controller
    {
        private readonly IDataProvider _dataProvider;

        public LogsController(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<IActionResult> Index(int page = 1, int count = 20)
        {
            var (logs, logCount) = await _dataProvider.FetchDataAsync(page, count);
            var viewModel = new LogViewModel
            {
                LogCount = logCount,
                Logs = logs
            };

            return View(viewModel);
        }
    }
}