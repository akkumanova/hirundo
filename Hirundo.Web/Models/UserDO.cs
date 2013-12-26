namespace Hirundo.Web.Models
{
    public class UserDO
    {
        public string UserId { get; set; }

        public string Fullname { get; set; }

        public string Image { get; set; }

        public long FollowersCount { get; set; }

        public long FollowingCount { get; set; }
    }
}