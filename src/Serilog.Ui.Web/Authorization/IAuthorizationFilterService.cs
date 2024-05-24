using System;
using System.Threading.Tasks;

namespace Serilog.Ui.Web.Authorization
{
    internal interface IAuthorizationFilterService
    {
        Task CheckAccessAsync(Func<Task> onSuccess, Func<Task>? onFailure = null);
    }
}