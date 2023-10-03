using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WheatherApp.Models;
using WheatherApp.Services.Contracts;

namespace WheatherApp.Controllers
{
    public class HomeController : Controller
    {
        IOpenweatherConnect wheatherService;
        public HomeController(IOpenweatherConnect _wheatherService)
        {
            wheatherService = _wheatherService;
        }

        public IActionResult Index()
        {
            wheatherService.RequestWheatherData("Varna" , "metric");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}