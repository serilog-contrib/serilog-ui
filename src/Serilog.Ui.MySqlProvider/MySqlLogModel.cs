using Serilog.Ui.Core;

namespace Serilog.Ui.MySqlProvider
{
    internal class MySqlLogModel : LogModel
    {
        public override string PropertyType => "json";
    }
}