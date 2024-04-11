using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Serilog.Ui.Web.Authorization
{
    internal interface IAuthorizationFilterService
    {
        Task CheckAccessAsync(Func<Task> onSuccess, Func<HttpResponse, Task> onFailure = null);
    }
}