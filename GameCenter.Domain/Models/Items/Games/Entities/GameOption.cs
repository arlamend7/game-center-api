using GameCenter.Common.Entities;
using GameCenter.Domain.Enums;
using GameCenter.Domain.Models.Games.Entities;
using GameCenter.Domain.Models.Games.Entities.Fileds;
using MongoDB.Bson.Serialization.Attributes;

namespace GameCenter.Domain.Responses
{

    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(MultiChoiceConfig), typeof(NumericConfig), typeof(SingleChoiceConfig), typeof(TextConfig))] // add all derived types
    public abstract class GameOption
    {
        public string FieldName {  get; set; }
        public bool Required { get; set; }
        public FieldTypeEnum Type { get; set; }

        public GameOption(FieldTypeEnum type) 
        { 
            Type = type;
        }

    }

    public abstract class GameOption<T> : GameOption, ITranslatable<T>
    {
        public Translation<T> Translation { get; set; }

        public GameOption(FieldTypeEnum type) : base(type)
        {
        }

    }

    public class GameOptionTranslation
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class GameOptionValueTranslation : GameOptionTranslation
    {
        public string Suffix { get; set; }
    }
}
