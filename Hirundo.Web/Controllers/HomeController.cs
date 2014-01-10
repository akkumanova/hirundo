namespace Hirundo.Web.Controllers
{
    using System.Web.Mvc;
    using Hirundo.Model.Infrastructure;
    using Hirundo.Web.Models;
    using Hirundo.Model.Repositories.UserRepository;
using Hirundo.Model.Repositories.ImagesRepository;
    using MongoDB.Bson;

    public class HomeController : Controller
    {
        private UserContext userContext;
        private IUserRepository userRepository;
        private IImageRepository imageRepository;
        private const int ItemsToTake = 20;

        public HomeController(
            IUserContextProvider userContextProvider,
            IUserRepository userRepository,
            IImageRepository imageRepository)
        {
            this.userContext = userContextProvider.GetCurrentUserContext();
            this.userRepository = userRepository;
            this.imageRepository = imageRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var imageId = this.userRepository.FindById(new ObjectId(this.userContext.UserId)).ImgId;

            var model = new HomeModel
            {
                UserId = this.userContext.UserId,
                Username = this.userContext.Username,
                UserImage = this.imageRepository.GetImage(imageId),
                ItemsToTake = ItemsToTake
            };

            return this.View("Index", model);
        }
    }
}
