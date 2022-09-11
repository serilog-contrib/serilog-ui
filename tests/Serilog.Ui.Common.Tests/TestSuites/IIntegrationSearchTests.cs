using System.Threading.Tasks;
using Xunit;

namespace Serilog.Ui.Common.Tests.TestSuites
{
    public interface IIntegrationSearchTests
    {
        Task It_finds_all_data_with_default_search();
    }
}
