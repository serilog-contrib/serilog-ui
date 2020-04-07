using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog.Ui.Core;
using Serilog.Ui.Web.ViewModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Ui.Web.Controllers
{
    public class LogsController : Controller
    {
        private static readonly string Scripts;
        private static readonly string Styles;
        private static readonly IEnumerable<SelectListItem> SelectListItems;
        private readonly IDataProvider _dataProvider;

        static LogsController()
        {
            Styles = SetResource("Serilog.Ui.Web.wwwroot.css.main.css");
            Scripts = SetResource("Serilog.Ui.Web.wwwroot.js.main.js");
            SelectListItems = new List<SelectListItem>
            {
                new SelectListItem {Text = "10", Value = "10"},
                new SelectListItem {Text = "25", Value = "25"},
                new SelectListItem {Text = "50", Value = "50"},
                new SelectListItem {Text = "100", Value = "100"}
            };
        }

        public LogsController(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<IActionResult> Index(int page = 1, int count = 10)
        {
            if (page < 1)
                page = 1;

            if (count > 100)
                count = 100;

            var x = SelectListItems.FirstOrDefault(i => i.Value == count.ToString());
            if (x != null)
                x.Selected = true;
            else
                SelectListItems.First().Selected = true;

            var (logs, logCount) = await _dataProvider.FetchDataAsync(page, count);
            var viewModel = new LogViewModel
            {
                LogCount = logCount,
                Logs = logs,
                Page = page,
                Count = count,
                CountSelectListItems = SelectListItems
            };

            ViewData["Styles"] = Styles;
            ViewData["Scripts"] = Scripts;

            return View(viewModel);
        }

        private static string SetResource(string resourceName)
        {
            var resourceStream = typeof(LogsController).Assembly.GetManifestResourceStream(resourceName);
            var resource = string.Empty;

            using var reader = new StreamReader(resourceStream, Encoding.UTF8);
            resource += reader.ReadToEnd();

            return resource;
        }
    }
}