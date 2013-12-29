using System;
using MongoDB.Bson;

namespace Hirundo.Web.Models
{
    public class ReplyDO
    {
        public string Author { get; set; }

        public ObjectId AuthorId { get; set; }

        public string AuthorImg { get; set; }

        public string Content { get; set; }

        public DateTime PublishDate { get; set; }
    }
}