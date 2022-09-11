using Serilog.Ui.Core;

namespace Serilog.Ui.Common.Tests.SqlUtil
{
    public static class Costants
    {
        // https://github.com/serilog-mssql/serilog-sinks-mssqlserver#table-definition
        public const string MsSqlCreateTable = "CREATE TABLE [Logs] (" +
            "[Id] int IDENTITY(1,1) NOT NULL," +
            "[Message] nvarchar(max) NULL," +
            "[MessageTemplate] nvarchar(max) NULL," +
            "[Level] nvarchar(128) NULL," +
            "[TimeStamp] datetime NOT NULL," +
            "[Exception] nvarchar(max) NULL," +
            "[Properties] nvarchar(max) NULL," +
            "CONSTRAINT[PK_Logs] PRIMARY KEY CLUSTERED([Id] ASC)" +
            ");";

        public const string MsSqlInsertFakeData = "INSERT [Logs]" +
            "(Message, MessageTemplate, Level, TimeStamp, Exception, Properties)" +
            $"values (" +
                $"@{nameof(LogModel.Message)}," +
                $"@{nameof(LogModel.Message)}," +
                $"@{nameof(LogModel.Level)}," +
                $"@{nameof(LogModel.Timestamp)}," +
                $"@{nameof(LogModel.Exception)}," +
                $"@{nameof(LogModel.Properties)}" +
            $")";
    }
}
