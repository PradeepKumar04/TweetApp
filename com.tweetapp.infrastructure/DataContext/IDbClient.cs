using com.tweetapp.domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.tweetapp.infrastructure.DataContext
{
    public interface IDbClient
    {
        public IMongoCollection<User> GetUserCollection();

        public IMongoCollection<Tweet> GetTweetCollection();
    }
}
