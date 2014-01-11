namespace Hirundo.Model.Repositories.ImagesRepository
{
    using System;
    using Hirundo.Model.Data;
    using MongoDB.Bson;
    using MongoDB.Driver.GridFS;

    public class ImageRepository : IImageRepository
    {
        private MongoGridFS gridFs;

        public ImageRepository(IHirundoContext mongoContext)
        {
            this.gridFs = mongoContext.GetGridFs();
        }

        public string GetImage(ObjectId imageId)
        {
            string imageSrc;
            var image = this.gridFs.FindOneById(imageId);
            if (image != null)
            {
                byte[] bytes;

                using (var stream = image.OpenRead())
                {
                    bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);
                }

                string imageBase64 = Convert.ToBase64String(bytes);
                imageSrc = string.Format("data:image/gif;base64,{0}", imageBase64);
            }
            else
            {
                throw new Exception(string.Format("Invalid image id: {0}", imageId.ToString()));
            }

            return imageSrc;
        }
    }
}
