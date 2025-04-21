using GameCenter.Domain.Enums;
using GameCenter.Domain.Responses;
using MongoDB.Bson.Serialization.Attributes;

namespace GameCenter.Domain.Models.Games.Entities.Fileds
{
    [BsonDiscriminator]

    public class NumericConfig : GameOption<GameOptionTranslation>
    {
        public decimal? Min { get; }
        public decimal? Max { get; }

        public NumericConfig(decimal min, decimal max)
            : base(FieldTypeEnum.Numeric)
        {
            Min = min;
            Max = max;
        }
    }

}
