using System.Net;
using System.Net.Http;
using System.Web.Http;
using Hirundo.Model.Infrastructure;
using Hirundo.Model.Models;
using Hirundo.Model.Repositories.CommentRepository;

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

        public HttpResponseMessage PostComment(Comment comment)
        {
            this.commentRepository.SaveComment(comment);
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
