﻿namespace Hirundo.Web.Controllers
{
    using System.Web.Mvc;
    using Hirundo.Model.Infrastructure;
    using Hirundo.Web.Models;

    public class HomeController : Controller
    {
        private UserContext userContext;
        private const int ItemsToTake = 20;

        public HomeController(IUserContextProvider userContextProvider)
        {
            this.userContext = userContextProvider.GetCurrentUserContext();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new HomeModel
            {
                UserId = this.userContext.UserId,
                Username = this.userContext.Username,
                ItemsToTake = ItemsToTake
            };

            return this.View("Index", model);
        }
    }
}
