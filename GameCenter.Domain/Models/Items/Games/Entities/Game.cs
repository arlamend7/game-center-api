using System;
using System.Collections.Generic;
using GameCenter.Domain.Enums;
using GameCenter.Domain.Models.Base;
using GameCenter.Domain.Models.Items.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace GameCenter.Domain.Models.Items.Games.Entities
{
    [BsonDiscriminator]
    public class Game : ServerItem
    {
        public GameCategoryEnum GameCategory { get; set; }
        public GamePlayers GamePlayers { get; set; }
        public Translation<GameBotVersionTranslation> GameBot { get; set; }
        public List<GameOption> GameOptions { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public Game() 
        { 
            Type = ItemTypeEnum.Game;
        }
    }
}


