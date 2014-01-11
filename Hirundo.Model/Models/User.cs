namespace Hirundo.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web.Helpers;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class User
    {
        public User()
        {
            this.Following = new List<ObjectId>();
        }

        public ObjectId Id { get; set; }

        [BsonRequired]
        public string Username { get; set; }

        public string Fullname { get; set; }

        [BsonRequired]
        public string Email { get; set; }

        public ObjectId ImgId { get; set; }

        [BsonRequired]
        public string PasswordHash { get; set; }

        [BsonRequired]
        public string PasswordSalt { get; set; }

        public DateTime RegistrationDate { get; set; }

        public List<ObjectId> Following { get; set; }

        public bool VerifyPassword(string password)
        {
            if (this.PasswordHash == null || this.PasswordSalt == null)
            {
                return false;
            }
            else
            {
                return Crypto.VerifyHashedPassword(this.PasswordHash, password + this.PasswordSalt);
            }
        }
    }
}
