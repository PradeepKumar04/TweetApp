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
            tweet.UploadDate = DateTime.Now;
            var userId = GetIdFromToken.GetId(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            
            return await _tweetQuery.PostTweet(tweet,userId);
        }

        [HttpGet]
        [Route("tweets/{username}")]
        public async Task<ApiResponse<IEnumerable<TweetDAO>>> GetTweetsByUserName([FromRoute] string username)
        {
            return await _tweetQuery.GetAllTweetsByUserName(username);
        }

        [HttpGet]
        [Route("AllTweets")]
        public async Task<ApiResponse<IEnumerable<TweetWithUserDAO>>> GetAllTweets()
        {
            return await _tweetQuery.GetAllTweets();
        }

        [HttpPut]
        [Route("{username}/like/{id}")]
        public async Task<ApiResponse<string>> LikeTweet([FromRoute]string username, [FromRoute] string id)
        {
            var userName = GetUserNameFromToken.GetNameFromToken(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            return await _tweetQuery.LikeTweet(userName, id);
        }

        [HttpPost]
        [Route("{username}/reply/{id}")]
        public async Task<ApiResponse<string>> ReplyTweet([FromRoute] string username, [FromRoute] string id, TweetReplyDAO reply)
        {
            var userName = GetUserNameFromToken.GetNameFromToken(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            return await  _tweetQuery.ReplyTweet(userName, id, reply.Reply);
        }

        [HttpPut]
        [Route("{username}/update/{id}")]
        public async Task<ApiResponse<bool>> UpdateTweet([FromRoute] string username, [FromRoute] string id,[FromBody] TweetDAO tweet)
        {

            var userName = GetUserNameFromToken.GetNameFromToken(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            return await _tweetQuery.UpdateTweet(id, userName,tweet);
        }

        [HttpDelete]
        [Route("{username}/delete/{id}")]
        public async Task<ApiResponse<bool>> DeleteTweet([FromRoute] string username,[FromRoute] string id)
        {
            var userName = GetUserNameFromToken.GetNameFromToken(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);

            return await _tweetQuery.DeleteTweet(id);
        }
        
    }
}
