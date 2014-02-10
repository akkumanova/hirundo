namespace Hirundo.Model.Repositories.CommentRepository
{
    using System;
using System.Collections.Generic;
using Hirundo.Model.Models;
using MongoDB.Bson;

    public interface ICommentRepository
    {
        Comment GetComment(ObjectId commentId, int replies = 0);

        void AddComment(ObjectId authorId, string content, DateTime publishDate, string location, ObjectId? imageId);

        void DeleteComment(ObjectId commentId);

        void AddReply(ObjectId commentId, ObjectId authorId, string content, DateTime publishDate, string location, ObjectId? imageId);

        void AddSharing(ObjectId commentId, ObjectId userId);

        void AddFavotite(ObjectId commentId, ObjectId userId);

        long GetCommentsCount(ObjectId userId);

        IEnumerable<Comment> GetComments(List<ObjectId> userIds, int take, int skip);

        IEnumerable<Comment> GetComments(List<ObjectId> userIds, ObjectId takeToId);

        IEnumerable<Comment> GetComments(ObjectId userId, int take, int skip);

        IEnumerable<Comment> GetFavorites(ObjectId userId, int take, int skip);

        IEnumerable<Reply> GetReplies(ObjectId commentId, int take, int skip);
    }
}
