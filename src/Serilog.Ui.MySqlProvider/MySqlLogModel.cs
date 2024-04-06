using Serilog.Ui.Core.Models;

namespace Serilog.Ui.MySqlProvider
{
    internal class MySqlLogModel : LogModel
    {
        public override string PropertyType => "json";
    }
}