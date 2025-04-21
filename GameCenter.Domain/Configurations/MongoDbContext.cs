using GameCenter.Domain.Models.Items.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using System;
using GameCenter.Domain.Models.Games.Entities;

namespace GameCenter.Infra
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var pack = new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String, false),
            };
            ConventionRegistry.Register("EnumStringConvention", pack, t => true);

            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://arlanmendes197:VvUuy2pLKvhGtJCs@kap-games-cluster.kas7ktx.mongodb.net/?retryWrites=true&w=majority&appName=KAP-games-cluster");
            settings.ClusterConfigurator = builder =>
            {
                builder.Subscribe<CommandStartedEvent>(e =>
                {
                    Console.WriteLine($"Command Started: {e.CommandName}, {e.Command}");
                });
            };
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            var client = new MongoClient(settings);
            _database = client.GetDatabase("kap-games");
        }

        public IMongoCollection<Item> Items => _database.GetCollection<Item>("items");
        public IMongoCollection<Game> Games => _database.GetCollection<Game>("items");
        public IMongoCollection<ServerItem> ServerItems => _database.GetCollection<ServerItem>("items");
    }

}
