using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Hirundo.Model.Data
{
    public interface IMongoContext
    {
        MongoCollection<TCollection> GetCollection<TCollection>();

        MongoGridFS GetGridFs();
    }
}
