namespace Hirundo.Model.Models
{
    using System;
    using MongoDB.Bson;

    public class Reply
    {
        public ObjectId Author { get; set; }

        public string Content { get; set; }

        public string Location { get; set; }

        public DateTime PublishDate { get; set; }
    }
}
