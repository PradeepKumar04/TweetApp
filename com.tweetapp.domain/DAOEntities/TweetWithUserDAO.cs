using System;
using System.Collections.Generic;
using System.Text;

namespace com.tweetapp.domain.DAOEntities
{
    public class TweetWithUserDAO
    {
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; }
    }
}
