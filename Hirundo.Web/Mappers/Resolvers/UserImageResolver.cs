namespace Hirundo.Web.Mappers.Resolvers
{
    using AutoMapper;
    using Hirundo.Model.Repositories.ImagesRepository;
    using Hirundo.Model.Repositories.UserRepository;
    using MongoDB.Bson;

    public class UserImageResolver : ValueResolver<ObjectId, string>
    {
        private IUserRepository userRepository;
        private IImageRepository imageRepository;

        public UserImageResolver(IUserRepository userRepository, IImageRepository imageRepository)
        {
            this.userRepository = userRepository;
            this.imageRepository = imageRepository;
        }

        protected override string ResolveCore(ObjectId userId)
        {
            var imageId = this.userRepository.GetUser(userId).ImgId;

            return this.imageRepository.GetImage(imageId);
        }
    }
}