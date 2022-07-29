using com.tweetapp.domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.tweetapp.domain.DAOEntities
{
    public class TweetDAO
    {
        public string Message { get; set; }
        public string UserId { get; set; }
        public DateTime UploadDate { get; set; }
        public UserDAO User { get; set; }
        public List<string> ImagePath { get; set; }
        public List<User> LikedUsers { get; set; }
        public List<ReplyTweet> ReplyTweets { get; set; }
    }
}