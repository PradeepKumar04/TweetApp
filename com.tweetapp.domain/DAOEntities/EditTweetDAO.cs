using System;
using System.Collections.Generic;
using System.Text;

namespace com.tweetapp.domain.DAOEntities
{
    public class EditTweetDAO
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public List<string> ImagePath { get; set; }
    }
}
