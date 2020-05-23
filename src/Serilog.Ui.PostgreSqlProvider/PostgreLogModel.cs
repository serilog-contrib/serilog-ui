using Serilog.Ui.Core;

namespace Serilog.Ui.PostgreSqlProvider
{
    internal class PostgresLogModel : LogModel
    {
        private string _level;

        public override string Level
        {
            get => _level;
            set => _level = LogLevelConverter.GetLevelName(value);
        }
    }
}