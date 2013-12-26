using System.Collections.Generic;
using Hirundo.Model.Models;

namespace Hirundo.Web.Models
{
    public class UserCommentsDO
    {
        public long Count { get; set; }

        public List<Comment> Comments { get; set; }
    }
}