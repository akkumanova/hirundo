namespace Hirundo.Web.Mappers
{
    using AutoMapper;
    using Hirundo.Model.Models;
    using Hirundo.Web.Mappers.Resolvers;
    using Hirundo.Web.Models.User;

    public class UserProfileMapper : IMapper
    {
        public void CreateMap()
        {
            Mapper.CreateMap<User, UserProfileDO>()
                .ForMember(ud => ud.UserId, m => m.MapFrom(u => u.Id))
                .ForMember(ud => ud.Image, m => m.ResolveUsing<ImageResolver>().FromMember(u => u.ImgId));
        }
    }
}