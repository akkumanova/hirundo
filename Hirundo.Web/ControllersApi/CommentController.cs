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
    using Hirundo.Model.Repositories.UserRepository;
    using Hirundo.Model.Repositories.ImagesRepository;
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
            Comment comment = this.commentRepository.GetComment(new ObjectId(commentId), MaxReplies);
            CommentDO commentDO = new CommentDO
            {
                CommentData = Mapper.Map<Comment, CommentDataDO>(comment),
                CommentDetails = Mapper.Map<Comment, CommentDetailsDO>(comment)
            };

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, commentDO);
        }

        public HttpResponseMessage PostComment(NewComment comment)
        {
            var currentUserId = new ObjectId(this.userContext.UserId);
            if (comment.Author != currentUserId)
            {
                throw new Exception("Cannot comment!");
            }

            ObjectId? imageId = null;
            if (comment.Image != null)
            {
                imageId = this.imageRepository.SaveImage(comment.Image);
            }

            this.commentRepository.AddComment(
                comment.Author,
                comment.Author,
                comment.Content,
                comment.PublishDate,
                comment.Location,
                imageId);

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage DeleteComment(string commentId)
        {
            ObjectId id = new ObjectId(commentId);
            var comment = this.commentRepository.GetComment(id);
            if (!comment.ImgId.Equals(ObjectId.Empty))
            {
                this.imageRepository.RemoveImage(comment.ImgId);
            }

            this.commentRepository.DeleteComment(id);

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage GetReplies(string commentId, int take, int skip = 0)
        {
            var replies = this.commentRepository.GetReplies(new ObjectId(commentId), take, skip);

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                Mapper.Map<IEnumerable<Reply>, List<ReplyDO>>(replies));
        }

        public HttpResponseMessage PostReply(NewComment reply, string commentId)
        {
            var currentUserId = new ObjectId(this.userContext.UserId);
            if (currentUserId != reply.Author)
            {
                throw new Exception("Cannot reply!");
            }

            ObjectId id = new ObjectId(commentId);

            ObjectId? imageId = null;
            if (reply.Image != null)
            {
                imageId = this.imageRepository.SaveImage(reply.Image);
            }

            this.commentRepository.AddReply(
                id,
                reply.Author,
                reply.Content,
                reply.PublishDate,
                reply.Location,
                imageId);
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

        public HttpResponseMessage PostSharing(string commentId)
        {
            ObjectId id = new ObjectId(commentId);

            this.commentRepository.AddSharing(
                id,
                new ObjectId(this.userContext.UserId));
            Comment comment = this.commentRepository.GetComment(new ObjectId(commentId), MinReplies);

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                Mapper.Map<Comment, CommentDetailsDO>(comment));
        }

        public HttpResponseMessage GetSharings(string commentId)
        {
            var userIds = this.commentRepository.GetComment(new ObjectId(commentId)).SharedBy;
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
