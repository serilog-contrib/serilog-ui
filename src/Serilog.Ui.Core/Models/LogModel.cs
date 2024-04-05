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
        public virtual int RowNo { get; private set; }

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

        /// <summary>
        /// It sets the RowNo using the RowStart and the log index.
        /// </summary>
        /// <param name="rowNoStart"></param>
        /// <param name="index"></param>
        public LogModel SetRowNo(int rowNoStart, int index)
        {
            RowNo = rowNoStart + index + 1;

            return this;
        }
    }
}