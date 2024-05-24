namespace Serilog.Ui.PostgreSqlProvider
{
    internal static class LogLevelConverter
    {
        public static string GetLevelName(string? value) => value switch
        {
            "0" => "Verbose",
            "1" => "Debug",
            "2" => "Information",
            "3" => "Warning",
            "4" => "Error",
            "5" => "Fatal",
            _ => ""
        };

        public static int GetLevelValue(string? name) => name switch
        {
            "Verbose" => 0,
            "Debug" => 1,
            "Information" => 2,
            "Warning" => 3,
            "Error" => 4,
            "Fatal" => 5,
            _ => 100
        };
    }
}