using WheatherApp.Models;

namespace WheatherApp.Services.Contracts
{
    public interface IOpenweatherConnect
    {
        public Task<Response> RequestWeatherData(string city,string units);
    }
}
