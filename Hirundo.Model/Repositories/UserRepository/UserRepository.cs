﻿using System.Collections.Generic;
using System.Linq;
using Hirundo.Model.Data;
using Hirundo.Model.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System.Web.Helpers;

namespace Hirundo.Model.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private MongoCollection<User> userCollection;

        public UserRepository(IMongoContext mongoContext)
        {
            this.userCollection = mongoContext.GetCollection<User>();
        }

        public User FindByUsername(string username)
        {
            var query = Query<User>.EQ(u => u.Username, username);

            return this.userCollection.FindOne(query);
        }

        public User FindByEmail(string email)
        {
            var query = Query<User>.Where(u => u.Email == email);

            return this.userCollection.FindOne(query);
        }

        public User FindById(ObjectId userId)
        {
            var query = Query<User>.EQ(u => u.Id, userId);

            return this.userCollection.FindOne(query);
        }

        public void ChangePassword(ObjectId id, string newPassword)
        {
            string passwordSalt = Crypto.GenerateSalt();
            string passwordHash = Crypto.HashPassword(newPassword + passwordSalt);

            var userQuery = Query<User>.EQ(u => u.Id, id);
            var update = Update<User>.Set(u => u.PasswordHash, passwordHash)
                                     .Set(u => u.PasswordSalt, passwordSalt);

            this.userCollection.Update(userQuery, update);
        }

        public List<User> GetUsers(List<ObjectId> userIds)
        {
            return this.userCollection.AsQueryable<User>()
                        .Where(u => u.Id.In(userIds))
                        .Take(25)
                        .ToList();
        }

        public long GetFollowersCount(ObjectId userId)
        {
            var followersQuery = Query<User>.Where(u => u.Following.Contains(userId));
            var followers = this.userCollection.Find(followersQuery);

            return followers.Count();
        }
    }
}
