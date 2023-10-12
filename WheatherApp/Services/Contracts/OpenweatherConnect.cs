using Newtonsoft.Json;
using System.Net;
using System.Text;
using WeatherApp.Models;

namespace WheatherApp.Services.Contracts
{
    public class OpenweatherConnect : IOpenweatherConnect
    {
        private string wheatherUrlPreliminary;
        private string wheatherKey;
        public IHttpClientFactory httpClient { get; set; }

        public OpenweatherConnect(IHttpClientFactory _httpClient,
            IConfiguration config)
        {
            httpClient = _httpClient;
            wheatherUrlPreliminary = config.GetValue<string>("Openweathermap:StartUrl");
            wheatherKey = config.GetValue<string>("Openweathermap:ApiKey");
        }
        public async Task<WheaterDTO> RequestWeatherData(string city, string units)
        {
            string wheatherUrl = GetWheatherUrl(city, units);


            var client = httpClient.CreateClient("WeatherAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(wheatherUrl);
            message.Method = HttpMethod.Get;

            HttpResponseMessage apiResponse = null;

            apiResponse = await client.SendAsync(message);
            var apiContet = await apiResponse.Content.ReadAsStringAsync(); 


            if (apiResponse.StatusCode != HttpStatusCode.BadRequest ||
                apiResponse.StatusCode != HttpStatusCode.NotFound)
            {
                WheaterDTO result = JsonConvert.DeserializeObject<WheaterDTO>(apiContet);
                return result;
            }

            return new WheaterDTO();
        }
        private string GetWheatherUrl(string city, string units)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(wheatherUrlPreliminary);
            sb.Append(city);
            sb.Append(wheatherKey);
            sb.Append(units);

            return sb.ToString();
        }
    }


}
