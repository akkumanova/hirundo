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
using MongoDB.Bson;

namespace Hirundo.Web.ControllersApi
{
    public class CommentController : ApiController
    {
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

        public HttpResponseMessage PostComment(Comment comment)
        {
            this.commentRepository.SaveComment(comment);
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage DeleteComment(string commentId)
        {
            this.commentRepository.DeleteComment(new ObjectId(commentId));
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage PostReply(Reply reply, string commentId)
        {
            ObjectId id = new ObjectId(commentId);
            this.commentRepository.SaveReply(id, reply);

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                this.GetCommentDetails(id));
        }

        public HttpResponseMessage GetCommentDetails(string commentId)
        {
            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                this.GetCommentDetails(new ObjectId(commentId)));
        }

        public HttpResponseMessage PostRetweet(string commentId)
        {
            var id = new ObjectId(commentId);

            this.commentRepository.AddRetweet(
                id,
                new ObjectId(this.userContext.UserId));

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                this.GetCommentDetails(id));
        }

        public HttpResponseMessage PostFavorite(string commentId)
        {
            var id = new ObjectId(commentId);

            this.commentRepository.AddFavotite(
                id,
                new ObjectId(this.userContext.UserId));

            return ControllerContext.Request.CreateResponse(
                HttpStatusCode.OK,
                this.GetCommentDetails(id));
        }

        private CommentDetailsDO GetCommentDetails(ObjectId commentId)
        {
            var comment = this.commentRepository.GetComment(commentId);

            CommentDetailsDO commentDetails = new CommentDetailsDO();
            commentDetails.CommentId = comment.Id;
            commentDetails.Favorites = comment.FavoritedBy.Count;
            commentDetails.Retweets = comment.RetweetedBy.Count;

            var repliesToDisplay = comment.Replies.OrderByDescending(r => r.PublishDate).Take(2);
            foreach (var reply in repliesToDisplay)
            {
                ReplyDO replyDO = new ReplyDO();
                replyDO.AuthorId = reply.Author;
                replyDO.Content = reply.Content;
                replyDO.PublishDate = reply.PublishDate;

                var replyAuthor = this.userRepository.FindById(reply.Author);
                replyDO.Author = replyAuthor.Username;
                replyDO.AuthorImg = this.imageRepository.GetImage(replyAuthor.ImgId);

                commentDetails.Replies.Add(replyDO);
            }

            return commentDetails;
        }
    }
}
