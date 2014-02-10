namespace Hirundo.Web.Mappers
{
    using AutoMapper;
    using Hirundo.Model.Models;
    using Hirundo.Web.Mappers.Resolvers;
    using Hirundo.Web.Models.Comment;

    public class ReplyMapper : IMapper
    {
        public void CreateMap()
        {
            Mapper.CreateMap<Reply, ReplyDO>()
                .ForMember(rd => rd.Author, m => m.ResolveUsing<UsernameResolver>().FromMember(r => r.Author))
                .ForMember(rd => rd.AuthorId, m => m.MapFrom(r => r.Author))
                .ForMember(rd => rd.AuthorImg, m => m.ResolveUsing<UserImageResolver>().FromMember(r => r.Author))
                .ForMember(rd => rd.Image, m => m.ResolveUsing<ImageResolver>().FromMember(r => r.ImageId));
        }
    }
}