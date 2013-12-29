using System.Collections.Generic;
using MongoDB.Bson;

namespace Hirundo.Web.Models.Comment
{
    public class CommentDetailsDO
    {
        public CommentDetailsDO()
        {
            this.Replies = new List<ReplyDO>();
        }

        public ObjectId CommentId { get; set; }

        public int Retweets { get; set; }

        public int Favorites { get; set; }

        public List<ReplyDO> Replies { get; set; }
    }
}