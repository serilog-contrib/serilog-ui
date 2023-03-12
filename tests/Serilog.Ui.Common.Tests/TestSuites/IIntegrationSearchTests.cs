using System.Threading.Tasks;

namespace Serilog.Ui.Common.Tests.TestSuites
{
    public interface IIntegrationSearchTests
    {
        Task It_finds_all_data_with_default_search();

        Task It_finds_same_data_on_same_repeated_search();

        Task It_finds_data_with_all_filters();
        
        Task It_finds_only_data_emitted_after_date();

        Task It_finds_only_data_emitted_before_date();

        Task It_finds_only_data_emitted_in_dates_range();

        Task It_finds_only_data_with_specific_level();

        Task It_finds_only_data_with_specific_message_content();
    }
}
