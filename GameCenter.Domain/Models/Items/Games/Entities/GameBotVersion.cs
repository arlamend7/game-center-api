using System.Collections.Generic;
using GameCenter.Domain.Models.Base;

namespace GameCenter.Domain.Models.Items.Games.Entities
{
    public class GameBotVersion : ITranslatable<GameBotVersionTranslation>
    {
        public string Version { get; set; }
        public Translation<GameBotVersionTranslation> Translation { get; set; }
    }

    public class GameBotVersionTranslation
    {
        public List<string> Difficults { get; set; }
    }
}
