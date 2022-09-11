using System.Threading.Tasks;

namespace Serilog.Ui.Common.Tests.TestSuites
{
    public interface IIntegrationPaginationTests
    {
        Task It_throws_when_page_param_is_zero();
    }
}
