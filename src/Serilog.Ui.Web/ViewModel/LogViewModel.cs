using Serilog.Ui.Core;
using System.Collections.Generic;

namespace Serilog.Ui.Web.ViewModel
{
    public class LogViewModel
    {
        public int LogCount { get; set; }

        public IEnumerable<LogModel> Logs { get; set; }
    }
}