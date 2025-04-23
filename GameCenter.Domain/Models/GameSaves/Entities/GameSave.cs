using System.Collections.Generic;
using GameCenter.Domain.Enums;
using GameCenter.Domain.Models.Base;
using GameCenter.Domain.Models.Items.Games.Entities;
using GameCenter.Domain.Models.Players.Entities;

namespace GameCenter.Domain.Models.GameSaves.Entities
{
    public class GameSave : EntityBase
    {
        public Game Game {  get; set; } 
        public List<UserSimpleInfo> Users { get; set; }

        public Dictionary<string, object> Options { get; set; }
        public PlayTypeEnum PlayType { get; set; }
        public GameRounds Rounds { get; set; }
        public int TotalRounds { get; set; }
    }
    public class GameRounds : Dictionary<int, List<GameMatch>>
    {
    }
}
