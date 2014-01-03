using System.Collections.Generic;
using MongoDB.Bson;

namespace Hirundo.Web.Models.User
{
    public class UserDO
    {
        public UserDO()
        {
            this.Comments = new List<UserCommentDO>();
        }

        public ObjectId UserId { get; set; }

        public string Fullname { get; set; }

        public string Username { get; set; }

        public string Image { get; set; }

        public long FollowersCount { get; set; }

        public long FollowingCount { get; set; }

        public bool IsFollowed { get; set; }

        public long CommentsCount { get; set; }

        public List<UserCommentDO> Comments { get; set; }
    }
}