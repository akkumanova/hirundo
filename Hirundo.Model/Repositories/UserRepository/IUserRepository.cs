using Hirundo.Model.Models;
using MongoDB.Bson;

namespace Hirundo.Model.Repositories.UserRepository
{
    public interface IUserRepository
    {
        User FindByUsername(string username);

        User FindById(ObjectId userId);

        long GetFollowersCount(ObjectId userId);
    }
}
