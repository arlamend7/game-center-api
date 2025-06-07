using System;
using GameCenter.Domain.Models.Base;

namespace GameCenter.Domain.Models.GameSaves.Entities
{
    public class GameAction
    {
        public Guid UserId { get; set; }
        public string Context { get; set; }
        public object Value { get; set; }
        public DateTime CreateAt { get; set; }
        public Translation<string> Description { get; set; }
    }
}
