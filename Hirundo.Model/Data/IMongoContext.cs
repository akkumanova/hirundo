namespace Hirundo.Model.Data
{
    using MongoDB.Driver;
    using MongoDB.Driver.GridFS;

    public interface IMongoContext
    {
        MongoCollection<TCollection> GetCollection<TCollection>();

        MongoGridFS GetGridFs();
    }
}
