namespace Hirundo.Web.Models.User
{
    public class PasswordDO
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string VerifyPassword { get; set; }
    }
}