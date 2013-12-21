using Hirundo.Model.Data;
using Hirundo.Model.Models;
using MongoDB.Driver;

namespace Hirundo.Model.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private IMongoContext mongoContext;

        public UserRepository(IMongoContext mongoContext)
        {
            this.mongoContext = mongoContext;
        }

        public User FindByUsername(string username)
        {
            var query = new QueryDocument("Username", username);

            return this.mongoContext.GetCollection<User>().FindOne(query);
        }
    }
}
