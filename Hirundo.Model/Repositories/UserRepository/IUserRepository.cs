namespace Hirundo.Model.Repositories.UserRepository
{
    using System.Collections.Generic;
    using Hirundo.Model.Models;
    using MongoDB.Bson;

    public interface IUserRepository
    {
        User GetUser(ObjectId userId);

        User GetByUsername(string username);

        User GetByEmail(string email);

        void AddUser(string fullname, string email, string password, string username);

        void AddFollowing(ObjectId userId, ObjectId followedUserId);

        void DeleteFollowing(ObjectId userId, ObjectId followedUserId);

        void ChangePassword(ObjectId id, string newPassword);

        long GetFollowersCount(ObjectId userId);

        IEnumerable<User> GetUsers(List<ObjectId> userIds, int take, int skip);

        IEnumerable<User> GetFollowers(ObjectId userId, int take, int skip);

        IEnumerable<User> GetFollowing(ObjectId userId, int take, int skip);
    }
}
