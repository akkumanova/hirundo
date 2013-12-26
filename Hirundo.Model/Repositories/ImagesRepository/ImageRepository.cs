using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hirundo.Model.Data;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace Hirundo.Model.Repositories.ImagesRepository
{
    public class ImageRepository : IImageRepository
    {
        private MongoGridFS gridFs;

        public ImageRepository(IMongoContext mongoContext)
        {
            this.gridFs = mongoContext.GetGridFs();
        }

        public string GetImage(ObjectId imageId)
        {
            var image = this.gridFs.FindOneById(imageId);
            byte[] bytes;

            using (var stream = image.OpenRead())
            {
                bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
            }

            string imageBase64 = Convert.ToBase64String(bytes);
            string imageSrc = string.Format("data:image/gif;base64,{0}", imageBase64);

            return imageSrc;
        }
    }
}
