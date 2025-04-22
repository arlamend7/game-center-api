using GameCenter.Domain.Services;
using GameCenter.Domain.Services.Interfaces;
using GameCenter.Infra;
using Microsoft.Extensions.DependencyInjection;
using SGTC.IOC.Injectors;
using Microsoft.Extensions.Configuration;
using SGTC.Utilities.Encryptors.Interfaces;
using SGTC.Utilities.Encryptors;
using SGTC.Authentication.TokenBuilder;
using GameCenter.Utilities.Injectors.Models;

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
