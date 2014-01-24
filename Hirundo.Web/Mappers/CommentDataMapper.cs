namespace Hirundo.Web.Mappers
{
    using AutoMapper;
    using Hirundo.Model.Models;
    using Hirundo.Web.Mappers.Resolvers;
    using Hirundo.Web.Models.Comment;

    public class CommentDataMapper : IMapper
    {
        public void CreateMap()
        {
            Mapper.CreateMap<Comment, CommentDataDO>()
                .ForMember(cd => cd.CommentId, m => m.MapFrom(c => c.Id))
                .ForMember(cd => cd.Author, m => m.ResolveUsing<UsernameResolver>().FromMember(c => c.Author))
                .ForMember(cd => cd.AuthorId, m => m.MapFrom(c => c.Author))
                .ForMember(cd => cd.AuthorImg, m => m.ResolveUsing<UserImageResolver>().FromMember(c => c.Author))
                .ForMember(cd => cd.IsShared, m => m.ResolveUsing<UserInListResolver>().FromMember(c => c.SharedBy))
                .ForMember(cd => cd.IsFavorited, m => m.ResolveUsing<UserInListResolver>().FromMember(c => c.FavoritedBy));
        }
    }
}