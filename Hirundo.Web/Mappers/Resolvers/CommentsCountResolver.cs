namespace Hirundo.Web.Mappers.Resolvers
{
    using AutoMapper;
    using Hirundo.Model.Repositories.CommentRepository;
    using MongoDB.Bson;

    public class CommentsCountResolver : ValueResolver<ObjectId, long>
    {
        private ICommentRepository commentRepository;

        public CommentsCountResolver(ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        protected override long ResolveCore(ObjectId userId)
        {
            return this.commentRepository.GetCommentsCount(userId);
        }
    }
}