using com.tweetapp.domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.tweetapp.domain.DAOEntities
{
    public class UserRegisterDAO
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
