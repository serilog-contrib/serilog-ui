using Microsoft.AspNetCore.Mvc;
using Serilog.Ui.Core;
using Serilog.Ui.Web.ViewModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Ui.Web.Controllers
{
    public class LogsController : Controller
    {
        private static string _scripts;
        private static string _styles;
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

            GetResources();

            return View(viewModel);
        }

        private void GetResources()
        {
            if (_styles != null)
            {
                ViewData["Styles"] = _styles;
                ViewData["Scripts"] = _scripts;
                return;
            }

            _styles = SetResource("Serilog.Ui.Web.wwwroot.css.main.css");
            ViewData["Styles"] = _styles;

            _scripts = SetResource("Serilog.Ui.Web.wwwroot.js.main.js");
            ViewData["Scripts"] = _scripts;
        }

        private string SetResource(string resourceName)
        {
            var resourceStream = GetType().Assembly.GetManifestResourceStream(resourceName);
            var resource = string.Empty;

            using var reader = new StreamReader(resourceStream, Encoding.UTF8);
            resource += reader.ReadToEnd();

            return resource;
        }
    }
}