using GameCenter.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace GameCenter.Domain.Models.Items.Games.Entities.Fileds
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
