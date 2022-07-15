using com.tweetapp.api.Helpers;
using com.tweetapp.application.Queries;
using com.tweetapp.application.Response;
using com.tweetapp.domain.DAOEntities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public TweetController(ITweetQuery tweetQuery)
        {
            _tweetQuery = tweetQuery;   
        }

        [HttpPost]
        [Route("PostTweet")]
        public async Task<ApiResponse<string>> PostTweet([FromBody] TweetDAO tweet)
        {
            tweet.CreatedDate = DateTime.Now;
            var userId = GetIdFromToken.GetId(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            
            return await _tweetQuery.PostTweet(tweet,userId);
        }

        [HttpGet]
        [Route("MyTweets")]
        public async Task<ApiResponse<IEnumerable<TweetDAO>>> GetAllMyTweets()
        {
            var email = GetEmailFromToken.GetEmail(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            return await _tweetQuery.GetAllMyTweets(email);
        }

        [HttpGet]
        [Route("AllTweets")]
        public async Task<ApiResponse<IEnumerable<TweetWithUserDAO>>> GetAllTweets()
        {
            return await _tweetQuery.GetAllTweets();
        }
    }
}
