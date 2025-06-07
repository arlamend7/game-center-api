using System;
using System.Collections.Generic;
using System.Linq;
using GameCenter.Domain.Models.Base;
using GameCenter.Domain.Models.GameSaves.Entities;
using GameCenter.Domain.Models.Items.Entities;
using GameCenter.Domain.Models.Players.Entities;
using GameCenter.Domain.Services.Interfaces;
using GameCenter.Utilities.MongoDb;
using MongoDB.Driver;
using SGTC.Common.Sessions.Interfaces;

namespace GameCenter.Domain.Services
{
    public class ItemsAppService : IItemsAppService
    {
        private readonly IMongoCollection<Item> _itemsDb;
        private readonly IMongoCollection<GameSave> _saves;
        private readonly IMongoCollection<User> _users;
        private readonly IUserSession _userSession;

        public ItemsAppService(IMongoDbContext mongoDb, IUserSession userSession)
        {
            _itemsDb = mongoDb.Database.GetCollection<Item>("items");
            _users = mongoDb.Database.GetCollection<User>("users");
            _saves = mongoDb.Database.GetCollection<GameSave>("Saves");
            _userSession = userSession;
        }

        public IEnumerable<Item> GetItems()
        {
            return _itemsDb.Find(_ => true).ToList();
        }

        public IEnumerable<GameSave> GetSaves()
        {
            _saves.DeleteMany(_ => true);
            AddSave();
            return _saves.Find(x => x.Users.Any(y => y.Id == _userSession.UserId)).ToList();
        }

        public void AddSave()
        {
            GameSoloSave gameSave = SoloGameMock();

            _saves.InsertOne(gameSave);
        }

        private GameSoloSave SoloGameMock()
        {
            var actionsList = Actions();
            var user = _users.Find(x => x.Id == _userSession.UserId).FirstOrDefault();

            var gameSave = new GameSoloSave()
            {
                Id = Guid.NewGuid(),
                GameId = Guid.Parse("7e29a3f7-1ec7-4f55-9bd5-cb6ac0a28321"),
                Users = new List<UniqueUserInfo>()
                {
                    new UniqueUserInfo()
                    {
                        Id = _userSession.UserId,
                        NickName = user.NickName,
                        Tag = user.Tag
                    }
                },
                Options = new Dictionary<string, object>
                {
                    ["Seed"] = "H2Z32XD"
                },
                Completed = true,
                Match = new GameMatch()
                {
                    Id = Guid.NewGuid(),
                    StartDate = DateTime.Now.AddMinutes(-25).AddSeconds(-17),
                    FinishDate = DateTime.Now,
                    Actions = actionsList.ToList(),
                    ResultInfo = null,
                }
            };
            return gameSave;
        }

        public IEnumerable<GameAction> Actions()
        {
            Random random = new Random();
            var date = DateTime.Now.AddMinutes(-30);

            foreach (var _ in Enumerable.Range(0, 25))
            {
                date = date.AddMinutes(1);
                var isFill = random.NextDouble() > 0.3;

                int row = random.Next(9);
                int col = random.Next(9);

                object obj;
                int? value = null;

                if (isFill)
                {
                    value = random.Next(1, 10);
                    obj = new { Row = row, Column = col, Value = value };
                }
                else
                {
                    obj = new { Row = row, Column = col };
                }

                yield return new GameAction
                {
                    UserId = _userSession.UserId,
                    Context = isFill ? "CellFilled" : "CellCleared",
                    CreateAt = date,
                    Value = obj,
                    Description = BuildDescription(row, col, value)
                };
            }
        }


        private Translation<string> BuildDescription(int row, int col, int? value = null)
        {
            var description = new Translation<string>();

            if (value.HasValue)
            {
                description[LanguageEnum.English] = $"Filled cell ({row},{col}) with {value}";
                description[LanguageEnum.Portuguese] = $"Preencheu célula ({row},{col}) com {value}";
                description[LanguageEnum.French] = $"Cellule remplie ({row},{col}) avec {value}";
                description[LanguageEnum.Spanish] = $"Celda completada ({row},{col}) con {value}";
            }
            else
            {
                description[LanguageEnum.English] = $"Cleared cell ({row},{col})";
                description[LanguageEnum.Portuguese] = $"Limpou célula ({row},{col})";
                description[LanguageEnum.French] = $"Cellule effacée ({row},{col})";
                description[LanguageEnum.Spanish] = $"Celda limpiada ({row},{col})";
            }

            return description;
        }
    }
}
