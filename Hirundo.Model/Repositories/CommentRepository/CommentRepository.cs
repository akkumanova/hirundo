using System.Collections.Generic;
using System.Linq;
using Hirundo.Model.Data;
using Hirundo.Model.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace Hirundo.Model.Repositories.CommentRepository
{
    public class CommentRepository : ICommentRepository
    {
        private MongoCollection<Comment> commentCollection;

        public CommentRepository(IMongoContext mongoContext)
        {
            this.commentCollection = mongoContext.GetCollection<Comment>();
        }

        public long GetCommentsCount(ObjectId userId)
        {
            var commentsQuery = Query<Comment>.EQ(c => c.Author, userId);

            return this.commentCollection.Find(commentsQuery).Count();
        }

        public List<Comment> GetComments(List<ObjectId> userIds, int skip)
        {
            return this.commentCollection.AsQueryable<Comment>()
                        .Where(c => c.Author.In(userIds))
                        .OrderByDescending(c => c.PublishDate)
                        .Skip(skip)
                        .Take(20)
                        .ToList();
        }

        public Comment GetComment(ObjectId commentId)
        {
            var query = Query<Comment>.EQ(c => c.Id, commentId);
            return this.commentCollection.FindOne(query);
        }

        public void SaveComment(Comment comment)
        {
            this.commentCollection.Insert(comment);
        }

        public void SaveReply(ObjectId commentId, Reply reply)
        {
            var query = Query<Comment>.EQ(c => c.Id, commentId);
            var update = Update<Comment>.Push<Reply>(c => c.Replies, reply);

            this.commentCollection.Update(query, update);
        }
    }
}
