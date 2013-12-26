using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Hirundo.Model.Repositories.ImagesRepository
{
    public interface IImageRepository
    {
        string GetImage(ObjectId imageId);
    }
}
