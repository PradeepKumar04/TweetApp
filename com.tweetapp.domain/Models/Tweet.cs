using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace com.tweetapp.domain.Models
{
    public class Tweet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRequired]
        [BsonElement("message")]
        public string Message { get; set; }
        [BsonRequired]
        [BsonElement("userId")]
        public string UserId  { get; set; }
        public DateTime UploadDate { get; set; }
        public User User { get; set; }
        public List<string> ImagePath { get; set; }
        public List<User> LikedUsers { get; set; }
        public List<ReplyTweet> ReplyTweets { get; set; }

    }

    public class ReplyTweet
    {
        public User User { get; set; }
        public string ReplyMessage { get; set; }
    }
}
