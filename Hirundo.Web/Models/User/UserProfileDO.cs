namespace Hirundo.Web.Models.User
{
    using MongoDB.Bson;

    public class UserProfileDO
    {
        public ObjectId UserId { get; set; }

        public string Fullname { get; set; }

        public string Image { get; set; }

        public string Location { get; set; }

        public string Website { get; set; }

        public string Bio { get; set; }
    }
}