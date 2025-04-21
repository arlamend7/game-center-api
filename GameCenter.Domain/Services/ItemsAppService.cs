using System;
using System.Collections.Generic;
using GameCenter.Common.Entities;
using GameCenter.Domain.Enums;
using GameCenter.Domain.Models.Games.Entities;
using GameCenter.Domain.Models.Games.Entities.Fileds;
using GameCenter.Domain.Models.Items.Entities;
using GameCenter.Domain.Responses;
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

        public IEnumerable<Game> GetGames()
        {
            return _mongoDb.Games.Find(_ => true).ToList();
        }

        public IEnumerable<ServerItem> GetServerItems()
        {
            return _mongoDb.ServerItems.Find(_ => true).ToList();
        }
    }
}
