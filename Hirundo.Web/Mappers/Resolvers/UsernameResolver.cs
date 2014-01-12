namespace Hirundo.Web.Mappers.Resolvers
{
    using AutoMapper;
    using Hirundo.Model.Repositories.UserRepository;
    using MongoDB.Bson;

    public class UsernameResolver : ValueResolver<ObjectId, string>
    {
        private IUserRepository userRepository;

        public UsernameResolver(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        protected override string ResolveCore(ObjectId userId)
        {
            return this.userRepository.GetUser(userId).Username;
        }
    }
}