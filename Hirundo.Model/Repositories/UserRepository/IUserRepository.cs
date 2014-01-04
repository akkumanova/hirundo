using System.Collections.Generic;
using Hirundo.Model.Models;
using MongoDB.Bson;

namespace Hirundo.Model.Repositories.UserRepository
{
    public interface IUserRepository
    {
        User FindByUsername(string username);

        User FindByEmail(string email);

        User FindById(ObjectId userId);

        void ChangePassword(ObjectId id, string newPassword);

        List<User> GetUsers(List<ObjectId> userIds);

        long GetFollowersCount(ObjectId userId);
    }
}
