namespace Hirundo.Model.Repositories.CommentRepository
{
    using System.Collections.Generic;
    using Hirundo.Model.Models;
    using MongoDB.Bson;

    public interface ICommentRepository
    {
        long GetCommentsCount(ObjectId userId);

        List<Comment> GetComments(List<ObjectId> userIds, int take, int skip);

        List<Comment> GetComments(ObjectId userId, int take, int skip);

        List<Comment> GetFavorites(ObjectId userId, int take, int skip);

        List<Comment> GetLastComments(ObjectId userId, int count);

        Comment GetComment(ObjectId commentId);

        void DeleteComment(ObjectId commentId);

        void SaveComment(Comment comment);

        void SaveReply(ObjectId commentId, Reply reply);

        void AddRetweet(ObjectId commentId, ObjectId userId);

        void AddFavotite(ObjectId commentId, ObjectId userId);
    }
}
