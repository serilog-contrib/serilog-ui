using Serilog.Ui.Core.Models;

namespace Serilog.Ui.PostgreSqlProvider.Models
{
    internal class PostgresLogModel : LogModel
    {
        private string _level;

        public override string Level
        {
            get => _level;
            set => _level = LogLevelConverter.GetLevelName(value);
        }

        public override string PropertyType => "json";
    }
}