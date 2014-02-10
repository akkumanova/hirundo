using System;
using MongoDB.Bson;

namespace Hirundo.Web.Models.Comment
{
    public class NewComment
    {
        public ObjectId Author { get; set; }

        public string Content { get; set; }

        public string Location { get; set; }

        public string Image { get; set; }

        public DateTime PublishDate { get; set; }
    }
}