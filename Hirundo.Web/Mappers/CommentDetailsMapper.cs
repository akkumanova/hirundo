﻿namespace Hirundo.Web.Mappers
{
    using AutoMapper;
    using Hirundo.Model.Models;
    using Hirundo.Web.Models.Comment;

    public class CommentDetailsMapper : IMapper
    {
        public void CreateMap()
        {
            Mapper.CreateMap<Comment, CommentDetailsDO>()
                .ForMember(cd => cd.CommentId, m => m.MapFrom(c => c.Id))
                .ForMember(cd => cd.Retweets, m => m.MapFrom(c => c.RetweetedBy.Count))
                .ForMember(cd => cd.Favorites, m => m.MapFrom(c => c.FavoritedBy.Count));
        }
    }
}