using System;
using System.Collections.Generic;
using System.Text;

namespace com.tweetapp.domain.Models
{
    public enum Gender
    {
        Male=1,
        Female=2,
        Others=3
    }
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastSeen { get; set; }
        public ICollection<Tweet> Tweets { get; set; }
    }
}
