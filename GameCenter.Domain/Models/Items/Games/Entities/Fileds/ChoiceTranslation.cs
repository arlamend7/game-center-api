using System.Collections.Generic;
using GameCenter.Domain.Models.Items.Games.Entities;

namespace GameCenter.Domain.Models.Items.Games.Entities.Fileds
{
    public class ChoiceTranslation : GameOptionTranslation
    {
        public List<string> Choices { get; set; }
    }
}
