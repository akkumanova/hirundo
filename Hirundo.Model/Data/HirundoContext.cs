﻿namespace Hirundo.Model.Data
{
    using System.Configuration;
    using System.Data.Entity;
    using MongoDB.Driver;
    using MongoDB.Driver.GridFS;

    public class HirundoContext : DbContext, IHirundoContext
    {
        private MongoDatabase database;

        public HirundoContext() 
            : base("name=MongoConnection")
        {
            MongoServer server = this.Connect();
            this.database = server.GetDatabase("hirundo");
        }

        public MongoCollection<TCollection> GetCollection<TCollection>()
        {
            string name = typeof(TCollection).Name.ToLower();

            return this.database.GetCollection<TCollection>(name);
        }

        public MongoGridFS GetGridFs()
        {
            return this.database.GridFS;
        }

        private MongoServer Connect()
        {
            var client = new MongoClient(ConfigurationManager.ConnectionStrings["MongoConnection"].ConnectionString.ToString());
            var server = client.GetServer();

            if (server.State == MongoServerState.Disconnected)
            {
                server.Connect();
            }

            return server;
        }
    }
}
