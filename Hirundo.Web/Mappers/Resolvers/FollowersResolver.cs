namespace Hirundo.Web.Mappers.Resolvers
{
    using AutoMapper;
    using Hirundo.Model.Repositories.UserRepository;
    using MongoDB.Bson;

    public class FollowersResolver : ValueResolver<ObjectId, long>
    {
        private IUserRepository userRepository;

        public FollowersResolver(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        protected override long ResolveCore(ObjectId userId)
        {
            return this.userRepository.GetFollowersCount(userId);
        }
    }
}