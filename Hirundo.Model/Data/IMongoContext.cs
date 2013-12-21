using MongoDB.Driver;

namespace Hirundo.Model.Data
{
    public interface IMongoContext
    {
        MongoCollection<TCollection> GetCollection<TCollection>();
    }
}
