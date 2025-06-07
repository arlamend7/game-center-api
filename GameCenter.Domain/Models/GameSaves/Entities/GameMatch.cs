using GameCenter.Domain.Models.Base;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace GameCenter.Domain.Models.GameSaves.Entities
{
    public class GameMultiplayerMatch : GameMatch
    {
        public List<UserGameResult> Users { get; set; }
    }
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(GameMultiplayerMatch))]
    public class GameMatch : EntityBase
    {
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public DynamicValue ResultInfo { get; set; }
        public List<GameAction> Actions { get; set; }
    }
    public class DynamicValue : Translation<Dictionary<string, object>>
    {

    }
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(WinLoseGameResult), typeof(OrderGameResult), typeof(PointGameResult))]
    public class UserGameResult
    {
        public Guid UserId { get; set; }
        public DynamicValue Result { get; set; }
    }
    public class WinLoseGameResult : UserGameResult
    {
        public bool Win { get; set; }
    }
    public class OrderGameResult : UserGameResult
    {
        public bool Win => Order == 0;
        public int Order { get; set; }
    }

    public class PointGameResult : UserGameResult
    {
        public object Value { get; set; }
        public Translation<string> Description { get; set; }
    }
}
