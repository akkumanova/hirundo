namespace Hirundo.Web.ControllersApi
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using AutoMapper;
    using Hirundo.Model.Infrastructure;
    using Hirundo.Model.Models;
    using Hirundo.Model.Repositories.CommentRepository;
    using Hirundo.Model.Repositories.ImagesRepository;
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
        private IImageRepository imageRepository;

        public UserController(
            IUserContextProvider userContextProvider,
            IUserRepository userRepository,
            ICommentRepository commentRepository,
            IImageRepository imageRepository)
        {
            this.userContext = userContextProvider.GetCurrentUserContext();
            this.userRepository = userRepository;
            this.commentRepository = commentRepository;
            this.imageRepository = imageRepository;
        }

        public HttpResponseMessage GetUser(string userId)
        {
            var user = this.userRepository.GetUser(new ObjectId(userId));

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                Mapper.Map<User, UserDO>(user));
        }

        public HttpResponseMessage PostUser(UserProfileDO user)
        {
            var currentUserId = new ObjectId(this.userContext.UserId);
            if (currentUserId != user.UserId)
            {
                throw new Exception("Cannot edit other user's profile!");
            }

            ObjectId? imageId = null;
            if (user.Image != null)
            {
                var currentUser = this.userRepository.GetUser(currentUserId);
                this.imageRepository.RemoveImage(currentUser.ImgId);

                imageId = this.imageRepository.SaveImage(user.Image);
            }

            this.userRepository.UpdateUser(
                currentUserId,
                user.Fullname,
                user.Location,
                user.Website,
                user.Bio,
                imageId);

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage PostPassowrd(PasswordDO passwords)
        {
            if (passwords.NewPassword != passwords.VerifyPassword)
            {
                throw new Exception("Passwords doesn't match!");
            }

            ObjectId userId = new ObjectId(this.userContext.UserId);
            var user = this.userRepository.GetUser(userId);

            if (!user.VerifyPassword(passwords.OldPassword))
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Password invalid!");
            }

            this.userRepository.ChangePassword(userId, passwords.NewPassword);

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK);
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

        public HttpResponseMessage GetProfile()
        {
            var user = this.userRepository.GetUser(new ObjectId(this.userContext.UserId));

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                Mapper.Map<User, UserProfileDO>(user));
        }

        public HttpResponseMessage GetTimeline(string userId, int take = 0, int skip = 0, string takeToId = null)
        {
            ObjectId id = new ObjectId(userId);
            User currentUser = this.userRepository.GetUser(id);

            var userIds = currentUser.Following;
            userIds.Add(id);
            IEnumerable<Comment> comments = takeToId != null ?
                this.commentRepository.GetComments(userIds, new ObjectId(takeToId)) :
                this.commentRepository.GetComments(userIds, take, skip);

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
