using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog.Ui.Core;
using Serilog.Ui.Web.Filters;
using Serilog.Ui.Web.ViewModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Ui.Web.Controllers
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class LogsController : Controller
    {
        private static readonly string Scripts;
        private static readonly string Styles;
        private static readonly IEnumerable<SelectListItem> LogCountSelectListItems;
        private static readonly IEnumerable<SelectListItem> LogLevelSelectListItems;
        private readonly IDataProvider _dataProvider;

        static LogsController()
        {
            Styles = SetResource("Serilog.Ui.Web.wwwroot.css.main.min.css");
            Scripts = SetResource("Serilog.Ui.Web.wwwroot.js.main.min.js");
            LogCountSelectListItems = new List<SelectListItem>
            {
                new SelectListItem {Text = "10", Value = "10", Selected = true},
                new SelectListItem {Text = "25", Value = "25"},
                new SelectListItem {Text = "50", Value = "50"},
                new SelectListItem {Text = "100", Value = "100"}
            };
            LogLevelSelectListItems = new List<SelectListItem>
            {
                new SelectListItem {Text = "---", Value = "", Selected = true},
                new SelectListItem {Text = "Verbose", Value = "Verbose"},
                new SelectListItem {Text = "Debug", Value = "Debug"},
                new SelectListItem {Text = "Information", Value = "Information"},
                new SelectListItem {Text = "Warning", Value = "Warning"},
                new SelectListItem {Text = "Error", Value = "Error"}
            };
        }

        public LogsController(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<IActionResult> Index(int page = 1, int count = 10, string level = null, string search = null)
        {
            Parallel.ForEach(new List<int>(), async i => { await _dataProvider.FetchDataAsync(page, count, level, search); });
            if (page < 1)
                page = 1;

            if (count > 100)
                count = 100;

            SetLogCountSelectListItem(count);

            SetLogLevelSelectListItem(level);

            var (logs, logCount) = await _dataProvider.FetchDataAsync(page, count, level, search);
            var viewModel = new LogViewModel
            {
                LogCount = logCount,
                Logs = logs,
                Page = page,
                Count = count,
                SearchCriteria = search,
                LogCountSelectListItems = LogCountSelectListItems,
                LogLevelSelectListItems = LogLevelSelectListItems
            };

            ViewData["Styles"] = Styles;
            ViewData["Scripts"] = Scripts;

            return View(viewModel);
        }

        private static void SetLogLevelSelectListItem(string level)
        {
            if (LogLevelSelectListItems.First(i => i.Selected).Value != level)
            {
                LogLevelSelectListItems.First(i => i.Selected).Selected = false;
                var x = LogLevelSelectListItems.FirstOrDefault(i => i.Value == level);
                if (x != null)
                    x.Selected = true;
                else
                    LogLevelSelectListItems.First().Selected = true;
            }

            ;
        }

        private static void SetLogCountSelectListItem(int count)
        {
            if (LogCountSelectListItems.First(i => i.Selected).Value != count.ToString())
            {
                LogCountSelectListItems.First(i => i.Selected).Selected = false;
                var x = LogCountSelectListItems.FirstOrDefault(i => i.Value == count.ToString());
                if (x != null)
                    x.Selected = true;
                else
                    LogCountSelectListItems.First().Selected = true;
            }

            ;
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