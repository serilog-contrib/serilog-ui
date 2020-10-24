using Serilog.Ui.Core;

namespace Serilog.Ui.MsSqlServerProvider
{
    internal class SqlServerLogModel : LogModel
    {
        public override string PropertyType => "xml";
    }
}