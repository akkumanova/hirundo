namespace Hirundo.Web.Models.User
{
    using MongoDB.Bson;

    public class UserDataDO
    {
        public ObjectId UserId { get; set; }

        public string Username { get; set; }

        public string Fullname { get; set; }

        public string Image { get; set; }

        public long FollowersCount { get; set; }

        public bool IsFollowed { get; set; }
    }
}