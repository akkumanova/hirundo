using System.Collections.Generic;
using Hirundo.Model.Models;
using MongoDB.Bson;

namespace Hirundo.Model.Repositories.CommentRepository
{
    public interface ICommentRepository
    {
        long GetCommentsCount(ObjectId userId);

        List<Comment> GetComments(List<ObjectId> userIds, int skip);

        List<Comment> GetLastComments(ObjectId userId, int count);

        Comment GetComment(ObjectId commentId);

        void DeleteComment(ObjectId commentId);

        void SaveComment(Comment comment);

        void SaveReply(ObjectId commentId, Reply reply);

        void AddRetweet(ObjectId commentId, ObjectId userId);

        void AddFavotite(ObjectId commentId, ObjectId userId);
    }
}
