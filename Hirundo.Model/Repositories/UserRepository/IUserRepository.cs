namespace Hirundo.Model.Repositories.UserRepository
{
    using System.Collections.Generic;
    using Hirundo.Model.Models;
    using MongoDB.Bson;

    public interface IUserRepository
    {
        User FindByUsername(string username);

        User FindByEmail(string email);

        User FindById(ObjectId userId);

        void ChangePassword(ObjectId id, string newPassword);

        List<User> GetUsers(List<ObjectId> userIds);

        long GetFollowersCount(ObjectId userId);

        List<User> GetFollowers(ObjectId userId, int take, int skip);

        void AddUser(string fullname, string email, string password, string username);
    }
}
