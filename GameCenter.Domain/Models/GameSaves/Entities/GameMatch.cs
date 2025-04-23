using System;
using System.Collections.Generic;
using GameCenter.Domain.Models.Base;
using GameCenter.Domain.Models.Players.Entities;

namespace GameCenter.Domain.Models.GameSaves.Entities
{
    public class GameMatch : EntityBase
    {
        public bool Started { get; set; }
        public bool Finished { get; set; }
        public List<Guid> Users { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public DynamicValue ResultInfo { get; set; }
        public List<GameAction> Actions { get; set; }
    }

    public class DynamicValue : Translation<Dictionary<string, object>>
    {

    }
    public class UserGameResult : UserSimpleInfo
    {
        public DynamicValue Result { get; set; }
    }
}
