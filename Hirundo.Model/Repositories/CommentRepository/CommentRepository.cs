namespace Hirundo.Model.Repositories.CommentRepository
{
    using System.Collections.Generic;
    using System.Linq;
    using Hirundo.Model.Data;
    using Hirundo.Model.Models;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;

    public class CommentRepository : ICommentRepository
    {
        private MongoCollection<Comment> commentCollection;

        public CommentRepository(IHirundoContext mongoContext)
        {
            this.commentCollection = mongoContext.GetCollection<Comment>();
        }

        public Comment GetComment(ObjectId commentId)
        {
            var query = Query<Comment>.EQ(c => c.Id, commentId);

            return this.commentCollection.FindOne(query);
        }

        public void AddComment(Comment comment)
        {
            this.commentCollection.Insert(comment, WriteConcern.Acknowledged);
        }

        public void DeleteComment(ObjectId commentId)
        {
            var query = Query<Comment>.EQ(c => c.Id, commentId);

            this.commentCollection.Remove(query, WriteConcern.Acknowledged);
        }

        public void AddReply(ObjectId commentId, Reply reply)
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

        public long GetCommentsCount(ObjectId userId)
        {
            var query = Query<Comment>.EQ(c => c.Author, userId);

            return this.commentCollection.Find(query).Count();
        }

        public IEnumerable<Comment> GetComments(List<ObjectId> userIds, int take, int skip)
        {
            var query = Query<Comment>.In<ObjectId>(c => c.Author, userIds);

            return this.commentCollection.Find(query)
                        .OrderByDescending(c => c.PublishDate)
                        .Skip(skip)
                        .Take(take);
        }


        public IEnumerable<Comment> GetComments(ObjectId userId, int take, int skip)
        {
            var query = Query<Comment>.EQ(c => c.Author, userId);

            return this.commentCollection.Find(query)
                        .OrderByDescending(c => c.PublishDate)
                        .Skip(skip)
                        .Take(take);
        }

        public IEnumerable<Comment> GetFavorites(ObjectId userId, int take, int skip)
        {
            var query = Query<Comment>.Where(c => c.FavoritedBy.Contains(userId));

            return this.commentCollection.Find(query)
                        .OrderByDescending(c => c.PublishDate)
                        .Skip(skip)
                        .Take(take);
        }
    }
}
