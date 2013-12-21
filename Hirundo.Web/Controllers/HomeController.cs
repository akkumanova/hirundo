using System.Web.Mvc;
using Hirundo.Model.Infrastructure;
using Hirundo.Web.Models;

namespace Hirundo.Web.Controllers
{
    public class HomeController : Controller
    {
        private UserContext userContext;

        public HomeController(IUserContextProvider userContextProvider)
        {
            this.userContext = userContextProvider.GetCurrentUserContext();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new UserModel
            {
                UserId = this.userContext.UserId,
                Username = this.userContext.Username
            };

            return this.View("Index", model);
        }
    }
}
