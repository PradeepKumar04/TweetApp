using com.tweetapp.domain;
using com.tweetapp.infrastructure.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDbClient _client;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDbClient client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var data =_client.GetUserCollection();
            return null;
        }


    }

    public class Example
    {
        [BsonId]
        [BsonElement("_id")]
        public Object Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
    }
}
