using WeatherApp.Models;

namespace WheatherApp.Services.Contracts
{
    public interface IOpenweatherConnect
    {
        public Task<WheaterDTO> RequestWeatherData(string city,string units);
    }
}
