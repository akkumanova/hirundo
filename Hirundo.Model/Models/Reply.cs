using System;
using MongoDB.Bson;

namespace Hirundo.Model.Models
{
    public class Reply
    {
        public ObjectId Author { get; set; }

        public string Content { get; set; }

        public DateTime PublishDate { get; set; }
    }
}
