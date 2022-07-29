using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

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
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastSeen { get; set; }
        public string PhoneNumber { get; set; }
    }
}
