using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleWebApplication.Models;
using System.Diagnostics;

namespace SampleWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int? id)
        {
            //if (id == null)
            //{
            //    try
            //    {
            //        throw new ArgumentNullException(nameof(id));
            //    }
            //    catch (Exception e)
            //    {
            //        _logger.LogError(e, e.Message);
            //    }
            //}

            return View();
        }

        public IActionResult Privacy()
        {
            _logger.LogWarning("Privacy api was called.");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}