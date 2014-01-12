namespace Hirundo.Web.Mappers.Resolvers
{
    using AutoMapper;
    using Hirundo.Model.Repositories.ImagesRepository;
    using MongoDB.Bson;

    public class ImageResolver : ValueResolver<ObjectId, string>
    {
        private IImageRepository imageRepository;

        public ImageResolver(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        protected override string ResolveCore(ObjectId imageId)
        {
            return this.imageRepository.GetImage(imageId);
        }
    }
}