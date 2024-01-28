using System;

namespace Serilog.Ui.Core
{
    /// <summary>
    /// Log Model class.
    /// </summary>
    public class LogModel
    {
        /// <summary>
        /// It gets or sets RowNo.
        /// </summary>
        public virtual int RowNo { get; set; }

        /// <summary>
        /// It gets or sets Level.
        /// </summary>
        public virtual string Level { get; set; }

        /// <summary>
        /// It gets or sets Message.
        /// </summary>
        public virtual string Message { get; set; }

        /// <summary>
        /// It gets or sets Timestamp.
        /// </summary>
        public virtual DateTime Timestamp { get; set; }

        /// <summary>
        /// It gets or sets Exception.
        /// </summary>
        public virtual string Exception { get; set; }

        /// <summary>
        /// It gets or sets Properties.
        /// </summary>
        public virtual string Properties { get; set; }

        /// <summary>
        /// It gets or sets PropertyType.
        /// </summary>
        public virtual string PropertyType { get; set; }
    }
}