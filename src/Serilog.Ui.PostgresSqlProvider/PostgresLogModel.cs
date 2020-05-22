using Serilog.Ui.Core;

namespace Serilog.Ui.PostgresSqlProvider
{
    internal class PostgresLogModel : LogModel
    {
        private string _level;

        public override string Level
        {
            get => _level;
            set
            {
                switch (value)
                {
                    case "0":
                        _level = "Verbose";
                        break;

                    case "1":
                        _level = "Debug";
                        break;

                    case "2":
                        _level = "Information";
                        break;

                    case "3":
                        _level = "Warning";
                        break;

                    case "4":
                        _level = "Error";
                        break;

                    default:
                        _level = "";
                        break;
                }
            }
        }
    }
}