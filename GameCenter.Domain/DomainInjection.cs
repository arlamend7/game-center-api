using GameCenter.Domain.Services;
using GameCenter.Domain.Services.Interfaces;
using GameCenter.Infra;
using Microsoft.Extensions.DependencyInjection;

namespace GameCenter.Domain
{
    public static class DomainInjection
    {

        public static IServiceCollection InjectDomain(this IServiceCollection serviceProvider)
        {
            return serviceProvider.AddSingleton<MongoDbContext>()
                                  //.AddScoped(typeof(DatabaseRepository<>))
                                  .AddScoped<IItemsAppService, ItemsAppService>();
        }
    }
}
