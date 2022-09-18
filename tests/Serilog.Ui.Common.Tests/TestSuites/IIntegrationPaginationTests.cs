using System.Threading.Tasks;

namespace Serilog.Ui.Common.Tests.TestSuites
{
    public interface IIntegrationPaginationTests
    {
        Task It_fetches_with_limit_and_skip();

        Task It_fetches_with_limit();

        Task It_fetches_with_skip();

        Task It_throws_when_skip_is_zero();
    }
}
