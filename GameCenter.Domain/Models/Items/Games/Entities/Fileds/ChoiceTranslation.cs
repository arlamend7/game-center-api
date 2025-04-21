using System.Collections.Generic;
using GameCenter.Domain.Responses;

namespace GameCenter.Domain.Models.Games.Entities.Fileds
{
    public class ChoiceTranslation : GameOptionTranslation
    {
        public List<string> Choices { get; }
    }
}
