using System.Collections.Generic;
using Hirundo.Model.Models;
using MongoDB.Bson;

namespace Hirundo.Model.Repositories.CommentRepository
{
    public interface ICommentRepository
    {
        long GetCommentsCount(ObjectId userId);

        List<Comment> GetComments(List<ObjectId> userIds, int skip);

        Comment GetComment(ObjectId commentId);

        void SaveComment(Comment comment);

        void SaveReply(ObjectId commentId, Reply reply);
    }
}
