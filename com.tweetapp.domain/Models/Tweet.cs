using System;
using System.Collections.Generic;
using System.Text;

namespace com.tweetapp.domain.Models
{
    public class Tweet
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int UserId  { get; set; }
        public DateTime CreatedDate { get; set; }
        public User User { get; set; }
    }
}
