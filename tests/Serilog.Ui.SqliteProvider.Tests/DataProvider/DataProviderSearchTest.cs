using MsSql.Tests.DataProvider; // TODO nope
using MySql.Tests.Util;
using Serilog.Ui.SqliteDataProvider;
using System.Threading.Tasks;
using Xunit;

namespace Sqlite.Tests.DataProvider
{
    [Collection(nameof(SqliteDataProvider))]
    [Trait("Integration-Search", "Sqlite")]
    public class DataProviderSearchTest : IntegrationSearchTests<SqliteTestProvider>
    {

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
        public DataProviderSearchTest(SqliteTestProvider instance) : base(instance)
        {
        }

        public override Task It_finds_all_data_with_default_search()
            => base.It_finds_all_data_with_default_search();

        public override Task It_finds_data_with_all_filters()
            => base.It_finds_data_with_all_filters();

        public override Task It_finds_only_data_emitted_after_date()
            => base.It_finds_only_data_emitted_after_date();

        public override Task It_finds_only_data_emitted_before_date()
            => base.It_finds_only_data_emitted_before_date();

        public override Task It_finds_only_data_emitted_in_dates_range()
            => base.It_finds_only_data_emitted_in_dates_range();

        public override Task It_finds_only_data_with_specific_level()
            => base.It_finds_only_data_with_specific_level();

        public override Task It_finds_only_data_with_specific_message_content()
            => base.It_finds_only_data_with_specific_message_content();

        public override Task It_finds_same_data_on_same_repeated_search()
            => base.It_finds_same_data_on_same_repeated_search();
    }
}