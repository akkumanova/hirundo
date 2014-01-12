namespace Hirundo.Web.Mappers
{
    using AutoMapper;
    using Hirundo.Model.Models;
    using Hirundo.Web.Mappers.Resolvers;
    using Hirundo.Web.Models.User;

    public class UserMapper : IMapper
    {
        public void CreateMap()
        {
            Mapper.CreateMap<User, UserDO>()
                .ForMember(ud => ud.UserId, m => m.MapFrom(u => u.Id))
                .ForMember(ud => ud.Image, m => m.ResolveUsing<ImageResolver>().FromMember(u => u.ImgId))
                .ForMember(ud => ud.FollowersCount, m => m.ResolveUsing<FollowersResolver>().FromMember(u => u.Id))
                .ForMember(ud => ud.FollowingCount, m => m.MapFrom(u => u.Following.Count))
                .ForMember(ud => ud.IsFollowed, m => m.ResolveUsing<FollowedResolver>().FromMember(u => u.Id))
                .ForMember(ud => ud.CommentsCount, m => m.ResolveUsing<CommentsCountResolver>().FromMember(u => u.Id));
        }
    }
}