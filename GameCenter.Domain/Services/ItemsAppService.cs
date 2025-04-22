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
        private readonly IMongoCollection<Item> _itemsDb;
        public ItemsAppService(IMongoDbContext mongoDb)
        {
            _itemsDb = mongoDb.Database.GetCollection<Item>("items");
        }

        public IEnumerable<Item> GetItems()
        {
            return _itemsDb.Find(_ => true).ToList();
        }
    }
}
