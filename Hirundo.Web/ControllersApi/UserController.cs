using System.Net;
using System.Net.Http;
using System.Web.Http;
using Hirundo.Model.Infrastructure;
using Hirundo.Model.Models;
using Hirundo.Model.Repositories.ImagesRepository;
using Hirundo.Model.Repositories.UserRepository;
using Hirundo.Web.Models;
using MongoDB.Bson;

namespace Hirundo.Web.ControllersApi
{
    public class UserController : ApiController
    {
        private UserContext userContext;
        private IUserRepository userRepository;
        private IImageRepository imageRepository;

        public UserController(IUserContextProvider userContextProvider, IUserRepository userRepository, IImageRepository imageRepository)
        {
            this.userContext = userContextProvider.GetCurrentUserContext();
            this.userRepository = userRepository;
            this.imageRepository = imageRepository;
        }

        public HttpResponseMessage GetUser()
        {
            var currentUserId = new ObjectId(this.userContext.UserId);
            var user = this.userRepository.FindById(currentUserId);

            UserDO userDO = new UserDO
            {
                UserId = user.Id.ToString(),
                Fullname = user.Fullname,
                Image = this.imageRepository.GetImage(user.ImgId),
                FollowersCount = this.userRepository.GetFollowersCount(currentUserId),
                FollowingCount = user.Following.Count
            };

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, userDO);
        }
    }
}
