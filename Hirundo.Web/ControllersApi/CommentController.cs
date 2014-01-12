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

    public class CommentController : ApiController
    {
        private const int Users = 25;
        private const int MaxReplies = 20;
        private const int MinReplies = 2;

        private UserContext userContext;
        private ICommentRepository commentRepository;
        private IUserRepository userRepository;

        public CommentController(
            IUserContextProvider userContextProvider,
            ICommentRepository commentRepository,
            IUserRepository userRepository)
        {
            this.userContext = userContextProvider.GetCurrentUserContext();
            this.commentRepository = commentRepository;
            this.userRepository = userRepository;
        }

        public HttpResponseMessage GetComment(string commentId)
        {
            Comment comment = this.commentRepository.GetComment(new ObjectId(commentId), MaxReplies);

            CommentDO commentDO = new CommentDO
            {
                CommentData = Mapper.Map<Comment, CommentDataDO>(comment),
                CommentDetails = Mapper.Map<Comment, CommentDetailsDO>(comment)
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

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                Mapper.Map<IEnumerable<Reply>, List<ReplyDO>>(replies));
        }

        public HttpResponseMessage PostReply(Reply reply, string commentId)
        {
            ObjectId id = new ObjectId(commentId);
            this.commentRepository.AddReply(id, reply);
            Comment comment = this.commentRepository.GetComment(id, MinReplies);

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                Mapper.Map<Comment, CommentDetailsDO>(comment));
        }

        public HttpResponseMessage GetCommentDetails(string commentId)
        {
            Comment comment = this.commentRepository.GetComment(new ObjectId(commentId), MinReplies);

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                Mapper.Map<Comment, CommentDetailsDO>(comment));
        }

        public HttpResponseMessage PostRetweet(string commentId)
        {
            ObjectId id = new ObjectId(commentId);

            this.commentRepository.AddRetweet(
                id,
                new ObjectId(this.userContext.UserId));
            Comment comment = this.commentRepository.GetComment(new ObjectId(commentId), MinReplies);

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                Mapper.Map<Comment, CommentDetailsDO>(comment));
        }

        public HttpResponseMessage GetRetweets(string commentId)
        {
            var userIds = this.commentRepository.GetComment(new ObjectId(commentId)).RetweetedBy;
            var users = this.userRepository.GetUsers(userIds, Users, 0);

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                Mapper.Map<IEnumerable<User>, List<UserDO>>(users));
        }

        public HttpResponseMessage PostFavorite(string commentId)
        {
            ObjectId id = new ObjectId(commentId);

            this.commentRepository.AddFavotite(
                id,
                new ObjectId(this.userContext.UserId));
            Comment comment = this.commentRepository.GetComment(id, MinReplies);

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                Mapper.Map<Comment, CommentDetailsDO>(comment));
        }

        public HttpResponseMessage GetFavorites(string commentId)
        {
            var userIds = this.commentRepository.GetComment(new ObjectId(commentId)).FavoritedBy;
            var users = this.userRepository.GetUsers(userIds, Users, 0);

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                Mapper.Map<IEnumerable<User>, List<UserDO>>(users));
        }
    }
}
