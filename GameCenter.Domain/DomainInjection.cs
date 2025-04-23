using System.Collections.Generic;
using GameCenter.Domain.Models.GameSaves.Entities;
using GameCenter.Domain.Services;
using GameCenter.Domain.Services.Interfaces;
using GameCenter.Utilities.Encryptors;
using GameCenter.Utilities.Encryptors.Interfaces;
using GameCenter.Utilities.Injectors;
using GameCenter.Utilities.Injectors.Models;
using GameCenter.Utilities.MongoDb;
using GameCenter.Utilities.TokenBuilder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GameCenter.Domain
{
    public static class DomainInjection
    {

        public static IServiceCollection InjectDomain(this IServiceCollection serviceProvider, IConfiguration configuration)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new GameRoundsSerializer());


            return serviceProvider.AddSingleton<IMongoDbContext, MongoDbContext>()
                                  .ResolveOptions<InjectorSetting>(configuration)
                                  .AddSingleton<IAesOperation>(provider =>
                                  {
                                      var encryptorSettings = provider.GetKeyedService<EncryptorSetting>("Encryptor");
                                      return new AesOperation(encryptorSettings.Key);
                                  })
                                  .AddScoped(provider =>
                                  {
                                      var encryptorSettings = provider.GetKeyedService<EncryptorSetting>("Token");
                                      return TokenBuilder.Factory(encryptorSettings.Key, expirationTime: 6);
                                  })
                                  .AddScoped<IItemsAppService, ItemsAppService>()
                                  .AddScoped<IUserAppService, UserAppService>();
        }
    }


    public class GameRoundsSerializer : SerializerBase<GameRounds>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, GameRounds value)
        {
            var writer = context.Writer;

            writer.WriteStartDocument();
            foreach (var kvp in value)
            {
                writer.WriteName(kvp.Key.ToString()); // Serializa a chave como string
                var listSerializer = BsonSerializer.LookupSerializer<List<GameMatch>>();
                listSerializer.Serialize(context, kvp.Value);
            }
            writer.WriteEndDocument();
        }

        public override GameRounds Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var reader = context.Reader;
            var result = new GameRounds();

            reader.ReadStartDocument();
            while (reader.ReadBsonType() != BsonType.EndOfDocument)
            {
                var keyString = reader.ReadName();
                if (int.TryParse(keyString, out var key))
                {
                    var listSerializer = BsonSerializer.LookupSerializer<List<GameMatch>>();
                    var list = listSerializer.Deserialize(context, args);
                    result[key] = list;
                }
                else
                {
                    reader.SkipValue(); // Evita quebra se alguma chave inválida aparecer
                }
            }
            reader.ReadEndDocument();

            return result;
        }
    }

}
