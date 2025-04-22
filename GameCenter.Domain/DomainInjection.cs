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

namespace GameCenter.Domain
{
    public static class DomainInjection
    {

        public static IServiceCollection InjectDomain(this IServiceCollection serviceProvider, IConfiguration configuration)
        {
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
}
