using System;
using System.Collections.Generic;
using System.Linq;
using GameCenter.Domain.Enums;
using GameCenter.Domain.Models.Base;
using GameCenter.Domain.Models.GameSaves.Entities;
using GameCenter.Domain.Models.Items.Entities;
using GameCenter.Domain.Models.Items.Games.Entities;
using GameCenter.Domain.Models.Players.Entities;
using GameCenter.Domain.Services.Interfaces;
using GameCenter.Utilities.MongoDb;
using MongoDB.Driver;

namespace GameCenter.Domain.Services
{
    public class ItemsAppService : IItemsAppService
    {
        private readonly IMongoCollection<Item> _itemsDb;
        private readonly IMongoCollection<GameSave> _saves;
        private readonly IMongoCollection<User> _users;
        public ItemsAppService(IMongoDbContext mongoDb)
        {
            _itemsDb = mongoDb.Database.GetCollection<Item>("items");
            _saves = mongoDb.Database.GetCollection<GameSave>("Saves");
            _users = mongoDb.Database.GetCollection<User>("users");
        }

        public IEnumerable<Item> GetItems()
        {
            return _itemsDb.Find(_ => true).ToList();
        }

        public IEnumerable<GameSave> GetGameSaves()
        {
            return _saves.Find(_ => true).ToList();
        }
    }
}
