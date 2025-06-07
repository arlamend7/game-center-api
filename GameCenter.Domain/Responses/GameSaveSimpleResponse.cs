using GameCenter.Domain.Enums;
using GameCenter.Domain.Models.Base;
using GameCenter.Domain.Models.Items.Entities;

namespace GameCenter.Domain.Responses
{
    public class GameSaveSimpleResponse : EntityBase
    {
        public Item Game { get; set; }
        public PlayTypeEnum PlayType { get; set; }
        public SessionTypeEnum SessionType { get; set; }
        public Translation<GameSaveResult> GameOwnResult { get; set; }
    }

    public class GameSaveResult
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
