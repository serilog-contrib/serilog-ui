namespace Serilog.Ui.PostgreSqlProvider
{
    internal class LogLevelConverter
    {
        public static string GetLevelName(string value)
        {
            switch (value)
            {
                case "0":
                    return "Verbose";

                case "1":
                    return "Debug";

                case "2":
                    return "Information";

                case "3":
                    return "Warning";

                case "4":
                    return "Error";

                case "5":
                    return "Fatal";

                default:
                    return "";
            }
        }

        public static int GetLevelValue(string name)
        {
            switch (name)
            {
                case "Verbose":
                    return 0;

                case "Debug":
                    return 1;

                case "Information":
                    return 2;

                case "Warning":
                    return 3;

                case "Error":
                    return 4;

                case "Fatal":
                    return 5;

                default:
                    return 100;
            }
        }
    }
}