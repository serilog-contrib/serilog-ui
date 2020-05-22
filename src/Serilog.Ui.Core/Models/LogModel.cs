using System;

namespace Serilog.Ui.Core
{
    public class LogModel
    {
        public virtual int Id { get; set; }

        public virtual string Level { get; set; }

        public virtual string Message { get; set; }

        public virtual DateTime Timestamp { get; set; }

        public virtual string Exception { get; set; }

        public virtual string Properties { get; set; }
    }
}