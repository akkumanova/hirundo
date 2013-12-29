﻿using System;
using System.Collections.Generic;
using System.Web.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hirundo.Model.Models
{
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

        public string Email { get; set; }

        public ObjectId ImgId { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime RegistrationDate { get; set; }

        public List<ObjectId> Following { get; set; }

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
