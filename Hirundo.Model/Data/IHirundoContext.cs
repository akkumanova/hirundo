namespace Hirundo.Model.Data
{
    using MongoDB.Driver;
    using MongoDB.Driver.GridFS;

    public interface IHirundoContext
    {
        MongoCollection<TCollection> GetCollection<TCollection>();

        MongoGridFS GetGridFs();
    }
}
