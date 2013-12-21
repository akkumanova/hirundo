using System.Web.Mvc;
using Hirundo.Model.Infrastructure;
using Hirundo.Model.Models;
using Hirundo.Model.Repositories.UserRepository;

namespace Hirundo.Web.Controllers
{
    public class AccountController : Controller
    {
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
            User user = this.userRepository.FindByUsername(username);

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
    }
}
