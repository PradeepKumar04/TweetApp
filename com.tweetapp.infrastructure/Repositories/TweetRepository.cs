using com.tweetapp.domain.Models;
using com.tweetapp.infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
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
        Task<IEnumerable<Tweet>> ViewMyTweets(string email);
    }
    public class TweetRepository : ITweetRepository
    {
        private readonly TwitterDbContext _context;
        public TweetRepository(TwitterDbContext context)
        {
            _context = context;
        }
        public async Task<bool> PostTweet(Tweet tweet)
        {
            await _context.Tweets.AddAsync(tweet);
            var changes = await _context.SaveChangesAsync();
            return  changes > 0;
        }

        public async Task<IEnumerable<Tweet>> ViewAllTweets()
        {
           return  await _context.Tweets.Include(s=>s.User).ToListAsync();
        }

        public async Task<IEnumerable<Tweet>> ViewMyTweets(string email)
        {
            return await _context.Tweets.Where(s => s.User.Email == email).ToListAsync();
        }
    }
}
