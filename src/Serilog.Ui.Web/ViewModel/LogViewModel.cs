using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog.Ui.Core;
using System.Collections.Generic;

namespace Serilog.Ui.Web.ViewModel
{
    public class LogViewModel
    {
        public int LogCount { get; set; }

        public IEnumerable<LogModel> Logs { get; set; }

        public int CurrentPage { get; set; }

        public int Count { get; set; }

        public string SearchCriteria { get; set; }

        public IEnumerable<SelectListItem> LogCountSelectListItems { get; set; }

        public IEnumerable<SelectListItem> LogLevelSelectListItems { get; set; }
    }
}