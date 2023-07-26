using CodersToolkit.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics;

namespace CodersToolkit.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

        public HomeController(ILogger logger)
        {
            _logger = logger;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View(new ConverterViewModel());
        }

        [HttpPost]
        public IActionResult Index(ConverterViewModel model)
        {
            ModelState.Clear();
            var newModel = new ConverterViewModel()
            {
                CSharpClass = model.CSharpClass,
                SqlTable = model.CSharpClass
            };
            return View(newModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        }
    }
}