using System;
using GameCenter.Utilities.Injectors.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace GameCenter.Infra
{
    public class MongoDbContext : IMongoDbContext
    {
        public IMongoDatabase Database { get; private set; }

        public MongoDbContext([FromKeyedServices("Database")] DatabaseSetting databaseSetting)
        {
            Register();

            MongoClientSettings settings = ConfigureSettings(databaseSetting.ConnectionString);

            var client = new MongoClient(settings);
            Database = client.GetDatabase("kap-games");
        }

        private static MongoClientSettings ConfigureSettings(string connectionString)
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.ClusterConfigurator = builder =>
            {
                builder.Subscribe<CommandStartedEvent>(e =>
                {
                    Console.WriteLine($"Command Started: {e.CommandName}, {e.Command}");
                });
            };
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            return settings;
        }

        private static void Register()
        {
            var pack = new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String, false),
            };
            ConventionRegistry.Register("EnumStringConvention", pack, t => true);
        }
    }
    public interface IMongoDbContext
    {
        IMongoDatabase Database { get; }
    }
}
