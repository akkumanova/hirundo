using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hirundo.Model.Models
{
    public class Comment
    {
        public Comment()
        {
            this.Replies = new List<Reply>();
        }

        public ObjectId Id { get; set; }

        public ObjectId Author { get; set; }

        public string Content { get; set; }

        [BsonDefaultValue(0)]
        public int Favorites { get; set; }

        public List<Reply> Replies { get; set; }

        public DateTime PublishDate { get; set; }

        public List<ObjectId> RetweetedBy { get; set; }
    }
}
