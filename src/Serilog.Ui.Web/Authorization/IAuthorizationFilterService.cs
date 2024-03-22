using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Serilog.Ui.Web.Authorization
{
    internal interface IAuthorizationFilterService
    {
        Task CheckAccessAsync(Func<Task> onSuccess, Func<HttpResponse, Task> onFailure = null);
    }
}