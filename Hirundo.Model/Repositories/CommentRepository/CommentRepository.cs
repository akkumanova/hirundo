namespace Hirundo.Model.Repositories.CommentRepository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Hirundo.Model.Data;
    using Hirundo.Model.Models;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;

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

        public List<Comment> GetComments(List<ObjectId> userIds, int take, int skip)
        {
            return this.commentCollection.AsQueryable<Comment>()
                        .Where(c => c.Author.In(userIds))
                        .OrderByDescending(c => c.PublishDate)
                        .Skip(skip)
                        .Take(take)
                        .ToList();
        }

        public List<Comment> GetComments(ObjectId userId, int take, int skip)
        {
            var query = Query<Comment>.EQ(c => c.Author, userId);

            return this.commentCollection.Find(query)
                        .OrderByDescending<Comment, DateTime>(c => c.PublishDate)
                        .Skip(skip)
                        .Take(take)
                        .ToList();
        }

        public List<Comment> GetLastComments(ObjectId userId, int count)
        {
            return this.commentCollection.AsQueryable<Comment>()
                        .Where(c => c.Author == userId)
                        .OrderByDescending(c => c.PublishDate)
                        .Take(count)
                        .ToList();
        }

        public Comment GetComment(ObjectId commentId)
        {
            var query = Query<Comment>.EQ(c => c.Id, commentId);
            return this.commentCollection.FindOne(query);
        }

        public void DeleteComment(ObjectId commentId)
        {
            var query = Query<Comment>.EQ(c => c.Id, commentId);
            this.commentCollection.Remove(query, WriteConcern.Acknowledged);
        }

        public void SaveComment(Comment comment)
        {
            this.commentCollection.Insert(comment, WriteConcern.Acknowledged);
        }

        public void SaveReply(ObjectId commentId, Reply reply)
        {
            var query = Query<Comment>.EQ(c => c.Id, commentId);
            var update = Update<Comment>.Push<Reply>(c => c.Replies, reply);

            this.commentCollection.Update(query, update, WriteConcern.Acknowledged);
        }

        public void AddRetweet(ObjectId commentId, ObjectId userId)
        {
            var query = Query<Comment>.EQ(c => c.Id, commentId);
            var update = Update<Comment>.Push<ObjectId>(c => c.RetweetedBy, userId);

            this.commentCollection.Update(query, update, WriteConcern.Acknowledged);
        }

        public void AddFavotite(ObjectId commentId, ObjectId userId)
        {
            var query = Query<Comment>.EQ(c => c.Id, commentId);
            var update = Update<Comment>.Push<ObjectId>(c => c.FavoritedBy, userId);

            this.commentCollection.Update(query, update, WriteConcern.Acknowledged);
        }
    }
}
