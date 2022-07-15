using com.tweetapp.domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.tweetapp.domain.DAOEntities
{
    public class UserDAO
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? LastSeen { get; set; }
    }
}
