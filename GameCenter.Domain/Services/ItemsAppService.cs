using System.Collections.Generic;
using System.Linq;
using GameCenter.Domain.Models.Items.Entities;
using GameCenter.Domain.Services.Interfaces;
using GameCenter.Infra;
using MongoDB.Driver;

namespace GameCenter.Domain.Services
{
    public class ItemsAppService : IItemsAppService
    {
        private readonly MongoDbContext _mongoDb;
        public ItemsAppService(MongoDbContext mongoDb)
        {
            _mongoDb = mongoDb;
        }

        public IEnumerable<Item> GetItems()
        {
            return _mongoDb.Items.Find(_ => true).ToList();
        }
    }
}
