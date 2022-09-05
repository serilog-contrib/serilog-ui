import { SearchResult } from "../../../types/types";

export const fakeLogs: SearchResult = {
    "logs": [
        {
            "rowNo": 1,
            "level": "Debug",
            "message": "Hosting starting",
            "timestamp": "2022-01-06T14:38:39.809+01:00",
            "properties": "{\"EventId\":{\"Id\":1,\"Name\":\"Starting\"},\"SourceContext\":\"Microsoft.Extensions.Hosting.Internal.Host\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 2,
            "level": "Debug",
            "message": "Hosting starting",
            "timestamp": "2022-01-06T14:39:24.37+01:00",
            "properties": "{\"EventId\":{\"Id\":1,\"Name\":\"Starting\"},\"SourceContext\":\"Microsoft.Extensions.Hosting.Internal.Host\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 3,
            "level": "Debug",
            "message": "Hosting starting",
            "timestamp": "2022-01-06T14:40:16.642+01:00",
            "properties": "{\"EventId\":{\"Id\":1,\"Name\":\"Starting\"},\"SourceContext\":\"Microsoft.Extensions.Hosting.Internal.Host\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 4,
            "level": "Information",
            "message": "User profile is available. Using '\"C:\\Users\\tmp\\AppData\\Local\\ASP.NET\\DataProtection-Keys\"' as key repository and Windows DPAPI to encrypt keys at rest.",
            "timestamp": "2022-01-06T14:44:07.545+01:00",
            "properties": "{\"FullName\":\"C:\\\\Users\\\\tmp\\\\AppData\\\\Local\\\\ASP.NET\\\\DataProtection-Keys\",\"EventId\":{\"Id\":63,\"Name\":\"UsingProfileAsKeyRepositoryWithDPAPI\"},\"SourceContext\":\"Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 5,
            "level": "Information",
            "message": "Application started. Press Ctrl+C to shut down.",
            "timestamp": "2022-01-06T14:44:07.886+01:00",
            "properties": "{\"SourceContext\":\"Microsoft.Hosting.Lifetime\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 6,
            "level": "Information",
            "message": "Hosting environment: \"Development\"",
            "timestamp": "2022-01-06T14:44:07.886+01:00",
            "properties": "{\"envName\":\"Development\",\"SourceContext\":\"Microsoft.Hosting.Lifetime\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 7,
            "level": "Information",
            "message": "Content root path: \"C:\\Users\\tmp\\CodeProjects\\study\\serilog-ui\\samples\\SampleWebApp\"",
            "timestamp": "2022-01-06T14:44:07.886+01:00",
            "properties": "{\"contentRoot\":\"C:\\\\Users\\\\tmp\\\\prjs\\\\serilog-ui\\\\samples\\\\SampleWebApp\",\"SourceContext\":\"Microsoft.Hosting.Lifetime\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 8,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/ - -",
            "timestamp": "2022-01-06T14:44:07.915+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/ - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000007c-0004-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 9,
            "level": "Information",
            "message": "Executing endpoint '\"SampleWebApp.Controllers.HomeController.Index (SampleWebApp)\"'",
            "timestamp": "2022-01-06T14:44:08.053+01:00",
            "properties": "{\"EndpointName\":\"SampleWebApp.Controllers.HomeController.Index (SampleWebApp)\",\"EventId\":{\"Name\":\"ExecutingEndpoint\"},\"SourceContext\":\"Microsoft.AspNetCore.Routing.EndpointMiddleware\",\"RequestId\":\"8000007c-0004-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 10,
            "level": "Information",
            "message": "Route matched with \"{action = \\\"Index\\\", controller = \\\"Home\\\", page = \\\"\\\", area = \\\"\\\"}\". Executing controller action with signature \"Microsoft.AspNetCore.Mvc.IActionResult Index()\" on controller \"SampleWebApp.Controllers.HomeController\" (\"SampleWebApp\").",
            "timestamp": "2022-01-06T14:44:08.072+01:00",
            "properties": "{\"RouteData\":\"{action = \\\"Index\\\", controller = \\\"Home\\\", page = \\\"\\\", area = \\\"\\\"}\",\"MethodInfo\":\"Microsoft.AspNetCore.Mvc.IActionResult Index()\",\"Controller\":\"SampleWebApp.Controllers.HomeController\",\"AssemblyName\":\"SampleWebApp\",\"EventId\":{\"Id\":3,\"Name\":\"ControllerActionExecuting\"},\"SourceContext\":\"Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker\",\"ActionId\":\"f6b7b50d-349d-4f3c-8824-9292b9c37465\",\"ActionName\":\"SampleWebApp.Controllers.HomeController.Index (SampleWebApp)\",\"RequestId\":\"8000007c-0004-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 11,
            "level": "Information",
            "message": "Executing ViewResult, running view \"Index\".",
            "timestamp": "2022-01-06T14:44:08.083+01:00",
            "properties": "{\"ViewName\":\"Index\",\"EventId\":{\"Id\":1,\"Name\":\"ViewResultExecuting\"},\"SourceContext\":\"Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor\",\"ActionId\":\"f6b7b50d-349d-4f3c-8824-9292b9c37465\",\"ActionName\":\"SampleWebApp.Controllers.HomeController.Index (SampleWebApp)\",\"RequestId\":\"8000007c-0004-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 12,
            "level": "Information",
            "message": "Executed ViewResult - view \"Index\" executed in 249.3991ms.",
            "timestamp": "2022-01-06T14:44:08.329+01:00",
            "properties": "{\"ViewName\":\"Index\",\"ElapsedMilliseconds\":249.3991,\"EventId\":{\"Id\":4,\"Name\":\"ViewResultExecuted\"},\"SourceContext\":\"Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor\",\"ActionId\":\"f6b7b50d-349d-4f3c-8824-9292b9c37465\",\"ActionName\":\"SampleWebApp.Controllers.HomeController.Index (SampleWebApp)\",\"RequestId\":\"8000007c-0004-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 13,
            "level": "Information",
            "message": "Executed action \"SampleWebApp.Controllers.HomeController.Index (SampleWebApp)\" in 260.7281ms",
            "timestamp": "2022-01-06T14:44:08.334+01:00",
            "properties": "{\"ActionName\":\"SampleWebApp.Controllers.HomeController.Index (SampleWebApp)\",\"ElapsedMilliseconds\":260.7281,\"EventId\":{\"Id\":2,\"Name\":\"ActionExecuted\"},\"SourceContext\":\"Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker\",\"ActionId\":\"f6b7b50d-349d-4f3c-8824-9292b9c37465\",\"RequestId\":\"8000007c-0004-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 14,
            "level": "Information",
            "message": "Executed endpoint '\"SampleWebApp.Controllers.HomeController.Index (SampleWebApp)\"'",
            "timestamp": "2022-01-06T14:44:08.334+01:00",
            "properties": "{\"EndpointName\":\"SampleWebApp.Controllers.HomeController.Index (SampleWebApp)\",\"EventId\":{\"Id\":1,\"Name\":\"ExecutedEndpoint\"},\"SourceContext\":\"Microsoft.AspNetCore.Routing.EndpointMiddleware\",\"RequestId\":\"8000007c-0004-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 15,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/ - - - 200 - text/html;+charset=utf-8 451.6396ms",
            "timestamp": "2022-01-06T14:44:08.365+01:00",
            "properties": "{\"ElapsedMilliseconds\":451.6396,\"StatusCode\":200,\"ContentType\":\"text/html; charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/ - - - 200 - text/html;+charset=utf-8 451.6396ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000007c-0004-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 16,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/css/site.css - -",
            "timestamp": "2022-01-06T14:44:08.379+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/css/site.css\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/css/site.css - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000004e-0003-fe00-b63f-84710c7967bb\",\"RequestPath\":\"/css/site.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 17,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/lib/bootstrap/dist/css/bootstrap.min.css - -",
            "timestamp": "2022-01-06T14:44:08.379+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/lib/bootstrap/dist/css/bootstrap.min.css\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/lib/bootstrap/dist/css/bootstrap.min.css - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000081-0003-ff00-b63f-84710c7967bb\",\"RequestPath\":\"/lib/bootstrap/dist/css/bootstrap.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 18,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/lib/jquery/dist/jquery.min.js - -",
            "timestamp": "2022-01-06T14:44:08.379+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/lib/jquery/dist/jquery.min.js\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/lib/jquery/dist/jquery.min.js - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000007a-0000-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/lib/jquery/dist/jquery.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 19,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/_framework/aspnetcore-browser-refresh.js - -",
            "timestamp": "2022-01-06T14:44:08.379+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/_framework/aspnetcore-browser-refresh.js\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/_framework/aspnetcore-browser-refresh.js - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000000a-0001-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/_framework/aspnetcore-browser-refresh.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 20,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/lib/bootstrap/dist/js/bootstrap.bundle.min.js - -",
            "timestamp": "2022-01-06T14:44:08.379+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/lib/bootstrap/dist/js/bootstrap.bundle.min.js\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/lib/bootstrap/dist/js/bootstrap.bundle.min.js - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000002f-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/lib/bootstrap/dist/js/bootstrap.bundle.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 21,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/js/site.js?v=4q1jwFhaPaZgr8WAUSrux6hAuh0XDg9kPS3xIVq36I0 - -",
            "timestamp": "2022-01-06T14:44:08.38+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/js/site.js\",\"QueryString\":\"?v=4q1jwFhaPaZgr8WAUSrux6hAuh0XDg9kPS3xIVq36I0\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/js/site.js?v=4q1jwFhaPaZgr8WAUSrux6hAuh0XDg9kPS3xIVq36I0 - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000001a-0001-f900-b63f-84710c7967bb\",\"RequestPath\":\"/js/site.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 22,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/_framework/aspnetcore-browser-refresh.js - - - 200 11946 application/javascript;+charset=utf-8 9.2617ms",
            "timestamp": "2022-01-06T14:44:08.389+01:00",
            "properties": "{\"ElapsedMilliseconds\":9.2617,\"StatusCode\":200,\"ContentType\":\"application/javascript; charset=utf-8\",\"ContentLength\":11946,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/_framework/aspnetcore-browser-refresh.js\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/_framework/aspnetcore-browser-refresh.js - - - 200 11946 application/javascript;+charset=utf-8 9.2617ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000000a-0001-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/_framework/aspnetcore-browser-refresh.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 23,
            "level": "Information",
            "message": "Sending file. Request path: '\"/css/site.css\"'. Physical path: '\"C:\\Users\\tmp\\prjs\\serilog-ui\\samples\\SampleWebApp\\wwwroot\\css\\site.css\"'",
            "timestamp": "2022-01-06T14:44:08.399+01:00",
            "properties": "{\"VirtualPath\":\"/css/site.css\",\"PhysicalPath\":\"C:\\\\Users\\\\tmp\\\\prjs\\\\serilog-ui\\\\samples\\\\SampleWebApp\\\\wwwroot\\\\css\\\\site.css\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"8000004e-0003-fe00-b63f-84710c7967bb\",\"RequestPath\":\"/css/site.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 24,
            "level": "Information",
            "message": "Sending file. Request path: '\"/js/site.js\"'. Physical path: '\"C:\\Users\\tmp\\prjs\\serilog-ui\\samples\\SampleWebApp\\wwwroot\\js\\site.js\"'",
            "timestamp": "2022-01-06T14:44:08.4+01:00",
            "properties": "{\"VirtualPath\":\"/js/site.js\",\"PhysicalPath\":\"C:\\\\Users\\\\tmp\\\\prjs\\\\serilog-ui\\\\samples\\\\SampleWebApp\\\\wwwroot\\\\js\\\\site.js\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"8000001a-0001-f900-b63f-84710c7967bb\",\"RequestPath\":\"/js/site.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 25,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/css/site.css - - - 200 1417 text/css 22.0739ms",
            "timestamp": "2022-01-06T14:44:08.401+01:00",
            "properties": "{\"ElapsedMilliseconds\":22.0739,\"StatusCode\":200,\"ContentType\":\"text/css\",\"ContentLength\":1417,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/css/site.css\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/css/site.css - - - 200 1417 text/css 22.0739ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000004e-0003-fe00-b63f-84710c7967bb\",\"RequestPath\":\"/css/site.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 26,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/js/site.js?v=4q1jwFhaPaZgr8WAUSrux6hAuh0XDg9kPS3xIVq36I0 - - - 200 230 application/javascript 21.9948ms",
            "timestamp": "2022-01-06T14:44:08.401+01:00",
            "properties": "{\"ElapsedMilliseconds\":21.9948,\"StatusCode\":200,\"ContentType\":\"application/javascript\",\"ContentLength\":230,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/js/site.js\",\"QueryString\":\"?v=4q1jwFhaPaZgr8WAUSrux6hAuh0XDg9kPS3xIVq36I0\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/js/site.js?v=4q1jwFhaPaZgr8WAUSrux6hAuh0XDg9kPS3xIVq36I0 - - - 200 230 application/javascript 21.9948ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000001a-0001-f900-b63f-84710c7967bb\",\"RequestPath\":\"/js/site.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 27,
            "level": "Information",
            "message": "Sending file. Request path: '\"/lib/jquery/dist/jquery.min.js\"'. Physical path: '\"C:\\Users\\tmp\\prjs\\serilog-ui\\samples\\SampleWebApp\\wwwroot\\lib\\jquery\\dist\\jquery.min.js\"'",
            "timestamp": "2022-01-06T14:44:08.419+01:00",
            "properties": "{\"VirtualPath\":\"/lib/jquery/dist/jquery.min.js\",\"PhysicalPath\":\"C:\\\\Users\\\\tmp\\\\prjs\\\\serilog-ui\\\\samples\\\\SampleWebApp\\\\wwwroot\\\\lib\\\\jquery\\\\dist\\\\jquery.min.js\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"8000007a-0000-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/lib/jquery/dist/jquery.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 28,
            "level": "Information",
            "message": "Sending file. Request path: '\"/lib/bootstrap/dist/css/bootstrap.min.css\"'. Physical path: '\"C:\\Users\\tmp\\prjs\\serilog-ui\\samples\\SampleWebApp\\wwwroot\\lib\\bootstrap\\dist\\css\\bootstrap.min.css\"'",
            "timestamp": "2022-01-06T14:44:08.419+01:00",
            "properties": "{\"VirtualPath\":\"/lib/bootstrap/dist/css/bootstrap.min.css\",\"PhysicalPath\":\"C:\\\\Users\\\\tmp\\\\prjs\\\\serilog-ui\\\\samples\\\\SampleWebApp\\\\wwwroot\\\\lib\\\\bootstrap\\\\dist\\\\css\\\\bootstrap.min.css\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"80000081-0003-ff00-b63f-84710c7967bb\",\"RequestPath\":\"/lib/bootstrap/dist/css/bootstrap.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 29,
            "level": "Information",
            "message": "Sending file. Request path: '\"/lib/bootstrap/dist/js/bootstrap.bundle.min.js\"'. Physical path: '\"C:\\Users\\tmp\\prjs\\serilog-ui\\samples\\SampleWebApp\\wwwroot\\lib\\bootstrap\\dist\\js\\bootstrap.bundle.min.js\"'",
            "timestamp": "2022-01-06T14:44:08.42+01:00",
            "properties": "{\"VirtualPath\":\"/lib/bootstrap/dist/js/bootstrap.bundle.min.js\",\"PhysicalPath\":\"C:\\\\Users\\\\tmp\\\\prjs\\\\serilog-ui\\\\samples\\\\SampleWebApp\\\\wwwroot\\\\lib\\\\bootstrap\\\\dist\\\\js\\\\bootstrap.bundle.min.js\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"8000002f-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/lib/bootstrap/dist/js/bootstrap.bundle.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 30,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/lib/jquery/dist/jquery.min.js - - - 200 89478 application/javascript 46.3711ms",
            "timestamp": "2022-01-06T14:44:08.425+01:00",
            "properties": "{\"ElapsedMilliseconds\":46.3711,\"StatusCode\":200,\"ContentType\":\"application/javascript\",\"ContentLength\":89478,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/lib/jquery/dist/jquery.min.js\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/lib/jquery/dist/jquery.min.js - - - 200 89478 application/javascript 46.3711ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000007a-0000-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/lib/jquery/dist/jquery.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 31,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/lib/bootstrap/dist/js/bootstrap.bundle.min.js - - - 200 78641 application/javascript 46.0291ms",
            "timestamp": "2022-01-06T14:44:08.425+01:00",
            "properties": "{\"ElapsedMilliseconds\":46.0291,\"StatusCode\":200,\"ContentType\":\"application/javascript\",\"ContentLength\":78641,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/lib/bootstrap/dist/js/bootstrap.bundle.min.js\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/lib/bootstrap/dist/js/bootstrap.bundle.min.js - - - 200 78641 application/javascript 46.0291ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000002f-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/lib/bootstrap/dist/js/bootstrap.bundle.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 32,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/lib/bootstrap/dist/css/bootstrap.min.css - - - 200 155764 text/css 46.3724ms",
            "timestamp": "2022-01-06T14:44:08.425+01:00",
            "properties": "{\"ElapsedMilliseconds\":46.3724,\"StatusCode\":200,\"ContentType\":\"text/css\",\"ContentLength\":155764,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/lib/bootstrap/dist/css/bootstrap.min.css\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/lib/bootstrap/dist/css/bootstrap.min.css - - - 200 155764 text/css 46.3724ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000081-0003-ff00-b63f-84710c7967bb\",\"RequestPath\":\"/lib/bootstrap/dist/css/bootstrap.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 33,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui - -",
            "timestamp": "2022-01-06T14:44:11.082+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"800000a4-0006-ff00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 34,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui - - - 301 - - 5.6482ms",
            "timestamp": "2022-01-06T14:44:11.087+01:00",
            "properties": "{\"ElapsedMilliseconds\":5.6482,\"StatusCode\":301,\"ContentType\":null,\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui - - - 301 - - 5.6482ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"800000a4-0006-ff00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 35,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/index.html - -",
            "timestamp": "2022-01-06T14:44:11.09+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/index.html\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/index.html - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000054-0002-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/index.html\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 36,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/index.html - - - 200 - text/html;charset=utf-8 102.5136ms",
            "timestamp": "2022-01-06T14:44:11.192+01:00",
            "properties": "{\"ElapsedMilliseconds\":102.5136,\"StatusCode\":200,\"ContentType\":\"text/html;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/index.html\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/index.html - - - 200 - text/html;charset=utf-8 102.5136ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000054-0002-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/index.html\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 37,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/font-awesome.min.css - -",
            "timestamp": "2022-01-06T14:44:11.201+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/font-awesome.min.css\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/font-awesome.min.css - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000056-0002-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/font-awesome.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 38,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/main.min.css - -",
            "timestamp": "2022-01-06T14:44:11.202+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/main.min.css\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/main.min.css - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000007e-0004-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/main.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 39,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/jquery-3.5.1.min.js - -",
            "timestamp": "2022-01-06T14:44:11.203+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/jquery-3.5.1.min.js\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/jquery-3.5.1.min.js - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000006b-0004-ff00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/jquery-3.5.1.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 40,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.min.css - -",
            "timestamp": "2022-01-06T14:44:11.203+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/bootstrap.min.css\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.min.css - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000031-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/bootstrap.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 41,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/netstack.min.js - -",
            "timestamp": "2022-01-06T14:44:11.203+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/netstack.min.js\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/netstack.min.js - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000081-0004-fe00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/netstack.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 42,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/_framework/aspnetcore-browser-refresh.js - -",
            "timestamp": "2022-01-06T14:44:11.203+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/_framework/aspnetcore-browser-refresh.js\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/_framework/aspnetcore-browser-refresh.js - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000015-0004-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/_framework/aspnetcore-browser-refresh.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 43,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/main.js - -",
            "timestamp": "2022-01-06T14:44:11.204+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/main.js\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/main.js - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000016-0004-fb00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/main.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 44,
            "level": "Information",
            "message": "Sending file. Request path: '\"/font-awesome.min.css\"'. Physical path: '\"N/A\"'",
            "timestamp": "2022-01-06T14:44:11.204+01:00",
            "properties": "{\"VirtualPath\":\"/font-awesome.min.css\",\"PhysicalPath\":\"N/A\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"80000056-0002-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/font-awesome.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 45,
            "level": "Information",
            "message": "Sending file. Request path: '\"/netstack.min.js\"'. Physical path: '\"N/A\"'",
            "timestamp": "2022-01-06T14:44:11.205+01:00",
            "properties": "{\"VirtualPath\":\"/netstack.min.js\",\"PhysicalPath\":\"N/A\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"80000081-0004-fe00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/netstack.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 46,
            "level": "Information",
            "message": "Sending file. Request path: '\"/main.min.css\"'. Physical path: '\"N/A\"'",
            "timestamp": "2022-01-06T14:44:11.205+01:00",
            "properties": "{\"VirtualPath\":\"/main.min.css\",\"PhysicalPath\":\"N/A\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"8000007e-0004-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/main.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 47,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/main.min.css - - - 200 5318 text/css 4.9268ms",
            "timestamp": "2022-01-06T14:44:11.207+01:00",
            "properties": "{\"ElapsedMilliseconds\":4.9268,\"StatusCode\":200,\"ContentType\":\"text/css\",\"ContentLength\":5318,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/main.min.css\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/main.min.css - - - 200 5318 text/css 4.9268ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000007e-0004-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/main.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 48,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/netstack.min.js - - - 200 2731 application/javascript 5.3027ms",
            "timestamp": "2022-01-06T14:44:11.208+01:00",
            "properties": "{\"ElapsedMilliseconds\":5.3027,\"StatusCode\":200,\"ContentType\":\"application/javascript\",\"ContentLength\":2731,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/netstack.min.js\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/netstack.min.js - - - 200 2731 application/javascript 5.3027ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000081-0004-fe00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/netstack.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 49,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/font-awesome.min.css - - - 200 30951 text/css 10.3452ms",
            "timestamp": "2022-01-06T14:44:11.211+01:00",
            "properties": "{\"ElapsedMilliseconds\":10.3452,\"StatusCode\":200,\"ContentType\":\"text/css\",\"ContentLength\":30951,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/font-awesome.min.css\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/font-awesome.min.css - - - 200 30951 text/css 10.3452ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000056-0002-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/font-awesome.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 50,
            "level": "Information",
            "message": "Sending file. Request path: '\"/main.js\"'. Physical path: '\"N/A\"'",
            "timestamp": "2022-01-06T14:44:11.211+01:00",
            "properties": "{\"VirtualPath\":\"/main.js\",\"PhysicalPath\":\"N/A\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"80000016-0004-fb00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/main.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 51,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.bundle.min.js - -",
            "timestamp": "2022-01-06T14:44:11.212+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/bootstrap.bundle.min.js\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.bundle.min.js - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000004c-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/bootstrap.bundle.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 52,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/main.js - - - 200 10899 application/javascript 10.3902ms",
            "timestamp": "2022-01-06T14:44:11.219+01:00",
            "properties": "{\"ElapsedMilliseconds\":10.3902,\"StatusCode\":200,\"ContentType\":\"application/javascript\",\"ContentLength\":10899,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/main.js\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/main.js - - - 200 10899 application/javascript 10.3902ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000016-0004-fb00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/main.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 53,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/_framework/aspnetcore-browser-refresh.js - - - 200 11946 application/javascript;+charset=utf-8 15.7003ms",
            "timestamp": "2022-01-06T14:44:11.219+01:00",
            "properties": "{\"ElapsedMilliseconds\":15.7003,\"StatusCode\":200,\"ContentType\":\"application/javascript; charset=utf-8\",\"ContentLength\":11946,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/_framework/aspnetcore-browser-refresh.js\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/_framework/aspnetcore-browser-refresh.js - - - 200 11946 application/javascript;+charset=utf-8 15.7003ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000015-0004-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/_framework/aspnetcore-browser-refresh.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 54,
            "level": "Information",
            "message": "Sending file. Request path: '\"/jquery-3.5.1.min.js\"'. Physical path: '\"N/A\"'",
            "timestamp": "2022-01-06T14:44:11.221+01:00",
            "properties": "{\"VirtualPath\":\"/jquery-3.5.1.min.js\",\"PhysicalPath\":\"N/A\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"8000006b-0004-ff00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/jquery-3.5.1.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 55,
            "level": "Information",
            "message": "Sending file. Request path: '\"/bootstrap.min.css\"'. Physical path: '\"N/A\"'",
            "timestamp": "2022-01-06T14:44:11.222+01:00",
            "properties": "{\"VirtualPath\":\"/bootstrap.min.css\",\"PhysicalPath\":\"N/A\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"80000031-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/bootstrap.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 56,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/jquery-3.5.1.min.js - - - 200 89479 application/javascript 20.0107ms",
            "timestamp": "2022-01-06T14:44:11.223+01:00",
            "properties": "{\"ElapsedMilliseconds\":20.0107,\"StatusCode\":200,\"ContentType\":\"application/javascript\",\"ContentLength\":89479,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/jquery-3.5.1.min.js\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/jquery-3.5.1.min.js - - - 200 89479 application/javascript 20.0107ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000006b-0004-ff00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/jquery-3.5.1.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 57,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.min.css - - - 200 160401 text/css 20.4653ms",
            "timestamp": "2022-01-06T14:44:11.223+01:00",
            "properties": "{\"ElapsedMilliseconds\":20.4653,\"StatusCode\":200,\"ContentType\":\"text/css\",\"ContentLength\":160401,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/bootstrap.min.css\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.min.css - - - 200 160401 text/css 20.4653ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000031-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/bootstrap.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 58,
            "level": "Information",
            "message": "Sending file. Request path: '\"/bootstrap.bundle.min.js\"'. Physical path: '\"N/A\"'",
            "timestamp": "2022-01-06T14:44:11.223+01:00",
            "properties": "{\"VirtualPath\":\"/bootstrap.bundle.min.js\",\"PhysicalPath\":\"N/A\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"8000004c-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/bootstrap.bundle.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 59,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.bundle.min.js - - - 200 84161 application/javascript 20.5787ms",
            "timestamp": "2022-01-06T14:44:11.225+01:00",
            "properties": "{\"ElapsedMilliseconds\":20.5787,\"StatusCode\":200,\"ContentType\":\"application/javascript\",\"ContentLength\":84161,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/bootstrap.bundle.min.js\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.bundle.min.js - - - 200 84161 application/javascript 20.5787ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000004c-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/bootstrap.bundle.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 60,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/fontawesome-webfont.woff2?v=4.7.0 - -",
            "timestamp": "2022-01-06T14:44:11.244+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/fontawesome-webfont.woff2\",\"QueryString\":\"?v=4.7.0\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/fontawesome-webfont.woff2?v=4.7.0 - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000007c-0000-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/fontawesome-webfont.woff2\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 61,
            "level": "Information",
            "message": "Sending file. Request path: '\"/fontawesome-webfont.woff2\"'. Physical path: '\"N/A\"'",
            "timestamp": "2022-01-06T14:44:11.246+01:00",
            "properties": "{\"VirtualPath\":\"/fontawesome-webfont.woff2\",\"PhysicalPath\":\"N/A\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"8000007c-0000-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/fontawesome-webfont.woff2\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 62,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/fontawesome-webfont.woff2?v=4.7.0 - - - 200 77160 font/woff2 3.2378ms",
            "timestamp": "2022-01-06T14:44:11.247+01:00",
            "properties": "{\"ElapsedMilliseconds\":3.2378,\"StatusCode\":200,\"ContentType\":\"font/woff2\",\"ContentLength\":77160,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/fontawesome-webfont.woff2\",\"QueryString\":\"?v=4.7.0\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/fontawesome-webfont.woff2?v=4.7.0 - - - 200 77160 font/woff2 3.2378ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000007c-0000-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/fontawesome-webfont.woff2\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 63,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:11.344+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000089-0002-fe00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 64,
            "level": "Information",
            "message": "Request starting HTTP/1.1 GET https://localhost:44377/serilog-ui/bootstrap.bundle.min.js.map - -",
            "timestamp": "2022-01-06T14:44:11.46+01:00",
            "properties": "{\"Protocol\":\"HTTP/1.1\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/bootstrap.bundle.min.js.map\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/1.1 GET https://localhost:44377/serilog-ui/bootstrap.bundle.min.js.map - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000058-0002-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/bootstrap.bundle.min.js.map\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 65,
            "level": "Information",
            "message": "Request finished HTTP/1.1 GET https://localhost:44377/serilog-ui/bootstrap.bundle.min.js.map - - - 404 - - 1.3235ms",
            "timestamp": "2022-01-06T14:44:11.462+01:00",
            "properties": "{\"ElapsedMilliseconds\":1.3235,\"StatusCode\":404,\"ContentType\":null,\"ContentLength\":null,\"Protocol\":\"HTTP/1.1\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/bootstrap.bundle.min.js.map\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/1.1 GET https://localhost:44377/serilog-ui/bootstrap.bundle.min.js.map - - - 404 - - 1.3235ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000058-0002-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/bootstrap.bundle.min.js.map\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 66,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 216.7630ms",
            "timestamp": "2022-01-06T14:44:11.561+01:00",
            "properties": "{\"ElapsedMilliseconds\":216.763,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 216.7630ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000089-0002-fe00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 67,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=2&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:16.486+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=2&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=2&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000035-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 68,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=2&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 10.6550ms",
            "timestamp": "2022-01-06T14:44:16.497+01:00",
            "properties": "{\"ElapsedMilliseconds\":10.655,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=2&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=2&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 10.6550ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000035-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 69,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=1&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:19.495+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=1&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=1&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000004b-0000-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 70,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=1&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.9938ms",
            "timestamp": "2022-01-06T14:44:19.503+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.9938,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=1&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=1&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.9938ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000004b-0000-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 71,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=7&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:27.825+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=7&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=7&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000008d-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 72,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=7&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 8.8866ms",
            "timestamp": "2022-01-06T14:44:27.834+01:00",
            "properties": "{\"ElapsedMilliseconds\":8.8866,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=7&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=7&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 8.8866ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000008d-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 73,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=7&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:30.847+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=7&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=7&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000004e-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 74,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=7&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.5379ms",
            "timestamp": "2022-01-06T14:44:30.855+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.5379,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=7&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=7&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.5379ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000004e-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 75,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=8&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:31.576+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=8&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=8&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000018-0005-fb00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 76,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=8&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 6.6391ms",
            "timestamp": "2022-01-06T14:44:31.583+01:00",
            "properties": "{\"ElapsedMilliseconds\":6.6391,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=8&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=8&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 6.6391ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000018-0005-fb00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 77,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=7&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:33.353+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=7&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=7&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000050-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 78,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=7&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 10.5100ms",
            "timestamp": "2022-01-06T14:44:33.364+01:00",
            "properties": "{\"ElapsedMilliseconds\":10.51,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=7&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=7&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 10.5100ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000050-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 79,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=10&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:36.341+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=10&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=10&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000004d-0000-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 80,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=10&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.2511ms",
            "timestamp": "2022-01-06T14:44:36.348+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.2511,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=10&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=10&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.2511ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000004d-0000-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 81,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=13&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:38.317+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=13&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=13&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000037-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 82,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=13&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.7431ms",
            "timestamp": "2022-01-06T14:44:38.325+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.7431,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=13&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=13&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.7431ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000037-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 83,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=12&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:39.819+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=12&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=12&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000039-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 84,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=12&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 6.9531ms",
            "timestamp": "2022-01-06T14:44:39.826+01:00",
            "properties": "{\"ElapsedMilliseconds\":6.9531,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=12&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=12&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 6.9531ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000039-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 85,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=11&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:40.278+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=11&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=11&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000008f-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 86,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=11&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 6.2920ms",
            "timestamp": "2022-01-06T14:44:40.285+01:00",
            "properties": "{\"ElapsedMilliseconds\":6.292,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=11&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=11&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 6.2920ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000008f-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 87,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=10&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:40.888+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=10&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=10&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000003b-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 88,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=10&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 6.6214ms",
            "timestamp": "2022-01-06T14:44:40.895+01:00",
            "properties": "{\"ElapsedMilliseconds\":6.6214,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=10&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=10&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 6.6214ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000003b-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 89,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=11&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:42.637+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=11&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=11&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000001a-0005-fb00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 90,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=11&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.1691ms",
            "timestamp": "2022-01-06T14:44:42.644+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.1691,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=11&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=11&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.1691ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000001a-0005-fb00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 91,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=1&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:44.7+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=1&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=1&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000003d-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 92,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=1&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.5149ms",
            "timestamp": "2022-01-06T14:44:44.708+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.5149,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=1&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=1&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.5149ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000003d-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 93,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=2&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:46.928+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=2&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=2&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000052-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 94,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=2&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.8682ms",
            "timestamp": "2022-01-06T14:44:46.936+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.8682,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=2&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=2&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.8682ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000052-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 95,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=3&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:48.125+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=3&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=3&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000091-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 96,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=3&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.7574ms",
            "timestamp": "2022-01-06T14:44:48.133+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.7574,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=3&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=3&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.7574ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000091-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 97,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=7&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:51.309+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=7&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=7&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000050-0003-fe00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 98,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=7&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 9.4299ms",
            "timestamp": "2022-01-06T14:44:51.318+01:00",
            "properties": "{\"ElapsedMilliseconds\":9.4299,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=7&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=7&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 9.4299ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000050-0003-fe00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 99,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=8&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:52.335+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=8&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=8&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000093-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 100,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=8&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 8.4315ms",
            "timestamp": "2022-01-06T14:44:52.343+01:00",
            "properties": "{\"ElapsedMilliseconds\":8.4315,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=8&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=8&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 8.4315ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000093-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 101,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=9&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:53.441+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=9&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=9&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000095-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 102,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=9&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.0220ms",
            "timestamp": "2022-01-06T14:44:53.448+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.022,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=9&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=9&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.0220ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000095-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 103,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=10&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:54.158+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=10&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=10&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000097-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 104,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=10&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.0353ms",
            "timestamp": "2022-01-06T14:44:54.165+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.0353,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=10&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=10&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.0353ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000097-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 105,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=8&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:54.732+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=8&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=8&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000099-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 106,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=8&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 5.8997ms",
            "timestamp": "2022-01-06T14:44:54.738+01:00",
            "properties": "{\"ElapsedMilliseconds\":5.8997,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=8&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=8&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 5.8997ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000099-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 107,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=9&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:55.603+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=9&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=9&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000054-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 108,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=9&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.9883ms",
            "timestamp": "2022-01-06T14:44:55.611+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.9883,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=9&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=9&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.9883ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000054-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 109,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=10&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:56.378+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=10&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=10&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000025-0002-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 110,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=10&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.1620ms",
            "timestamp": "2022-01-06T14:44:56.385+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.162,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=10&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=10&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.1620ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000025-0002-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 111,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=11&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:57.102+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=11&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=11&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000056-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 112,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=11&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 6.9915ms",
            "timestamp": "2022-01-06T14:44:57.109+01:00",
            "properties": "{\"ElapsedMilliseconds\":6.9915,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=11&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=11&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 6.9915ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000056-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 113,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=9&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:57.817+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=9&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=9&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000027-0002-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 114,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=9&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 13.7293ms",
            "timestamp": "2022-01-06T14:44:57.831+01:00",
            "properties": "{\"ElapsedMilliseconds\":13.7293,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=9&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=9&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 13.7293ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000027-0002-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 115,
            "level": "Critical",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=10&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:44:59.366+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=10&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=10&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000003f-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 116,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=10&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.3060ms",
            "timestamp": "2022-01-06T14:44:59.373+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.306,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=10&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=10&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.3060ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000003f-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 117,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=11&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:45:00.746+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=11&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=11&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000009b-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 118,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=11&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 6.4165ms",
            "timestamp": "2022-01-06T14:45:00.753+01:00",
            "properties": "{\"ElapsedMilliseconds\":6.4165,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=11&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=11&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 6.4165ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000009b-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 119,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=12&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:45:01.652+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=12&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=12&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000052-0003-fe00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 120,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=12&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.4357ms",
            "timestamp": "2022-01-06T14:45:01.66+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.4357,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=12&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=12&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.4357ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000052-0003-fe00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 121,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=12&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:45:04.547+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=12&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=12&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000009d-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 122,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=12&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 13.0904ms",
            "timestamp": "2022-01-06T14:45:04.56+01:00",
            "properties": "{\"ElapsedMilliseconds\":13.0904,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=12&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=12&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 13.0904ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000009d-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 123,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=11&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:45:09.05+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=11&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=11&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000041-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 124,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=11&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 6.8765ms",
            "timestamp": "2022-01-06T14:45:09.057+01:00",
            "properties": "{\"ElapsedMilliseconds\":6.8765,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=11&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=11&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 6.8765ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000041-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 125,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=15&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:45:10.861+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=15&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=15&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000043-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 126,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=15&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 6.9191ms",
            "timestamp": "2022-01-06T14:45:10.867+01:00",
            "properties": "{\"ElapsedMilliseconds\":6.9191,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=15&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=15&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 6.9191ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000043-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 127,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/index.html - -",
            "timestamp": "2022-01-06T14:45:13.328+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/index.html\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/index.html - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000045-0007-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/index.html\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 128,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/index.html - - - 200 - text/html;charset=utf-8 11.3130ms",
            "timestamp": "2022-01-06T14:45:13.339+01:00",
            "properties": "{\"ElapsedMilliseconds\":11.313,\"StatusCode\":200,\"ContentType\":\"text/html;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/index.html\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/index.html - - - 200 - text/html;charset=utf-8 11.3130ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000045-0007-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/index.html\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 129,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/font-awesome.min.css - -",
            "timestamp": "2022-01-06T14:45:13.356+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/font-awesome.min.css\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/font-awesome.min.css - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000058-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/font-awesome.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 130,
            "level": "Information",
            "message": "Sending file. Request path: '\"/font-awesome.min.css\"'. Physical path: '\"N/A\"'",
            "timestamp": "2022-01-06T14:45:13.357+01:00",
            "properties": "{\"VirtualPath\":\"/font-awesome.min.css\",\"PhysicalPath\":\"N/A\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"80000058-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/font-awesome.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 131,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.min.css - -",
            "timestamp": "2022-01-06T14:45:13.357+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/bootstrap.min.css\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.min.css - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000006d-0004-ff00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/bootstrap.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 132,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/font-awesome.min.css - - - 200 30951 text/css 2.2222ms",
            "timestamp": "2022-01-06T14:45:13.358+01:00",
            "properties": "{\"ElapsedMilliseconds\":2.2222,\"StatusCode\":200,\"ContentType\":\"text/css\",\"ContentLength\":30951,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/font-awesome.min.css\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/font-awesome.min.css - - - 200 30951 text/css 2.2222ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000058-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/font-awesome.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 133,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/main.min.css - -",
            "timestamp": "2022-01-06T14:45:13.361+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/main.min.css\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/main.min.css - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000001c-0005-fb00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/main.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 134,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/jquery-3.5.1.min.js - -",
            "timestamp": "2022-01-06T14:45:13.362+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/jquery-3.5.1.min.js\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/jquery-3.5.1.min.js - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000018-0005-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/jquery-3.5.1.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 135,
            "level": "Information",
            "message": "Sending file. Request path: '\"/main.min.css\"'. Physical path: '\"N/A\"'",
            "timestamp": "2022-01-06T14:45:13.365+01:00",
            "properties": "{\"VirtualPath\":\"/main.min.css\",\"PhysicalPath\":\"N/A\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"8000001c-0005-fb00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/main.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 136,
            "level": "Critical",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/netstack.min.js - -",
            "timestamp": "2022-01-06T14:45:13.366+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/netstack.min.js\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/netstack.min.js - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000036-0007-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/netstack.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 137,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.bundle.min.js - -",
            "timestamp": "2022-01-06T14:45:13.365+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/bootstrap.bundle.min.js\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.bundle.min.js - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000047-0007-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/bootstrap.bundle.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 138,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/_framework/aspnetcore-browser-refresh.js - -",
            "timestamp": "2022-01-06T14:45:13.368+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/_framework/aspnetcore-browser-refresh.js\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/_framework/aspnetcore-browser-refresh.js - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000081-0006-fe00-b63f-84710c7967bb\",\"RequestPath\":\"/_framework/aspnetcore-browser-refresh.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 139,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/main.js - -",
            "timestamp": "2022-01-06T14:45:13.368+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/main.js\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/main.js - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"800000a6-0006-ff00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/main.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 140,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/main.min.css - - - 200 5318 text/css 7.2131ms",
            "timestamp": "2022-01-06T14:45:13.369+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.2131,\"StatusCode\":200,\"ContentType\":\"text/css\",\"ContentLength\":5318,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/main.min.css\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/main.min.css - - - 200 5318 text/css 7.2131ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000001c-0005-fb00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/main.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 141,
            "level": "Information",
            "message": "Sending file. Request path: '\"/netstack.min.js\"'. Physical path: '\"N/A\"'",
            "timestamp": "2022-01-06T14:45:13.37+01:00",
            "properties": "{\"VirtualPath\":\"/netstack.min.js\",\"PhysicalPath\":\"N/A\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"80000036-0007-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/netstack.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 142,
            "level": "Information",
            "message": "Sending file. Request path: '\"/main.js\"'. Physical path: '\"N/A\"'",
            "timestamp": "2022-01-06T14:45:13.373+01:00",
            "properties": "{\"VirtualPath\":\"/main.js\",\"PhysicalPath\":\"N/A\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"800000a6-0006-ff00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/main.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 143,
            "level": "Information",
            "message": "Sending file. Request path: '\"/bootstrap.min.css\"'. Physical path: '\"N/A\"'",
            "timestamp": "2022-01-06T14:45:13.372+01:00",
            "properties": "{\"VirtualPath\":\"/bootstrap.min.css\",\"PhysicalPath\":\"N/A\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"8000006d-0004-ff00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/bootstrap.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 144,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/netstack.min.js - - - 200 2731 application/javascript 5.7558ms",
            "timestamp": "2022-01-06T14:45:13.373+01:00",
            "properties": "{\"ElapsedMilliseconds\":5.7558,\"StatusCode\":200,\"ContentType\":\"application/javascript\",\"ContentLength\":2731,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/netstack.min.js\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/netstack.min.js - - - 200 2731 application/javascript 5.7558ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000036-0007-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/netstack.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 145,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/_framework/aspnetcore-browser-refresh.js - - - 200 11946 application/javascript;+charset=utf-8 15.2509ms",
            "timestamp": "2022-01-06T14:45:13.383+01:00",
            "properties": "{\"ElapsedMilliseconds\":15.2509,\"StatusCode\":200,\"ContentType\":\"application/javascript; charset=utf-8\",\"ContentLength\":11946,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/_framework/aspnetcore-browser-refresh.js\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/_framework/aspnetcore-browser-refresh.js - - - 200 11946 application/javascript;+charset=utf-8 15.2509ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000081-0006-fe00-b63f-84710c7967bb\",\"RequestPath\":\"/_framework/aspnetcore-browser-refresh.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 146,
            "level": "Information",
            "message": "Sending file. Request path: '\"/bootstrap.bundle.min.js\"'. Physical path: '\"N/A\"'",
            "timestamp": "2022-01-06T14:45:13.385+01:00",
            "properties": "{\"VirtualPath\":\"/bootstrap.bundle.min.js\",\"PhysicalPath\":\"N/A\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"80000047-0007-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/bootstrap.bundle.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 147,
            "level": "Information",
            "message": "Sending file. Request path: '\"/jquery-3.5.1.min.js\"'. Physical path: '\"N/A\"'",
            "timestamp": "2022-01-06T14:45:13.385+01:00",
            "properties": "{\"VirtualPath\":\"/jquery-3.5.1.min.js\",\"PhysicalPath\":\"N/A\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"80000018-0005-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/jquery-3.5.1.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 148,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/main.js - - - 200 10899 application/javascript 22.1641ms",
            "timestamp": "2022-01-06T14:45:13.39+01:00",
            "properties": "{\"ElapsedMilliseconds\":22.1641,\"StatusCode\":200,\"ContentType\":\"application/javascript\",\"ContentLength\":10899,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/main.js\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/main.js - - - 200 10899 application/javascript 22.1641ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"800000a6-0006-ff00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/main.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 149,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.min.css - - - 200 160401 text/css 33.2699ms",
            "timestamp": "2022-01-06T14:45:13.391+01:00",
            "properties": "{\"ElapsedMilliseconds\":33.2699,\"StatusCode\":200,\"ContentType\":\"text/css\",\"ContentLength\":160401,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/bootstrap.min.css\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.min.css - - - 200 160401 text/css 33.2699ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000006d-0004-ff00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/bootstrap.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 150,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/jquery-3.5.1.min.js - - - 200 89479 application/javascript 38.0915ms",
            "timestamp": "2022-01-06T14:45:13.401+01:00",
            "properties": "{\"ElapsedMilliseconds\":38.0915,\"StatusCode\":200,\"ContentType\":\"application/javascript\",\"ContentLength\":89479,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/jquery-3.5.1.min.js\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/jquery-3.5.1.min.js - - - 200 89479 application/javascript 38.0915ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000018-0005-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/jquery-3.5.1.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 151,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/fontawesome-webfont.woff2?v=4.7.0 - -",
            "timestamp": "2022-01-06T14:45:13.41+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/fontawesome-webfont.woff2\",\"QueryString\":\"?v=4.7.0\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/fontawesome-webfont.woff2?v=4.7.0 - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000045-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/fontawesome-webfont.woff2\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 152,
            "level": "Information",
            "message": "Sending file. Request path: '\"/fontawesome-webfont.woff2\"'. Physical path: '\"N/A\"'",
            "timestamp": "2022-01-06T14:45:13.419+01:00",
            "properties": "{\"VirtualPath\":\"/fontawesome-webfont.woff2\",\"PhysicalPath\":\"N/A\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"80000045-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/fontawesome-webfont.woff2\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 153,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.bundle.min.js - - - 200 84161 application/javascript 53.9913ms",
            "timestamp": "2022-01-06T14:45:13.42+01:00",
            "properties": "{\"ElapsedMilliseconds\":53.9913,\"StatusCode\":200,\"ContentType\":\"application/javascript\",\"ContentLength\":84161,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/bootstrap.bundle.min.js\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.bundle.min.js - - - 200 84161 application/javascript 53.9913ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000047-0007-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/bootstrap.bundle.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 154,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/fontawesome-webfont.woff2?v=4.7.0 - - - 200 77160 font/woff2 9.6979ms",
            "timestamp": "2022-01-06T14:45:13.42+01:00",
            "properties": "{\"ElapsedMilliseconds\":9.6979,\"StatusCode\":200,\"ContentType\":\"font/woff2\",\"ContentLength\":77160,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/fontawesome-webfont.woff2\",\"QueryString\":\"?v=4.7.0\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/fontawesome-webfont.woff2?v=4.7.0 - - - 200 77160 font/woff2 9.6979ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000045-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/fontawesome-webfont.woff2\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 155,
            "level": "Information",
            "message": "Request starting HTTP/1.1 GET https://localhost:44377/serilog-ui/bootstrap.bundle.min.js.map - -",
            "timestamp": "2022-01-06T14:45:13.444+01:00",
            "properties": "{\"Protocol\":\"HTTP/1.1\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/bootstrap.bundle.min.js.map\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/1.1 GET https://localhost:44377/serilog-ui/bootstrap.bundle.min.js.map - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000049-0007-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/bootstrap.bundle.min.js.map\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 156,
            "level": "Information",
            "message": "Request finished HTTP/1.1 GET https://localhost:44377/serilog-ui/bootstrap.bundle.min.js.map - - - 404 - - 1.5978ms",
            "timestamp": "2022-01-06T14:45:13.446+01:00",
            "properties": "{\"ElapsedMilliseconds\":1.5978,\"StatusCode\":404,\"ContentType\":null,\"ContentLength\":null,\"Protocol\":\"HTTP/1.1\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/bootstrap.bundle.min.js.map\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/1.1 GET https://localhost:44377/serilog-ui/bootstrap.bundle.min.js.map - - - 404 - - 1.5978ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000049-0007-fd00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/bootstrap.bundle.min.js.map\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 157,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:45:13.477+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000089-0003-ff00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 158,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 11.6366ms",
            "timestamp": "2022-01-06T14:45:13.488+01:00",
            "properties": "{\"ElapsedMilliseconds\":11.6366,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 11.6366ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000089-0003-ff00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 159,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/favicon.ico - -",
            "timestamp": "2022-01-06T14:45:13.492+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/favicon.ico\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/favicon.ico - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000006f-0004-ff00-b63f-84710c7967bb\",\"RequestPath\":\"/favicon.ico\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 160,
            "level": "Information",
            "message": "Sending file. Request path: '\"/favicon.ico\"'. Physical path: '\"C:\\Users\\tmp\\prjs\\serilog-ui\\samples\\SampleWebApp\\wwwroot\\favicon.ico\"'",
            "timestamp": "2022-01-06T14:45:13.499+01:00",
            "properties": "{\"VirtualPath\":\"/favicon.ico\",\"PhysicalPath\":\"C:\\\\Users\\\\tmp\\\\prjs\\\\serilog-ui\\\\samples\\\\SampleWebApp\\\\wwwroot\\\\favicon.ico\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"8000006f-0004-ff00-b63f-84710c7967bb\",\"RequestPath\":\"/favicon.ico\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 161,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/favicon.ico - - - 200 5430 image/x-icon 7.9526ms",
            "timestamp": "2022-01-06T14:45:13.5+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.9526,\"StatusCode\":200,\"ContentType\":\"image/x-icon\",\"ContentLength\":5430,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/favicon.ico\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/favicon.ico - - - 200 5430 image/x-icon 7.9526ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000006f-0004-ff00-b63f-84710c7967bb\",\"RequestPath\":\"/favicon.ico\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 162,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=13&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:45:15.749+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=13&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=13&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000047-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 163,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=13&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 8.3337ms",
            "timestamp": "2022-01-06T14:45:15.757+01:00",
            "properties": "{\"ElapsedMilliseconds\":8.3337,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=13&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=13&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 8.3337ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000047-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 164,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=14&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:45:19.737+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=14&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=14&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"800000a3-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 165,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=14&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 8.3877ms",
            "timestamp": "2022-01-06T14:45:19.745+01:00",
            "properties": "{\"ElapsedMilliseconds\":8.3877,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=14&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=14&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 8.3877ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"800000a3-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 166,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=15&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:45:21.947+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=15&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=15&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000005a-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 167,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=15&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.6452ms",
            "timestamp": "2022-01-06T14:45:21.954+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.6452,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=15&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=15&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.6452ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000005a-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 168,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=16&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:45:22.806+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=16&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=16&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000054-0003-fe00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 169,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=16&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 8.7601ms",
            "timestamp": "2022-01-06T14:45:22.815+01:00",
            "properties": "{\"ElapsedMilliseconds\":8.7601,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=16&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=16&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 8.7601ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000054-0003-fe00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 170,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=17&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:45:24.123+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=17&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=17&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000056-0003-fe00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 171,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=17&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.6044ms",
            "timestamp": "2022-01-06T14:45:24.13+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.6044,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=17&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=17&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.6044ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000056-0003-fe00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 172,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=17&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:45:26.208+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=17&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=17&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000001a-0005-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 173,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=17&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.8450ms",
            "timestamp": "2022-01-06T14:45:26.215+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.845,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=17&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=17&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.8450ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000001a-0005-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 174,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=18&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:45:28.276+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=18&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=18&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"800000a5-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 175,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=18&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 12.0078ms",
            "timestamp": "2022-01-06T14:45:28.288+01:00",
            "properties": "{\"ElapsedMilliseconds\":12.0078,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=18&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=18&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 12.0078ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"800000a5-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 176,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=18&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:45:29.906+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=18&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=18&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000001c-0005-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 177,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=18&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.6661ms",
            "timestamp": "2022-01-06T14:45:29.914+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.6661,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=18&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=18&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.6661ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000001c-0005-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 178,
            "level": "Critical",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=18&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:45:31.835+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=18&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=18&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"800000a7-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 179,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=18&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.3338ms",
            "timestamp": "2022-01-06T14:45:31.842+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.3338,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=18&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=18&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.3338ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"800000a7-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 180,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=18&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:45:33.344+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=18&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=18&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000029-0002-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 181,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=18&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.4673ms",
            "timestamp": "2022-01-06T14:45:33.352+01:00",
            "properties": "{\"ElapsedMilliseconds\":7.4673,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=18&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=18&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 7.4673ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000029-0002-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 182,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=18&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:45:36.125+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=18&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=18&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000080-0000-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 183,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=18&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 8.1331ms",
            "timestamp": "2022-01-06T14:45:36.133+01:00",
            "properties": "{\"ElapsedMilliseconds\":8.1331,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=18&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=18&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 8.1331ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000080-0000-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 184,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=19&count=10&level=&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:45:38.412+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=19&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=19&count=10&level=&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000049-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 185,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=19&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 8.3286ms",
            "timestamp": "2022-01-06T14:45:38.421+01:00",
            "properties": "{\"ElapsedMilliseconds\":8.3286,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=19&count=10&level=&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=19&count=10&level=&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 8.3286ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000049-0001-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 186,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.bundle.min.js.map - -",
            "timestamp": "2022-01-06T14:45:54.329+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/bootstrap.bundle.min.js.map\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.bundle.min.js.map - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000005c-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/bootstrap.bundle.min.js.map\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 187,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.bundle.min.js.map - - - 404 - - 1.0667ms",
            "timestamp": "2022-01-06T14:45:54.33+01:00",
            "properties": "{\"ElapsedMilliseconds\":1.0667,\"StatusCode\":404,\"ContentType\":null,\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/bootstrap.bundle.min.js.map\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.bundle.min.js.map - - - 404 - - 1.0667ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000005c-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/bootstrap.bundle.min.js.map\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 188,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.min.css.map - -",
            "timestamp": "2022-01-06T14:45:54.368+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/bootstrap.min.css.map\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.min.css.map - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000005e-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/bootstrap.min.css.map\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 189,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.min.css.map - - - 404 - - 1.1882ms",
            "timestamp": "2022-01-06T14:45:54.369+01:00",
            "properties": "{\"ElapsedMilliseconds\":1.1882,\"StatusCode\":404,\"ContentType\":null,\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/bootstrap.min.css.map\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.min.css.map - - - 404 - - 1.1882ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000005e-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/bootstrap.min.css.map\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 190,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=1&count=10&level=Information&search=&startDate=&endDate= - -",
            "timestamp": "2022-01-06T14:47:41.839+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=1&count=10&level=Information&search=&startDate=&endDate=\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=1&count=10&level=Information&search=&startDate=&endDate= - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000058-0003-fe00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 191,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=1&count=10&level=Information&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 19.8272ms",
            "timestamp": "2022-01-06T14:47:41.858+01:00",
            "properties": "{\"ElapsedMilliseconds\":19.8272,\"StatusCode\":200,\"ContentType\":\"application/json;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/api/logs\",\"QueryString\":\"?page=1&count=10&level=Information&search=&startDate=&endDate=\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/api/logs?page=1&count=10&level=Information&search=&startDate=&endDate= - - - 200 - application/json;charset=utf-8 19.8272ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000058-0003-fe00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/api/logs\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 192,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/index.html - -",
            "timestamp": "2022-01-06T14:48:19.34+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/index.html\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/index.html - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000082-0000-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/index.html\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 193,
            "level": "Information",
            "message": "Request finished HTTP/2 GET https://localhost:44377/serilog-ui/index.html - - - 200 - text/html;charset=utf-8 9.3211ms",
            "timestamp": "2022-01-06T14:48:19.35+01:00",
            "properties": "{\"ElapsedMilliseconds\":9.3211,\"StatusCode\":200,\"ContentType\":\"text/html;charset=utf-8\",\"ContentLength\":null,\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/index.html\",\"QueryString\":\"\",\"HostingRequestFinishedLog\":\"Request finished HTTP/2 GET https://localhost:44377/serilog-ui/index.html - - - 200 - text/html;charset=utf-8 9.3211ms\",\"EventId\":{\"Id\":2,\"Name\":\"RequestFinished\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000082-0000-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/index.html\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 194,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/font-awesome.min.css - -",
            "timestamp": "2022-01-06T14:48:19.37+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/font-awesome.min.css\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/font-awesome.min.css - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000002d-0002-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/font-awesome.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 195,
            "level": "Information",
            "message": "Sending file. Request path: '\"/font-awesome.min.css\"'. Physical path: '\"N/A\"'",
            "timestamp": "2022-01-06T14:48:19.372+01:00",
            "properties": "{\"VirtualPath\":\"/font-awesome.min.css\",\"PhysicalPath\":\"N/A\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"8000002d-0002-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/font-awesome.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 196,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.min.css - -",
            "timestamp": "2022-01-06T14:48:19.373+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/bootstrap.min.css\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/bootstrap.min.css - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000060-0004-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/bootstrap.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 197,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/main.min.css - -",
            "timestamp": "2022-01-06T14:48:19.373+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/main.min.css\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/main.min.css - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000017-0004-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/main.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 198,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/netstack.min.js - -",
            "timestamp": "2022-01-06T14:48:19.374+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/netstack.min.js\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/netstack.min.js - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"80000083-0006-fe00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/netstack.min.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 199,
            "level": "Information",
            "message": "Sending file. Request path: '\"/main.min.css\"'. Physical path: '\"N/A\"'",
            "timestamp": "2022-01-06T14:48:19.375+01:00",
            "properties": "{\"VirtualPath\":\"/main.min.css\",\"PhysicalPath\":\"N/A\",\"EventId\":{\"Id\":2,\"Name\":\"FileServed\"},\"SourceContext\":\"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware\",\"RequestId\":\"80000017-0004-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/main.min.css\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 200,
            "level": "Information",
            "message": "Request starting HTTP/2 GET https://localhost:44377/serilog-ui/main.js - -",
            "timestamp": "2022-01-06T14:48:19.376+01:00",
            "properties": "{\"Protocol\":\"HTTP/2\",\"Method\":\"GET\",\"ContentType\":null,\"ContentLength\":null,\"Scheme\":\"https\",\"Host\":\"localhost:44377\",\"PathBase\":\"\",\"Path\":\"/serilog-ui/main.js\",\"QueryString\":\"\",\"HostingRequestStartingLog\":\"Request starting HTTP/2 GET https://localhost:44377/serilog-ui/main.js - -\",\"EventId\":{\"Id\":1,\"Name\":\"RequestStarting\"},\"SourceContext\":\"Microsoft.AspNetCore.Hosting.Diagnostics\",\"RequestId\":\"8000001e-0005-fa00-b63f-84710c7967bb\",\"RequestPath\":\"/serilog-ui/main.js\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 201,
            "level": "Error",
            "message": "An unhandled exception has occurred while executing the request.",
            "timestamp": "2022-01-08T19:26:08.643+01:00",
            "exception": "{ \"_t\" : \"MongoDB.Bson.BsonDocument, MongoDB.Bson\", \"_v\" : { \"HelpLink\" : null, \"Source\" : \"SampleWebApp\", \"HResult\" : -2146233088, \"Message\" : \"Exception of type 'System.Exception' was thrown.\", \"StackTrace\" : \"   at SampleWebApp.Controllers.HomeController.Privacy() in C:\\\\Users\\\\matte\\\\CodeProjects\\\\study\\\\serilog-ui\\\\samples\\\\SampleWebApp\\\\Controllers\\\\HomeController.cs:line 28\\r\\n   at lambda_method74(Closure , Object , Object[] )\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute(IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()\\r\\n--- End of stack trace from previous location ---\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()\\r\\n--- End of stack trace from previous location ---\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextResourceFilter>g__Awaited|24_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Rethrow(ResourceExecutedContextSealed context)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.InvokeFilterPipelineAsync()\\r\\n--- End of stack trace from previous location ---\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Logged|17_1(ResourceInvoker invoker)\\r\\n   at Microsoft.AspNetCore.Routing.EndpointMiddleware.<Invoke>g__AwaitRequestTask|6_0(Endpoint endpoint, Task requestTask, ILogger logger)\\r\\n   at Serilog.Ui.Web.SerilogUiMiddleware.Invoke(HttpContext httpContext) in C:\\\\Users\\\\matte\\\\CodeProjects\\\\study\\\\serilog-ui\\\\src\\\\Serilog.Ui.Web\\\\SerilogUiMiddleware.cs:line 106\\r\\n   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)\\r\\n   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)\\r\\n   at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)\\r\\n   at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)\\r\\n   at Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore.MigrationsEndPointMiddleware.Invoke(HttpContext context)\\r\\n   at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware.Invoke(HttpContext context)\", \"Data\" : { } } }",
            "properties": "{\"EventId\":{\"Id\":1,\"Name\":\"UnhandledException\"},\"SourceContext\":\"Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware\",\"RequestId\":\"800000de-0004-ff00-b63f-84710c7967bb\",\"RequestPath\":\"/Home/Privacy\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 202,
            "level": "Error",
            "message": "An unhandled exception has occurred while executing the request.",
            "timestamp": "2022-01-08T19:26:15.586+01:00",
            "exception": "{ \"_t\" : \"MongoDB.Bson.BsonDocument, MongoDB.Bson\", \"_v\" : { \"HelpLink\" : null, \"Source\" : \"SampleWebApp\", \"HResult\" : -2146233088, \"Message\" : \"Exception of type 'System.Exception' was thrown.\", \"StackTrace\" : \"   at SampleWebApp.Controllers.HomeController.Privacy() in C:\\\\Users\\\\matte\\\\CodeProjects\\\\study\\\\serilog-ui\\\\samples\\\\SampleWebApp\\\\Controllers\\\\HomeController.cs:line 28\\r\\n   at lambda_method74(Closure , Object , Object[] )\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute(IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()\\r\\n--- End of stack trace from previous location ---\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()\\r\\n--- End of stack trace from previous location ---\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextResourceFilter>g__Awaited|24_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Rethrow(ResourceExecutedContextSealed context)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.InvokeFilterPipelineAsync()\\r\\n--- End of stack trace from previous location ---\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Logged|17_1(ResourceInvoker invoker)\\r\\n   at Microsoft.AspNetCore.Routing.EndpointMiddleware.<Invoke>g__AwaitRequestTask|6_0(Endpoint endpoint, Task requestTask, ILogger logger)\\r\\n   at Serilog.Ui.Web.SerilogUiMiddleware.Invoke(HttpContext httpContext) in C:\\\\Users\\\\matte\\\\CodeProjects\\\\study\\\\serilog-ui\\\\src\\\\Serilog.Ui.Web\\\\SerilogUiMiddleware.cs:line 106\\r\\n   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)\\r\\n   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)\\r\\n   at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)\\r\\n   at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)\\r\\n   at Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore.MigrationsEndPointMiddleware.Invoke(HttpContext context)\\r\\n   at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware.Invoke(HttpContext context)\", \"Data\" : { } } }",
            "properties": "{\"EventId\":{\"Id\":1,\"Name\":\"UnhandledException\"},\"SourceContext\":\"Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware\",\"RequestId\":\"80000072-0004-f800-b63f-84710c7967bb\",\"RequestPath\":\"/Home/Privacy\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 203,
            "level": "Error",
            "message": "An unhandled exception has occurred while executing the request.",
            "timestamp": "2022-01-08T19:26:22.728+01:00",
            "exception": "{ \"_t\" : \"MongoDB.Bson.BsonDocument, MongoDB.Bson\", \"_v\" : { \"HelpLink\" : null, \"Source\" : \"SampleWebApp\", \"HResult\" : -2146233088, \"Message\" : \"Exception of type 'System.Exception' was thrown.\", \"StackTrace\" : \"   at SampleWebApp.Controllers.HomeController.Privacy() in C:\\\\Users\\\\matte\\\\CodeProjects\\\\study\\\\serilog-ui\\\\samples\\\\SampleWebApp\\\\Controllers\\\\HomeController.cs:line 28\\r\\n   at lambda_method74(Closure , Object , Object[] )\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute(IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()\\r\\n--- End of stack trace from previous location ---\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()\\r\\n--- End of stack trace from previous location ---\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextResourceFilter>g__Awaited|24_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Rethrow(ResourceExecutedContextSealed context)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.InvokeFilterPipelineAsync()\\r\\n--- End of stack trace from previous location ---\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Logged|17_1(ResourceInvoker invoker)\\r\\n   at Microsoft.AspNetCore.Routing.EndpointMiddleware.<Invoke>g__AwaitRequestTask|6_0(Endpoint endpoint, Task requestTask, ILogger logger)\\r\\n   at Serilog.Ui.Web.SerilogUiMiddleware.Invoke(HttpContext httpContext) in C:\\\\Users\\\\matte\\\\CodeProjects\\\\study\\\\serilog-ui\\\\src\\\\Serilog.Ui.Web\\\\SerilogUiMiddleware.cs:line 106\\r\\n   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)\\r\\n   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)\\r\\n   at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)\\r\\n   at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)\\r\\n   at Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore.MigrationsEndPointMiddleware.Invoke(HttpContext context)\\r\\n   at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware.Invoke(HttpContext context)\", \"Data\" : { } } }",
            "properties": "{\"EventId\":{\"Id\":1,\"Name\":\"UnhandledException\"},\"SourceContext\":\"Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware\",\"RequestId\":\"8000013f-0006-fc00-b63f-84710c7967bb\",\"RequestPath\":\"/Home/Privacy\"}",
            "propertyType": "json"
        },
        {
            "rowNo": 204,
            "level": "Error",
            "message": "An unhandled exception has occurred while executing the request.",
            "timestamp": "2022-01-08T19:26:26.507+01:00",
            "exception": "{ \"_t\" : \"MongoDB.Bson.BsonDocument, MongoDB.Bson\", \"_v\" : { \"HelpLink\" : null, \"Source\" : \"SampleWebApp\", \"HResult\" : -2146233088, \"Message\" : \"Exception of type 'System.Exception' was thrown.\", \"StackTrace\" : \"   at SampleWebApp.Controllers.HomeController.Privacy() in C:\\\\Users\\\\matte\\\\CodeProjects\\\\study\\\\serilog-ui\\\\samples\\\\SampleWebApp\\\\Controllers\\\\HomeController.cs:line 28\\r\\n   at lambda_method74(Closure , Object , Object[] )\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute(IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()\\r\\n--- End of stack trace from previous location ---\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()\\r\\n--- End of stack trace from previous location ---\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextResourceFilter>g__Awaited|24_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Rethrow(ResourceExecutedContextSealed context)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.InvokeFilterPipelineAsync()\\r\\n--- End of stack trace from previous location ---\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Logged|17_1(ResourceInvoker invoker)\\r\\n   at Microsoft.AspNetCore.Routing.EndpointMiddleware.<Invoke>g__AwaitRequestTask|6_0(Endpoint endpoint, Task requestTask, ILogger logger)\\r\\n   at Serilog.Ui.Web.SerilogUiMiddleware.Invoke(HttpContext httpContext) in C:\\\\Users\\\\matte\\\\CodeProjects\\\\study\\\\serilog-ui\\\\src\\\\Serilog.Ui.Web\\\\SerilogUiMiddleware.cs:line 106\\r\\n   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)\\r\\n   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)\\r\\n   at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)\\r\\n   at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)\\r\\n   at Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore.MigrationsEndPointMiddleware.Invoke(HttpContext context)\\r\\n   at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware.Invoke(HttpContext context)\", \"Data\" : { } } }",
            "properties": "{\"EventId\":{\"Id\":1,\"Name\":\"UnhandledException\"},\"SourceContext\":\"Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware\",\"RequestId\":\"800000e0-0004-ff00-b63f-84710c7967bb\",\"RequestPath\":\"/Home/Privacy\"}",
            "propertyType": "json"
        }
    ],
    "total": 3357,
    "count": 10,
    "currentPage": 1
}