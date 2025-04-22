using GameCenter.Domain.Enums;
using GameCenter.Domain.Responses;
using MongoDB.Bson.Serialization.Attributes;

namespace GameCenter.Domain.Models.Games.Entities.Fileds
{
    [BsonDiscriminator]
    public class NumericConfig : GameOption<GameOptionValueTranslation>
    {
        public decimal? DefaultValue { get; set; }
        public decimal? Min { get; set; }
        public decimal? Max { get; set; }

        public NumericConfig(decimal min, decimal max, decimal? defaultValue = null)
            : base(FieldTypeEnum.Numeric)
        {
            Min = min;
            Max = max;
            DefaultValue = defaultValue;
        }
    }

}
