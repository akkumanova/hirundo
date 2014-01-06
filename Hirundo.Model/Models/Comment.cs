﻿namespace Hirundo.Model.Models
{
    using System;
    using System.Collections.Generic;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Comment
    {
        public Comment()
        {
            this.Replies = new List<Reply>();
            this.RetweetedBy = new List<ObjectId>();
            this.FavoritedBy = new List<ObjectId>();
        }

        public ObjectId Id { get; set; }

        public ObjectId Author { get; set; }

        public string Content { get; set; }

        public List<Reply> Replies { get; set; }

        public DateTime PublishDate { get; set; }

        public List<ObjectId> RetweetedBy { get; set; }

        public List<ObjectId> FavoritedBy { get; set; }
    }
}
