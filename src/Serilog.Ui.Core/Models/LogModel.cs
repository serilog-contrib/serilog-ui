using System;

namespace Serilog.Ui.Core
{
    public class LogModel
    {
        public int Id { get; set; }

        public string Level { get; set; }

        public string Message { get; set; }

        public DateTime Timestamp { get; set; }

        public string Exception { get; set; }
    }
}