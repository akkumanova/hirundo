using Hirundo.Model.Data;
using Hirundo.Model.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

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

        public User FindById(ObjectId userId)
        {
            var query = Query<User>.EQ(u => u.Id, userId);

            return this.userCollection.FindOne(query);
        }

        public long GetFollowersCount(ObjectId userId)
        {
            var followersQuery = Query<User>.Where(u => u.Following.Contains(userId));
            var followers = this.userCollection.Find(followersQuery);

            return followers.Count();
        }
    }
}
