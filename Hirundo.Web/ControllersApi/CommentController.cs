namespace Hirundo.Web.ControllersApi
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Hirundo.Model.Infrastructure;
    using Hirundo.Model.Models;
    using Hirundo.Model.Repositories.CommentRepository;
    using Hirundo.Model.Repositories.ImagesRepository;
    using Hirundo.Model.Repositories.UserRepository;
    using Hirundo.Web.Models;
    using Hirundo.Web.Models.Comment;
    using Hirundo.Web.Models.User;
    using MongoDB.Bson;

    public class CommentController : ApiController
    {
        private const int Users = 25;
        private const int Replies = 20;

        private UserContext userContext;
        private ICommentRepository commentRepository;
        private IUserRepository userRepository;
        private IImageRepository imageRepository;

        public CommentController(
            IUserContextProvider userContextProvider,
            ICommentRepository commentRepository,
            IUserRepository userRepository,
            IImageRepository imageRepository)
        {
            this.userContext = userContextProvider.GetCurrentUserContext();
            this.commentRepository = commentRepository;
            this.userRepository = userRepository;
            this.imageRepository = imageRepository;
        }

        public HttpResponseMessage GetComment(string commentId)
        {
            Comment comment = this.commentRepository.GetComment(new ObjectId(commentId));
            ObjectId currentUserId = new ObjectId(this.userContext.UserId);
            var author = this.userRepository.GetUser(comment.Author);

            CommentDataDO commentDataDO = new CommentDataDO
            {
                CommentId = comment.Id,
                Author = author.Username,
                AuthorId = comment.Author,
                AuthorImg = this.imageRepository.GetImage(author.ImgId),
                Content = comment.Content,
                PublishDate = comment.PublishDate,
                IsRetweeted = comment.RetweetedBy.Contains(currentUserId),
                IsFavorited = comment.FavoritedBy.Contains(currentUserId)
            };

            CommentDO commentDO = new CommentDO
            {
                CommentData = commentDataDO,
                CommentDetails = this.GetCommentDetails(comment, Replies)
            };

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, commentDO);
        }

        public HttpResponseMessage PostComment(Comment comment)
        {
            this.commentRepository.AddComment(comment);
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage DeleteComment(string commentId)
        {
            this.commentRepository.DeleteComment(new ObjectId(commentId));
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage GetReplies(string commentId, int take, int skip = 0)
        {
            var replies = this.commentRepository.GetReplies(new ObjectId(commentId), take, skip);

            List<ReplyDO> repliesDO = new List<ReplyDO>();
            foreach (var reply in replies)
            {
                ReplyDO replyDO = new ReplyDO();
                replyDO.AuthorId = reply.Author;
                replyDO.Content = reply.Content;
                replyDO.PublishDate = reply.PublishDate;

                var replyAuthor = this.userRepository.GetUser(reply.Author);
                replyDO.Author = replyAuthor.Username;
                replyDO.AuthorImg = this.imageRepository.GetImage(replyAuthor.ImgId);

                repliesDO.Add(replyDO);
            }

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, repliesDO);
        }

        public HttpResponseMessage PostReply(Reply reply, string commentId)
        {
            ObjectId id = new ObjectId(commentId);
            this.commentRepository.AddReply(id, reply);
            Comment comment = this.commentRepository.GetComment(id);

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                this.GetCommentDetails(comment, 2));
        }

        public HttpResponseMessage GetCommentDetails(string commentId)
        {
            Comment comment = this.commentRepository.GetComment(new ObjectId(commentId));

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                this.GetCommentDetails(comment, 2));
        }

        public HttpResponseMessage PostRetweet(string commentId)
        {
            ObjectId id = new ObjectId(commentId);

            this.commentRepository.AddRetweet(
                id,
                new ObjectId(this.userContext.UserId));
            Comment comment = this.commentRepository.GetComment(new ObjectId(commentId));

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                this.GetCommentDetails(comment, 2));
        }

        public HttpResponseMessage GetRetweets(string commentId)
        {
            var userIds = this.commentRepository.GetComment(new ObjectId(commentId)).RetweetedBy;
            var users = this.userRepository.GetUsers(userIds, Users, 0);

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

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, userDOs);
        }

        public HttpResponseMessage PostFavorite(string commentId)
        {
            ObjectId id = new ObjectId(commentId);

            this.commentRepository.AddFavotite(
                id,
                new ObjectId(this.userContext.UserId));
            Comment comment = this.commentRepository.GetComment(id);

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                this.GetCommentDetails(comment, 2));
        }

        public HttpResponseMessage GetFavorites(string commentId)
        {
            var userIds = this.commentRepository.GetComment(new ObjectId(commentId)).FavoritedBy;
            var users = this.userRepository.GetUsers(userIds, Users, 0);

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

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, userDOs);
        }

        private CommentDetailsDO GetCommentDetails(Comment comment, int replies)
        {
            CommentDetailsDO commentDetails = new CommentDetailsDO();
            commentDetails.CommentId = comment.Id;
            commentDetails.Favorites = comment.FavoritedBy.Count;
            commentDetails.Retweets = comment.RetweetedBy.Count;

            var repliesToDisplay = comment.Replies.OrderByDescending(r => r.PublishDate).Take(replies);
            foreach (var reply in repliesToDisplay)
            {
                ReplyDO replyDO = new ReplyDO();
                replyDO.AuthorId = reply.Author;
                replyDO.Content = reply.Content;
                replyDO.PublishDate = reply.PublishDate;

                var replyAuthor = this.userRepository.GetUser(reply.Author);
                replyDO.Author = replyAuthor.Username;
                replyDO.AuthorImg = this.imageRepository.GetImage(replyAuthor.ImgId);

                commentDetails.Replies.Add(replyDO);
            }

            return commentDetails;
        }
    }
}
