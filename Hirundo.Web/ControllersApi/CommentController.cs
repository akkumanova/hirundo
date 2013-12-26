using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Hirundo.Model.Infrastructure;
using Hirundo.Model.Models;
using Hirundo.Model.Repositories.CommentRepository;
using Hirundo.Web.Models;
using MongoDB.Bson;

namespace Hirundo.Web.ControllersApi
{
    public class CommentController : ApiController
    {
        private UserContext userContext;
        private ICommentRepository commentRepository;

        public CommentController(IUserContextProvider userContextProvider, ICommentRepository commentRepository)
        {
            this.userContext = userContextProvider.GetCurrentUserContext();
            this.commentRepository = commentRepository;
        }

        public HttpResponseMessage GetComments(string userId)
        {
            long commentsCount = this.commentRepository.GetCommentsCount(new ObjectId(userId));

            UserCommentsDO userComments = new UserCommentsDO
            {
                Comments = new List<Comment>(),
                Count = commentsCount
            };

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, userComments);
        }

        public HttpResponseMessage PostComment(Comment comment)
        {
            this.commentRepository.SaveComment(comment);
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
