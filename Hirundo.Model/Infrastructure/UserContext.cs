using Hirundo.Model.Models;

namespace Hirundo.Model.Infrastructure
{
    public class UserContext
    {
        private string userId;
        private string username;

        public UserContext(User user)
        {
            this.userId = user.Id.ToString();
            this.username = user.Username;
        }

        public UserContext(string userId, string username)
        {
            this.userId = userId;
            this.username = username;
        }

        public string UserId
        {
            get
            {
                return this.userId;
            }
        }

        public string Username
        {
            get
            {
                return this.username;
            }
        }
    }
}
