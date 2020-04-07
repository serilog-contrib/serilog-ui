using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog.Ui.Core;
using System.Collections.Generic;

namespace Serilog.Ui.Web.ViewModel
{
    public class LogViewModel
    {
        public int LogCount { get; set; }

        public IEnumerable<LogModel> Logs { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public IEnumerable<SelectListItem> CountSelectListItems { get; set; }
    }
}