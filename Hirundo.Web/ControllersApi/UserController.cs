using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Hirundo.Model.Infrastructure;
using Hirundo.Model.Models;
using Hirundo.Model.Repositories.CommentRepository;
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
        private ICommentRepository commentRepository;
        private IImageRepository imageRepository;

        public UserController(IUserContextProvider userContextProvider,
                              IUserRepository userRepository,
                              IImageRepository imageRepository,
                              ICommentRepository commentRepository)
        {
            this.userContext = userContextProvider.GetCurrentUserContext();
            this.userRepository = userRepository;
            this.imageRepository = imageRepository;
            this.commentRepository = commentRepository;
        }

        public HttpResponseMessage GetUser()
        {
            var currentUserId = new ObjectId(this.userContext.UserId);
            var user = this.userRepository.FindById(currentUserId);

            UserDO userDO = new UserDO
            {
                UserId = user.Id,
                Fullname = user.Fullname,
                Image = this.imageRepository.GetImage(user.ImgId),
                FollowersCount = this.userRepository.GetFollowersCount(currentUserId),
                FollowingCount = user.Following.Count,
                CommentsCount = this.commentRepository.GetCommentsCount(currentUserId)
            };

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, userDO);
        }

        public HttpResponseMessage GetTimeline(string userId, int skip = 0)
        {
            ObjectId id = new ObjectId(userId);
            User currentUser = this.userRepository.FindById(id);

            long commentsCount = this.commentRepository.GetCommentsCount(id);

            var userIds = currentUser.Following;
            userIds.Add(id);
            List<Comment> comments = this.commentRepository.GetComments(userIds, skip);

            List<CommentDO> commentDOs = new List<CommentDO>();
            foreach (var comment in comments)
            {
                CommentDO commentDO = new CommentDO();
                commentDO.Content = comment.Content;
                commentDO.PublishDate = comment.PublishDate;
                commentDO.AuthorId = comment.Author;

                var author = this.userRepository.FindById(comment.Author);
                commentDO.Author = author.Username;
                commentDO.AuthorImg = this.imageRepository.GetImage(author.ImgId);

                commentDOs.Add(commentDO);
            }

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, commentDOs);
        }
    }
}
