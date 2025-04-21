using GameCenter.Domain.Enums;
using GameCenter.Domain.Responses;
using MongoDB.Bson.Serialization.Attributes;

namespace GameCenter.Domain.Models.Games.Entities.Fileds
{
    [BsonDiscriminator]

    public class TextConfig : GameOption<GameOptionTranslation>
    {
        public int? MinLength { get; }
        public int? MaxLength { get; }

        public TextConfig(int minLength, int maxLength)
            : base(FieldTypeEnum.Text)
        {
            MinLength = minLength;
            MaxLength = maxLength;
        }
    }

}
