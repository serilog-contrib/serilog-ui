using FluentAssertions;
using Xunit;

namespace Serilog.Ui.PostgreSqlProvider.Tests.Model
{
    public class LogLevelConverterTest
    {
        [Theory]
        [InlineData("0", "Verbose")]
        [InlineData("1", "Debug")]
        [InlineData("2", "Information")]
        [InlineData("3", "Warning")]
        [InlineData("4", "Error")]
        [InlineData("5", "Fatal")]
        [InlineData("random", "")]
        public void It_maps_the_correct_log_level_name(string input, string expected)
        {
            var act = LogLevelConverter.GetLevelName(input);
            act.Should().Be(expected);
        }

        [Theory]
        [InlineData("Verbose", 0)]
        [InlineData("Debug", 1)]
        [InlineData("Information", 2)]
        [InlineData("Warning", 3)]
        [InlineData("Error", 4)]
        [InlineData("Fatal", 5)]
        [InlineData("random", 100)]
        public void It_maps_the_correct_log_level_value(string input, int expected)
        {
            var act = LogLevelConverter.GetLevelValue(input);
            act.Should().Be(expected);
        }
    }
}
