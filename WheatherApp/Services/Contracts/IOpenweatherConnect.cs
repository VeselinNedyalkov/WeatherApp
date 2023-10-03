using WheatherApp.Models;

namespace WheatherApp.Services.Contracts
{
    public interface IOpenweatherConnect
    {
        public Task<Response> RequestWheatherData(string city,string units);
    }
}
