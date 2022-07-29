using com.tweetapp.domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.tweetapp.domain.DAOEntities
{
    public class TweetWithUserDAO
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public DateTime UploadDate { get; set; }
        public List<string> ImagePath { get; set; }
        public List<UserDAO> LikedUsers { get; set; }
        public List<ReplyTweetDAO> ReplyTweets { get; set; }
        public UserDAO User { get; set; }
    }

    public class ReplyTweetDAO
    {
        public UserDAO User { get; set; }
        public string ReplyMessage { get; set; }
    }
}
