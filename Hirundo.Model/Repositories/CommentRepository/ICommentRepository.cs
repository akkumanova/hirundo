using Hirundo.Model.Models;
using MongoDB.Bson;

namespace Hirundo.Model.Repositories.CommentRepository
{
    public interface ICommentRepository
    {
        long GetCommentsCount(ObjectId userId);

        void SaveComment(Comment comment);
    }
}
