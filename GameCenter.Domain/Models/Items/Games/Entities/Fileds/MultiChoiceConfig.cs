using GameCenter.Domain.Enums;
using GameCenter.Domain.Responses;
using MongoDB.Bson.Serialization.Attributes;

namespace GameCenter.Domain.Models.Games.Entities.Fileds
{
    [BsonDiscriminator]

    public class MultiChoiceConfig : GameOption<ChoiceTranslation>
    {
        public MultiChoiceConfig()
            : base(FieldTypeEnum.MultiChoice)
        {
        }
    }
}
