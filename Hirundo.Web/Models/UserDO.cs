using MongoDB.Bson;

namespace Hirundo.Web.Models
{
    public class UserDO
    {
        public ObjectId UserId { get; set; }

        public string Fullname { get; set; }

        public string Image { get; set; }

        public long FollowersCount { get; set; }

        public long FollowingCount { get; set; }

        public long CommentsCount { get; set; }
    }
}