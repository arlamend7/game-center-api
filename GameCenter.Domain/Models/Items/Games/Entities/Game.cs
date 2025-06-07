using GameCenter.Domain.Enums;
using GameCenter.Domain.Models.Base;
using GameCenter.Domain.Models.Items.Entities;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace GameCenter.Domain.Models.Items.Games.Entities
{
    [BsonDiscriminator]
    public class Game : ServerItem
    {
        public GameCategoryEnum GameCategory { get; set; }
        public GamePlayers GamePlayers { get; set; }
        public Translation<GameBotVersionTranslation> GameBot { get; set; }
        public List<GameOption> GameOptions { get; set; }
        public ServerItem Tutorial { get; set; }


        public Game() 
        { 
            Type = ItemTypeEnum.Game;
        }
    }
}


