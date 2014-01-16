namespace Hirundo.Web.ControllersApi
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using AutoMapper;
    using Hirundo.Model.Infrastructure;
    using Hirundo.Model.Models;
    using Hirundo.Model.Repositories.CommentRepository;
    using Hirundo.Model.Repositories.UserRepository;
    using Hirundo.Web.Models.Comment;
    using Hirundo.Web.Models.User;
    using MongoDB.Bson;

    public class UserController : ApiController
    {
        private const int UserComments = 2;
        private const int MinUsers = 4;

        private UserContext userContext;
        private IUserRepository userRepository;
        private ICommentRepository commentRepository;

        public UserController(
            IUserContextProvider userContextProvider,
            IUserRepository userRepository,
            ICommentRepository commentRepository)
        {
            this.userContext = userContextProvider.GetCurrentUserContext();
            this.userRepository = userRepository;
            this.commentRepository = commentRepository;
        }

        public HttpResponseMessage GetUser(string userId)
        {
            var user = this.userRepository.GetUser(new ObjectId(userId));

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                Mapper.Map<User, UserDO>(user));
        }

        public HttpResponseMessage GetUserExists(string username = null, string email = null)
        {
            bool userExists = false;

            if (!string.IsNullOrWhiteSpace(username))
            {
                var user = this.userRepository.GetByUsername(username);
                userExists = userExists || user != null;
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                var user = this.userRepository.GetByEmail(email);
                userExists = userExists || user != null;
            }

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, userExists);
        }

        public HttpResponseMessage GetTimeline(string userId, int take, int skip = 0)
        {
            ObjectId id = new ObjectId(userId);
            User currentUser = this.userRepository.GetUser(id);

            var userIds = currentUser.Following;
            userIds.Add(id);
            IEnumerable<Comment> comments = this.commentRepository.GetComments(userIds, take, skip);

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                Mapper.Map<IEnumerable<Comment>, List<CommentDataDO>>(comments));
        }

        public HttpResponseMessage GetComments(string userId, int take, int skip = 0)
        {
            ObjectId id = new ObjectId(userId);
            IEnumerable<Comment> comments = this.commentRepository.GetComments(id, take, skip);

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                 Mapper.Map<IEnumerable<Comment>, List<CommentDataDO>>(comments));
        }

        public HttpResponseMessage GetFavorites(string userId, int take, int skip = 0)
        {
            ObjectId id = new ObjectId(userId);
            IEnumerable<Comment> comments = this.commentRepository.GetFavorites(id, take, skip);

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                 Mapper.Map<IEnumerable<Comment>, List<CommentDataDO>>(comments));
        }

        public HttpResponseMessage GetFollowers(string userId, int take, int skip = 0)
        {
            IEnumerable<User> users = this.userRepository.GetFollowers(new ObjectId(userId), take, skip);

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                Mapper.Map<IEnumerable<User>, List<UserDataDO>>(users));
        }

        public HttpResponseMessage GetFollowing(string userId, int take, int skip = 0)
        {
            IEnumerable<User> users = this.userRepository.GetFollowing(new ObjectId(userId), take, skip);

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                Mapper.Map<IEnumerable<User>, List<UserDataDO>>(users));
        }

        public HttpResponseMessage PostFollowing(string userId)
        {
            this.userRepository.AddFollowing(
                new ObjectId(this.userContext.UserId),
                new ObjectId(userId));

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage DeleteFollowing(string userId)
        {
            this.userRepository.DeleteFollowing(
                new ObjectId(this.userContext.UserId),
                new ObjectId(userId));

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage GetUsers(string username, int? take = null, int skip = 0)
        {
            if (!take.HasValue)
            {
                take = MinUsers;
            }

            var users = this.userRepository.GetUsers(username, take.Value, skip);
            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                Mapper.Map<IEnumerable<User>, List<UserDataDO>>(users));
        }
    }
}
