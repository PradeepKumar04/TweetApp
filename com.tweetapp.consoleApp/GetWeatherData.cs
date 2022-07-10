using com.tweetapp.domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace com.tweetapp.consoleApp
{
    public class GetWeatherData
    {
        public  async Task<List<WeatherForecast>> GetData()
        {
            using HttpClient client = new HttpClient();
            UriBuilder builder = new UriBuilder("https://localhost:44387/weatherforecast");

            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var httpResponse = await client.GetAsync(builder.Uri);

            if (!httpResponse.IsSuccessStatusCode)
            {
                var response = await httpResponse.Content.ReadAsStringAsync();
                string exception = JObject.Parse(response).SelectToken("message").ToString();
                throw new HttpRequestException("Error:" + exception);
            }
            var content = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<WeatherForecast>>(content);
        }
    }
}
