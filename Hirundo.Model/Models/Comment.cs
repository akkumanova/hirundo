using System;
using Hirundo.Model.Converters;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace Hirundo.Model.Models
{
    public class Comment
    {
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }

        public ObjectId Author { get; set; }

        public string Content { get; set; }

        public DateTime PublishDate { get; set; }
    }
}
