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

        // https://github.com/saleem-mirza/serilog-sinks-mysql/blob/dev/src/Serilog.Sinks.MySQL/Sinks/MySQL/MySqlSink.cs
        public const string MySqlCreateTable = "CREATE TABLE IF NOT EXISTS Logs (" +
            "id INT NOT NULL AUTO_INCREMENT PRIMARY KEY," +
            "Timestamp VARCHAR(100)," +
            "LogLevel VARCHAR(15)," +
            "Template TEXT," +
            "Message TEXT," +
            "Exception TEXT," +
            "Properties TEXT," +
            "_ts TIMESTAMP DEFAULT CURRENT_TIMESTAMP" +
            ")";

        public const string MySqlInsertFakeData = "INSERT INTO Logs" +
            "(Timestamp, LogLevel, Template, Message, Exception, Properties)" +
            "VALUES (" +
            $"@{nameof(LogModel.Timestamp)}," +
                $"@{nameof(LogModel.Level)}," +
                $"@{nameof(LogModel.Message)}," +
                $"@{nameof(LogModel.Message)}," +
                $"@{nameof(LogModel.Exception)}," +
                $"@{nameof(LogModel.Properties)}" +
            ")";

        // https://github.com/b00ted/serilog-sinks-postgresql/blob/master/Serilog.Sinks.PostgreSQL/Sinks/PostgreSQL/ColumnOptions.cs
        public const string PostgresCreateTable = "CREATE TABLE IF NOT EXISTS public.logs (" +
            "timestamp TimestampTz," +
            "level INTEGER," +
            "message_template TEXT," +
            "message TEXT," +
            "exception TEXT," +
            "log_event TEXT" +
            ")";

        public const string PostgresInsertFakeData = "INSERT INTO public.logs" +
            "(timestamp, level, message_template, message, exception, log_event)" +
            "VALUES (" +
            $"@{nameof(LogModel.Timestamp)}," +
                $"@{nameof(LogModel.Level)}," +
                $"@{nameof(LogModel.Message)}," +
                $"@{nameof(LogModel.Message)}," +
                $"@{nameof(LogModel.Exception)}," +
                $"@{nameof(LogModel.Properties)}" +
            ")";

        // https://github.com/saleem-mirza/serilog-sinks-sqlite/blob/dev/src/Serilog.Sinks.SQLite/Sinks/SQLite/SQLiteSink.cs
        public const string SqliteCreateTable = "CREATE TABLE IF NOT EXISTS Logs (" +
            "id INTEGER PRIMARY KEY AUTOINCREMENT," +
            "Timestamp TEXT," +
            "LogLevel VARCHAR(10)," +
            "Exception TEXT," +
            "Message TEXT," +
            "Properties TEXT," +
            "_ts TEXT DEFAULT (strftime('%Y-%m-%d %H:%M:%f','now'))" + // SQLite equivalent for CURRENT_TIMESTAMP
        ")";

        public const string SqliteInsertFakeData = "INSERT INTO Logs" +
            "(Timestamp, LogLevel, Exception, Message, Properties)" +
            "VALUES (" +
            $"@{nameof(LogModel.Timestamp)}," +
            $"@{nameof(LogModel.Level)}," +
            $"@{nameof(LogModel.Exception)}," +
            $"@{nameof(LogModel.Message)}," +
            $"@{nameof(LogModel.Properties)}" +
            ")";

    }
}
