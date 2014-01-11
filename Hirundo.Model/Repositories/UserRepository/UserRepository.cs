namespace Hirundo.Model.Repositories.UserRepository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Helpers;
    using Hirundo.Model.Data;
    using Hirundo.Model.Models;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.GridFS;

    public class UserRepository : IUserRepository
    {
        private MongoCollection<User> userCollection;
        private MongoGridFS gridFs;

        public UserRepository(IHirundoContext mongoContext)
        {
            this.userCollection = mongoContext.GetCollection<User>();
            this.gridFs = mongoContext.GetGridFs();
        }

        public User GetUser(ObjectId userId)
        {
            var query = Query<User>.EQ(u => u.Id, userId);

            return this.userCollection.FindOne(query);
        }

        public User GetByUsername(string username)
        {
            var query = Query<User>.EQ(u => u.Username, username);

            return this.userCollection.FindOne(query);
        }

        public User GetByEmail(string email)
        {
            var query = Query<User>.Where(u => u.Email == email);

            return this.userCollection.FindOne(query);
        }

        public void AddUser(string fullname, string email, string password, string username)
        {
            string passwordSalt = Crypto.GenerateSalt();
            string passwordHash = Crypto.HashPassword(password + passwordSalt);
            var file = this.gridFs.FindOne("user.jpg");

            User user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Fullname = fullname,
                RegistrationDate = DateTime.Now,
                ImgId = new ObjectId(file.Id.ToString())
            };

            this.userCollection.Insert(user);
        }

        public void AddFollowing(ObjectId userId, ObjectId followedUserId)
        {
            var query = Query<User>.EQ(u => u.Id, userId);
            var update = Update<User>.Push<ObjectId>(u => u.Following, followedUserId);

            this.userCollection.Update(query, update, WriteConcern.Acknowledged);
        }

        public void DeleteFollowing(ObjectId userId, ObjectId followedUserId)
        {
            var query = Query<User>.EQ(u => u.Id, userId);
            var update = Update<User>.Pull<ObjectId>(u => u.Following, followedUserId);

            this.userCollection.Update(query, update, WriteConcern.Acknowledged);
        }

        public void ChangePassword(ObjectId id, string newPassword)
        {
            string passwordSalt = Crypto.GenerateSalt();
            string passwordHash = Crypto.HashPassword(newPassword + passwordSalt);

            var query = Query<User>.EQ(u => u.Id, id);
            var update = Update<User>.Set(u => u.PasswordHash, passwordHash)
                                     .Set(u => u.PasswordSalt, passwordSalt);

            this.userCollection.Update(query, update);
        }

        public long GetFollowersCount(ObjectId userId)
        {
            var query = Query<User>.Where(u => u.Following.Contains(userId));

            return this.userCollection.Find(query).Count();
        }

        public IEnumerable<User> GetUsers(List<ObjectId> userIds, int take, int skip)
        {
            var query = Query<User>.In<ObjectId>(u => u.Id, userIds);

            return this.userCollection.Find(query)
                        .OrderBy(u => u.Username)
                        .Skip(skip)
                        .Take(take)
                        .ToList();
        }

        public IEnumerable<User> GetFollowers(ObjectId userId, int take, int skip)
        {
            var query = Query<User>.Where(u => u.Following.Contains(userId));

            return this.userCollection.Find(query)
                            .OrderBy(u => u.Username)
                            .Skip(skip)
                            .Take(take)
                            .ToList();
        }

        public IEnumerable<User> GetFollowing(ObjectId userId, int take, int skip)
        {
            var user = this.GetUser(userId);

            return this.GetUsers(user.Following, take, skip);
        }
    }
}
