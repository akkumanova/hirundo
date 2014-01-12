namespace Hirundo.Web.Mappers.Resolvers
{
    using AutoMapper;
    using Hirundo.Model.Infrastructure;
    using Hirundo.Model.Repositories.UserRepository;
    using MongoDB.Bson;

    public class FollowedResolver : ValueResolver<ObjectId, bool>
    {
        private UserContext userContext;
        private IUserRepository userRepository;

        public FollowedResolver(IUserContextProvider userContextProvider, IUserRepository userRepository)
        {
            this.userContext = userContextProvider.GetCurrentUserContext();
            this.userRepository = userRepository;

        }

        protected override bool ResolveCore(ObjectId userId)
        {
            var currentUser = this.userRepository.GetUser(new ObjectId(this.userContext.UserId));

            return currentUser.Following.Contains(userId);
        }
    }
}