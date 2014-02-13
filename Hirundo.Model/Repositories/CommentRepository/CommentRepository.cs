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

    public class CommentRepository : ICommentRepository
    {
        private MongoCollection<Comment> commentCollection;

        public CommentRepository(IHirundoContext mongoContext)
        {
            this.commentCollection = mongoContext.GetCollection<Comment>();
        }

        public Comment GetComment(ObjectId commentId, int replies = 0)
        {
            var query = Query<Comment>.EQ(c => c.Id, commentId);
            var comment = this.commentCollection.FindOne(query);

            comment.Replies = comment.Replies.OrderByDescending(r => r.PublishDate)
                                    .Take(replies)
                                    .ToList();
            return comment;
        }

        public void AddComment(ObjectId authorId, ObjectId orAuthorId, string content, DateTime publishDate, string location, ObjectId? imageId)
        {
            Comment newComment = new Comment
            {
                Author = authorId,
                OriginalAuthor = orAuthorId,
                Content = content,
                PublishDate = publishDate,
                Location = location,
            };

            if (imageId.HasValue)
            {
                newComment.ImgId = imageId.Value;
            }

            this.commentCollection.Insert(newComment, WriteConcern.Acknowledged);
        }

        public void DeleteComment(ObjectId commentId)
        {
            var query = Query<Comment>.EQ(c => c.Id, commentId);

            this.commentCollection.Remove(query, WriteConcern.Acknowledged);
        }

        public void AddReply(ObjectId commentId, ObjectId authorId, string content, DateTime publishDate, string location, ObjectId? imageId)
        {
            Reply reply = new Reply
            {
                Author = authorId,
                Content = content,
                PublishDate = publishDate,
                Location = location
            };

            if (imageId.HasValue)
            {
                reply.ImageId = imageId.Value;
            }

            var query = Query<Comment>.EQ(c => c.Id, commentId);
            var update = Update<Comment>.Push<Reply>(c => c.Replies, reply);

            this.commentCollection.Update(query, update, WriteConcern.Acknowledged);
        }

        public void AddSharing(ObjectId commentId, ObjectId userId)
        {
            var query = Query<Comment>.EQ(c => c.Id, commentId);
            var update = Update<Comment>.Push<ObjectId>(c => c.SharedBy, userId);

            this.commentCollection.Update(query, update, WriteConcern.Acknowledged);

            Comment commentToShare = GetComment(commentId);
            AddComment(userId, commentToShare.Author, commentToShare.Content, DateTime.Now, commentToShare.Location, commentToShare.ImgId);
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

        public IEnumerable<Comment> GetComments(List<ObjectId> userIds, ObjectId takeToId)
        {
            var query = Query<Comment>.In<ObjectId>(c => c.Author, userIds);
            
            return this.commentCollection.Find(query)
                        .OrderByDescending(c => c.PublishDate)
                        .TakeWhile(c => c.Id != takeToId);
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

        public IEnumerable<Reply> GetReplies(ObjectId commentId, int take, int skip)
        {
            var query = Query<Comment>.EQ(c => c.Id, commentId);

            return this.commentCollection.FindOne(query)
                        .Replies
                        .OrderByDescending(r => r.PublishDate)
                        .Skip(skip)
                        .Take(take);
        }
    }
}
