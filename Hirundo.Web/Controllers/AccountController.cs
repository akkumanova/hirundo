namespace Hirundo.Web.Controllers
{
    using System.Net.Mail;
    using System.Web.Mvc;
    using Hirundo.Model.Helpers;
    using Hirundo.Model.Infrastructure;
    using Hirundo.Model.Models;
    using Hirundo.Model.Repositories.UserRepository;

    public class AccountController : Controller
    {
        private static int passwordLength = 7;

        private IUserRepository userRepository;
        private IUserContextProvider userContextProvider;

        public AccountController(IUserRepository userRepository, IUserContextProvider userContextProvider)
        {
            this.userRepository = userRepository;
            this.userContextProvider = userContextProvider;
        }

        [HttpGet]
        public ActionResult Login()
        {
            this.userContextProvider.ClearCurrentUserContext();

            return this.View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            this.userContextProvider.ClearCurrentUserContext();

            string referrer;
            if (Request.UrlReferrer != null)
            {
                referrer = Request.UrlReferrer.PathAndQuery.ToString();
            }
            else
            {
                referrer = "~/";
            }

            return this.Redirect(referrer);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Login(string username, string password)
        {
            return this.GetLoginResult(this.TrySetUserContext(username, password), "Invalid username or password.");
        }

        private bool TrySetUserContext(string username, string password)
        {
            User user = this.userRepository.GetByUsername(username);

            if (user != null && user.VerifyPassword(password))
            {
                this.userContextProvider.SetCurrentUserContext(new UserContext(user));
                return true;
            }
            else
            {
                return false;
            }
        }

        private ActionResult GetLoginResult(bool loginSuccessful, string errorMessage)
        {
            if (loginSuccessful)
            {
                string returnUrl = Request.QueryString["ReturnUrl"];
                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    returnUrl = "~/";
                }

                return this.Redirect(returnUrl);
            }
            else
            {
                ViewBag.ErrorMessage = errorMessage;
                return this.View("Login");
            }
        }

        [HttpGet]
        public ActionResult ForgottenPassword()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ForgottenPassword(string email, string username)
        {
            User user;

            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(username))
            {
                ViewBag.ErrorMessage = "Specify something!";
                return this.View();
            }

            if (!string.IsNullOrEmpty(username))
            {
                user = this.userRepository.GetByUsername(username);

                if (!string.IsNullOrEmpty(email) && user != null && user.Email != email)
                {
                    ViewBag.ErrorMessage = "The specified user has different email!";
                    return this.View();
                }
            }
            else
            {
                user = this.userRepository.GetByEmail(email);
            }

            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid username or email!";
                return this.View();
            }

            string newPassword = Password.CreateRandom(passwordLength);
            this.userRepository.ChangePassword(user.Id, newPassword);

            this.SendEmail(user, newPassword);

            return this.View("Login");
        }

        /// <summary> Send eMail to user with new password</summary>
        /// <param name="user"> user db account</param>
        /// <param name="newPassword"> new password</param>
        private void SendEmail(User user, string newPassword)
        {
            SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Host = "smtp.gmail.com"; // Host is gmail
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("hirundooo@gmail.com", "alabala123");
            smtp.EnableSsl = true;

            MailMessage message = new System.Net.Mail.MailMessage();
            message.To.Add(user.Email);
            message.Subject = "Forgotren Password Change - Hirundo Acount";
            message.From = new System.Net.Mail.MailAddress("hirundooo@gmail.com");
            message.Body = "Dear stupid " + user.Fullname + " !\nYour new password is " + newPassword;

            // send message
            try
            {
                smtp.Send(message);
            }
            catch (SmtpException ex)
            {
                Response.Write(ex.Message);
            }
            finally
            { // Clean up.
                message.Dispose();
            }
        }

        [HttpGet]
        public ActionResult Signup()
        {
            return this.View();
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Signup(string fullname, string email, string password, string username)
        {
            this.userRepository.AddUser(fullname, email, password, username);

            return this.View("Login");
        }
    }
}
