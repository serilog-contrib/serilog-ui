using System.Threading.Tasks;

namespace Serilog.Ui.Core.Interfaces
{
    /// <summary>
    /// Authorization filter, used to authorize access to Serilog Ui pages.
    /// Runs synchronous.
    /// </summary>
    public interface IUiAuthorizationFilter
    {
        /// <summary>
        /// Authorizes a request sync.
        /// </summary>
        bool Authorize();
    }

    /// <summary>
    /// Authorization filter, used to authorize access to Serilog Ui pages.
    /// Runs asynchronous.
    /// </summary>
    public interface IUiAsyncAuthorizationFilter
    {
        /// <summary>
        /// Authorizes a request async.
        /// </summary>
        Task<bool> AuthorizeAsync();
    }
}