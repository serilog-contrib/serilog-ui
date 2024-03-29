﻿using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Serilog.Ui.Web.Endpoints
{
    public interface ISerilogUiAppRoutes: ISerilogUiOptionsSetter
    {
        Task GetHomeAsync(HttpContext httpContext);

        Task RedirectHomeAsync(HttpContext httpContext);
    }
}
