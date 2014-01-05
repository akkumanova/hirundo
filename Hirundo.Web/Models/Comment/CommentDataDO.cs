namespace Hirundo.Web.Models.Comment
{
    using System;
    using MongoDB.Bson;

    public class CommentDataDO
    {
        public ObjectId CommentId { get; set; }

        public string Author { get; set; }

        public ObjectId AuthorId { get; set; }

        public string AuthorImg { get; set; }

        public string Content { get; set; }

        public DateTime PublishDate { get; set; }

        public bool IsRetweeted { get; set; }

        public bool IsFavorited { get; set; }
    }
}