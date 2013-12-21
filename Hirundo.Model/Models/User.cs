using System;
using System.Collections.Generic;
using System.Web.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Hirundo.Model.Models
{
    public class User
    {
        public User()
        {
            this.Follows = new List<string>();
        }

        public ObjectId Id { get; set; }

        [BsonRequired]
        public string Username { get; set; }

        public string Email { get; set; }

        [BsonIgnore]
        public string Password { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

        [JsonIgnore]
        public string PasswordSalt { get; set; }

        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime RegistrationDate { get; set; }

        public List<string> Follows { get; set; }

        public void SetPassword(string password)
        {
            if (password == null)
            {
                this.PasswordSalt = null;
                this.PasswordHash = null;
            }
            else
            {
                this.PasswordSalt = Crypto.GenerateSalt();
                this.PasswordHash = Crypto.HashPassword(password + this.PasswordSalt);
            }
        }

        public bool VerifyPassword(string password)
        {
            return Crypto.VerifyHashedPassword(this.PasswordHash, password + this.PasswordSalt);
        }
    }
}
