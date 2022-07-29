using com.tweetapp.domain.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.tweetapp.infrastructure.DataContext
{
    public class DbClient : IDbClient
    {
        private readonly IMongoCollection<User> Users;
        private readonly IMongoCollection<Tweet> Tweets;

        public DbClient(ITweetAppDbSettings settings,IMongoClient client) 
        {
            var database = client.GetDatabase(settings.DatabaseName);
            Users = GetDBCollection<User>(database, settings.UserCollectionName);
            Tweets = GetDBCollection<Tweet>(database, settings.TweetCollectionName);
        }

        public IMongoCollection<Tweet> GetTweetCollection()
        {
            return Tweets;
        }

        public IMongoCollection<User> GetUserCollection()
        {
            return Users;
        }

        public static  IMongoCollection<T> GetDBCollection<T>(IMongoDatabase database,string collectionName)
        {
            var collectionList = database.ListCollectionNames().ToList();
            var isExists = collectionList.Any(x => x == collectionName);
            if (!isExists)
            {
                database.CreateCollection(collectionName);
            }
            return database.GetCollection<T>(collectionName);
        }
    }
}
