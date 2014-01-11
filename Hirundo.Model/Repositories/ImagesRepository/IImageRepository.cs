namespace Hirundo.Model.Repositories.ImagesRepository
{
    using MongoDB.Bson;

    public interface IImageRepository
    {
        string GetImage(ObjectId imageId);
    }
}
