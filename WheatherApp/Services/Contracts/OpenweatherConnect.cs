using Newtonsoft.Json;
using System.Net;
using System.Text;
using WheatherApp.Models;

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
        public async Task<Response> RequestWeatherData(string city, string units)
        {
            string wheatherUrl = GetWheatherUrl(city, units);

            try
            {
                var client = httpClient.CreateClient("WeatherAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(wheatherUrl);
                message.Method = HttpMethod.Get;

                HttpResponseMessage apiResponse = null;

                apiResponse = await client.SendAsync(message);
                var apiContet = await apiResponse.Content.ReadAsStringAsync();

                try
                {
                    Response responceModel = JsonConvert.DeserializeObject<Response>(apiContet);

                    if (apiResponse.StatusCode != HttpStatusCode.BadRequest ||
                        apiResponse.StatusCode != HttpStatusCode.NotFound)
                    {
                        responceModel.IsSuccess = true;
                        responceModel.Result = apiContet.ToString();
                        return responceModel;
                    }
                }
                catch (Exception ex)
                {
                    Response dto = new Response
                    {
                        Errors = new List<string> { Convert.ToString(ex.Message) },
                        IsSuccess = false
                    };
                    return dto;
                }
                return new Response { IsSuccess = false };
            }
            catch (Exception ex)
            {
                Response dto = new Response
                {
                    Errors = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };
                return dto;
            }
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
