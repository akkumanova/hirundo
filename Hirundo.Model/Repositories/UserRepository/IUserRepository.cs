using System.Collections.Generic;
using Hirundo.Model.Models;
using MongoDB.Bson;

namespace Hirundo.Model.Repositories.UserRepository
{
    public interface IUserRepository
    {
        User FindByUsername(string username);

        User FindById(ObjectId userId);

        List<User> GetUsers(List<ObjectId> userIds);

        long GetFollowersCount(ObjectId userId);
    }
}
