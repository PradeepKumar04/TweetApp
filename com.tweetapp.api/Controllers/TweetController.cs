using Azure.Messaging.ServiceBus;
using com.tweetapp.api.Helpers;
using com.tweetapp.application.Queries;
using com.tweetapp.application.Response;
using com.tweetapp.domain.DAOEntities;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class TweetController : ControllerBase
    {
        protected readonly ITweetQuery _tweetQuery;
        protected readonly IConfiguration _config;
        protected readonly ILogger<TweetController> _logger;
        private const string connectionString = "Endpoint=sb://tweetappeventhub2.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=3qVKYR0wDhpusLmfFvbyZOOTICQgIIcsW1k7Sm/fZO8=";
        private const string eventName = "tweetappeventhub";

        public TweetController(ITweetQuery tweetQuery,ILogger<TweetController> logger,IConfiguration config)
        {
            _tweetQuery = tweetQuery;
            _logger = logger;
            _config = config;
        }


        /// <summary>
        /// Creates a new tweet
        /// </summary>
        /// <param name="tweet">Request's payload</param>
        /// <returns>Adds a new tweet</returns>
        /// <response code="201">Tweet created successfully</response>
        [HttpPost]
        [Route("PostTweet")]
        public async Task<ApiResponse<string>> PostTweet([FromBody] TweetDAO tweet)
        {
            tweet.UploadDate = DateTime.Now;
            var userId = GetIdFromToken.GetId(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            _logger.LogInformation($"{userId} posted tweet");

            //using (var producer =
            //     new ProducerBuilder<Null, string>(new ProducerConfig { BootstrapServers = "localhost:9092" }).Build())
            //{
            //    try
            //    {
            //        Console.WriteLine(producer.ProduceAsync("tweet_app", new Message<Null, string> { Value = "User Id "+userId+" posted a tweet." })
            //            .GetAwaiter()
            //            .GetResult());


            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine($"Oops, something went wrong: {e}");
            //    }
                
            //};
            return await _tweetQuery.PostTweet(tweet,userId);
        }

        /// <summary>
        /// Returns tweets by username
        /// </summary>
        /// <param name="username">Request's payload</param>
        /// <returns>tweets by username</returns>
        /// <response code="200">Returns tweets by username</response>
        [HttpGet]
        [Route("tweets/{username}")]
        public async Task<ApiResponse<IEnumerable<TweetDAO>>> GetTweetsByUserName([FromRoute] string username)
        {
            _logger.LogInformation($"{username} get tweets by user name");

            return await _tweetQuery.GetAllTweetsByUserName(username);
        }

        /// <summary>
        /// Returns a list of all tweets
        /// </summary>
        /// <returns> A list of all tweets</returns>
        /// <remarks>
        /// Sample request
        /// GET/api/Tweet/AllTweets
        /// </remarks>
        /// <response code="200">Returns a list of all tweets </response>
        [HttpGet]
        [Route("AllTweets")]
        public async Task<ApiResponse<IEnumerable<TweetWithUserDAO>>> GetAllTweets()
        {
            _logger.LogInformation($"Get All Tweets");
            return await _tweetQuery.GetAllTweets();
        }

        /// <summary>
        /// Updates a tweet by getting liked through username and Id
        /// </summary>
        /// <param name="username">Request's payload</param>
        /// <param name="id">Request's payload</param>
        /// <returns>likes a tweet by username and Id</returns>
        /// <response code="200">Updated tweet by a like </response>
        [HttpPut]
        [Route("{username}/like/{id}")]
        public async Task<ApiResponse<string>> LikeTweet([FromRoute]string username, [FromRoute] string id)
        {
            var userName = GetUserNameFromToken.GetNameFromToken(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            _logger.LogInformation($"{userName} liked the tweet");
            return await _tweetQuery.LikeTweet(userName, id);
        }


        /// <summary>
        /// Replying a tweet by username and Id
        /// </summary>
        /// <param name="username">Request's payload</param>
        /// <param name="id">Request's payload</param>
        /// <param name="reply"></param>
        /// <returns>Replying a tweet</returns>
        /// <response code="201">Tweet replied successfully</response>
        [HttpPost]
        [Route("{username}/reply/{id}")]
        public async Task<ApiResponse<string>> ReplyTweet([FromRoute] string username, [FromRoute] string id, TweetReplyDAO reply)
        {
            var userName = GetUserNameFromToken.GetNameFromToken(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            _logger.LogInformation($"{userName} replied the tweet");
            return await  _tweetQuery.ReplyTweet(userName, id, reply.Reply);
        }

        /// <summary>
        /// Updates a tweet by username and Id
        /// </summary>
        /// <param name="username">Request's payload</param>
        /// <param name="id">Request's payload</param>
        /// <param name="tweet"></param>
        /// <returns>Updates a tweet</returns>
        /// <response code="200">Tweet updated successfully</response>
        [HttpPut]
        [Route("{username}/update/{id}")]
        public async Task<ApiResponse<bool>> UpdateTweet([FromRoute] string username, [FromRoute] string id,[FromBody] TweetDAO tweet)
        {

            var userName = GetUserNameFromToken.GetNameFromToken(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            _logger.LogInformation($"{userName} updated the tweet");
            return await _tweetQuery.UpdateTweet(id, userName,tweet);
        }


        /// <summary>
        /// Deletes a tweet by username and Id
        /// </summary>
        /// <param name="username">Request's payload</param>
        /// <param name="id">Request's payload</param>
        /// <returns>Deletes a tweet</returns>
        /// <response code="204">Tweet deleted successfully</response>
        [HttpDelete]
        [Route("{username}/delete/{id}")]
        public async Task<ApiResponse<bool>> DeleteTweet([FromRoute] string username,[FromRoute] string id)
        {
            var userName = GetUserNameFromToken.GetNameFromToken(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            _logger.LogInformation($"{userName} Deleted the tweet");
            string message = username + "deleted tweet " + " on " + DateTime.Now;
            string connectionstr = _config.GetValue<string>("sbConnString");
            var client = new ServiceBusClient(connectionstr);
            var sender = client.CreateSender("tweet-app-messaging");
            var sbmsg = new ServiceBusMessage(message);
            await sender.SendMessageAsync(sbmsg);
            return await _tweetQuery.DeleteTweet(id);
        }

        /// <summary>
        /// Updates a tweet by username and Id
        /// </summary>
        /// <param name="username">Request's payload</param>
        /// <param name="id">Request's payload</param>
        /// <param name="tweet"></param>
        /// <returns>Updates a tweet</returns>
        /// <response code="200">Tweet updated successfully</response>
        [HttpPut]
        [Route("edit/tweet")]
        public async Task<ApiResponse<bool>> EditTweet([FromBody]EditTweetDAO editTweet)
        {
            _logger.LogInformation($"Tweet {editTweet.Id} updated.");
            return await _tweetQuery.EditTweet(editTweet);
        }
        
    }
}
