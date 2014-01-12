namespace Hirundo.Web.Mappers.Resolvers
{
    using System.Collections.Generic;
    using AutoMapper;
    using Hirundo.Model.Infrastructure;
    using MongoDB.Bson;

    public class UserInListResolver : ValueResolver<List<ObjectId>, bool>
    {
        private UserContext userContext;

        public UserInListResolver(IUserContextProvider userContextProvider)
        {
            this.userContext = userContextProvider.GetCurrentUserContext();
        }

        protected override bool ResolveCore(List<ObjectId> userIds)
        {
            return userIds.Contains(new ObjectId(this.userContext.UserId));
        }

    }
}