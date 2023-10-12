using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WeatherApp.Models;
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

        public async Task<IActionResult> Index()
        {
            WheaterDTO wheater = await wheatherService.RequestWeatherData("Varna" , "metric");
            return View(wheater);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}