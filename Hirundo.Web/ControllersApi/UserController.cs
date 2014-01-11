namespace Hirundo.Web.ControllersApi
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
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

        private UserContext userContext;
        private IUserRepository userRepository;
        private ICommentRepository commentRepository;
        private IImageRepository imageRepository;

        public UserController(
            IUserContextProvider userContextProvider,
            IUserRepository userRepository,
            IImageRepository imageRepository,
            ICommentRepository commentRepository)
        {
            this.userContext = userContextProvider.GetCurrentUserContext();
            this.userRepository = userRepository;
            this.imageRepository = imageRepository;
            this.commentRepository = commentRepository;
        }

        public HttpResponseMessage GetUser(string userId)
        {
            var currentUser = this.userRepository.GetUser(new ObjectId(this.userContext.UserId));
            var user = this.userRepository.GetUser(new ObjectId(userId));

            var comments = this.commentRepository.GetComments(user.Id, UserComments, 0);
            List<UserCommentDO> userCommentsDO = new List<UserCommentDO>();
            foreach (var comment in comments)
            {
                userCommentsDO.Add(new UserCommentDO
                {
                    Content = comment.Content,
                    PublishDate = comment.PublishDate
                });
            }

            UserDO userDO = new UserDO
            {
                UserId = user.Id,
                Fullname = user.Fullname,
                Username = user.Username,
                Image = this.imageRepository.GetImage(user.ImgId),
                FollowersCount = this.userRepository.GetFollowersCount(user.Id),
                FollowingCount = user.Following.Count,
                IsFollowed = currentUser.Following.Contains(user.Id),
                CommentsCount = this.commentRepository.GetCommentsCount(user.Id),
                Comments = userCommentsDO
            };

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, userDO);
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
                this.GetCommentData(comments, id));
        }

        public HttpResponseMessage GetComments(string userId, int take, int skip = 0)
        {
            ObjectId id = new ObjectId(userId);
            IEnumerable<Comment> comments = this.commentRepository.GetComments(id, take, skip);

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                this.GetCommentData(comments, id));
        }

        public HttpResponseMessage GetFavorites(string userId, int take, int skip = 0)
        {
            ObjectId id = new ObjectId(userId);
            IEnumerable<Comment> comments = this.commentRepository.GetFavorites(id, take, skip);

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                this.GetCommentData(comments, id));
        }

        public HttpResponseMessage GetFollowers(string userId, int take, int skip = 0)
        {
            IEnumerable<User> users = this.userRepository.GetFollowers(new ObjectId(userId), take, skip);

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                this.GetUserData(users));
        }

        public HttpResponseMessage GetFollowing(string userId, int take, int skip = 0)
        {
            IEnumerable<User> users = this.userRepository.GetFollowing(new ObjectId(userId), take, skip);

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                this.GetUserData(users));
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

        private List<UserDO> GetUserData(IEnumerable<User> users)
        {
            var currentUser = this.userRepository.GetUser(new ObjectId(this.userContext.UserId));

            List<UserDO> userDOs = new List<UserDO>();
            foreach (var user in users)
            {
                UserDO userDO = new UserDO
                {
                    UserId = user.Id,
                    Fullname = user.Fullname,
                    Username = user.Username,
                    Image = this.imageRepository.GetImage(user.ImgId),
                    FollowersCount = this.userRepository.GetFollowersCount(user.Id),
                    IsFollowed = currentUser.Following.Contains(user.Id)
                };

                userDOs.Add(userDO);
            }

            return userDOs;
        }

        private List<CommentDataDO> GetCommentData(IEnumerable<Comment> comments, ObjectId userId)
        {
            List<CommentDataDO> commentsData = new List<CommentDataDO>();
            foreach (var comment in comments)
            {
                CommentDataDO commentData = new CommentDataDO();
                commentData.CommentId = comment.Id;
                commentData.Content = comment.Content;
                commentData.PublishDate = comment.PublishDate;
                commentData.AuthorId = comment.Author;
                commentData.IsRetweeted = comment.RetweetedBy.Contains(userId);
                commentData.IsFavorited = comment.FavoritedBy.Contains(userId);

                var author = this.userRepository.GetUser(comment.Author);
                commentData.Author = author.Username;
                commentData.AuthorImg = this.imageRepository.GetImage(author.ImgId);

                commentsData.Add(commentData);
            }

            return commentsData;
        }
    }
}
