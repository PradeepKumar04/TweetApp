using com.tweetapp.domain.Models;
using com.tweetapp.infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.tweetapp.infrastructure.Repositories
{
    public interface ITweetRepository
    {
        Task<bool> PostTweet(Tweet tweet);
        Task<IEnumerable<Tweet>> ViewAllTweets();
        Task<IEnumerable<Tweet>> ViewMyTweets(string username);
        Task<IEnumerable<Tweet>> ViewTweetsByUserName(string username);
        Task<bool> ReplyOrLikeTweet(Tweet tweet);
        Task<bool> DeleteTweet(string id);
        Task<Tweet> GetTweetByID(string id);
    }
    public class TweetRepository : ITweetRepository
    {
        
        private readonly IDbClient _dbClient;
        public TweetRepository( IDbClient dbClient)
        {
            _dbClient = dbClient;
        }

        public async Task<Tweet> GetTweetByID(string id)
        {
            var filter = Builders<Tweet>.Filter.Eq(s => s.Id, id);
            return await _dbClient.GetTweetCollection().Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> ReplyOrLikeTweet(Tweet tweet)
        {
            try
            {
                var filter = Builders<Tweet>.Filter.Eq(s=>s.Id,tweet.Id);
                var data= await _dbClient.GetTweetCollection().ReplaceOneAsync(filter, tweet);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> PostTweet(Tweet tweet)
        {
            try
            {
                await _dbClient.GetTweetCollection().InsertOneAsync(tweet);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Tweet>> ViewAllTweets()
        {
            var twitterData= await _dbClient.GetTweetCollection().Find(_=>true).ToListAsync();
            return twitterData;
        }

        public async Task<IEnumerable<Tweet>> ViewMyTweets(string username)
        {
            var filter = Builders<Tweet>.Filter.Eq(s => s.User.UserName,username);
            return await _dbClient.GetTweetCollection().Find(filter).ToListAsync();
        }

        public Task<IEnumerable<Tweet>> ViewTweetsByUserName(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteTweet(string id)
        {
            try
            {
            var filter = Builders<Tweet>.Filter.Eq(s => s.Id, id);
            await _dbClient.GetTweetCollection().DeleteOneAsync(filter);
            return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        
    }
}
