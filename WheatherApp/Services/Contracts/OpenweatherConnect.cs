using Newtonsoft.Json;
using System.Net;
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
        public async Task<Response> RequestWheatherData(string city, string units)
        {
            string wheatherUrl = wheatherUrlPreliminary + city + wheatherKey + units;

            try
            {
                var client = httpClient.CreateClient("WheatherAPI");
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
    }
}
