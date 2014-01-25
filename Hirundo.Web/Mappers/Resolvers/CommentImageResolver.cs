namespace Hirundo.Web.Mappers.Resolvers
{
    using AutoMapper;
    using Hirundo.Model.Repositories.ImagesRepository;
    using Hirundo.Model.Repositories.CommentRepository;
    using MongoDB.Bson;

    public class CommentImageResolver : ValueResolver<ObjectId, string>
    {
        private ICommentRepository commentRepository;
        private IImageRepository imageRepository;

        public CommentImageResolver(ICommentRepository commentRepository, IImageRepository imageRepository)
        {
            this.commentRepository = commentRepository;
            this.imageRepository = imageRepository;
        }

        protected override string ResolveCore(ObjectId commentId)
        {
            ObjectId imageId = this.commentRepository.GetComment(commentId).ImgId;
            if (imageId.Increment != 0)
            {
                return this.imageRepository.GetImage(imageId);
            }
            return null;
        }
    }
}