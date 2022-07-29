using System;
using System.Collections.Generic;
using System.Text;

namespace com.tweetapp.domain.Models
{
    public class TweetAppDbSettings : ITweetAppDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string UserCollectionName { get; set; }
        public string TweetCollectionName { get; set; }
    }

    public interface ITweetAppDbSettings
    {
        public string UserCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string TweetCollectionName { get; set; }
    }
}
