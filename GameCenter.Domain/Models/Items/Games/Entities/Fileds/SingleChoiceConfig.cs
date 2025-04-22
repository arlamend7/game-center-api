using GameCenter.Domain.Enums;
using GameCenter.Domain.Models.Items.Games.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace GameCenter.Domain.Models.Items.Games.Entities.Fileds
{
    [BsonDiscriminator]

    public class SingleChoiceConfig : GameOption<ChoiceTranslation>
    {
        public SingleChoiceConfig()
            : base(FieldTypeEnum.SingleChoice)
        {
        }
    }

}
