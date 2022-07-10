using com.tweetapp.api.Controllers;
using com.tweetapp.domain;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace com.tweetapp.consoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            await Task.Delay(TimeSpan.FromSeconds(30));
            Console.WriteLine("Hello World!");
            GetWeatherData weatherData = new GetWeatherData();
            var data = await weatherData.GetData();
            foreach (var item in data)
            {
                Console.WriteLine(item.Date + " " + item.Summary + " " + item.TemperatureC + " " + item.TemperatureF);
            }
        }

       
    }

    
}
