using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using GameCenter.Common.Entities;

namespace GameCenter.Common.Entities
{

    public enum LanguageEnum
    {
        English,
        French,
        Portuguese,
        Spanish
    }

    public class Translatable<T> : EntityBase where T : ITranslation
    {
        [BsonIgnore]
        public Dictionary<LanguageEnum, T> Translation
        {
            get
            {
                if (TranslationKeys == null)
                    return new Dictionary<LanguageEnum, T>();

                return TranslationKeys.ToDictionary(
                    kv => Enum.Parse<LanguageEnum>(kv.Key),
                    kv => kv.Value
                );
            }
            set
            {
                TranslationKeys = value?.ToDictionary(
                    kv => kv.Key.ToString(),
                    kv =>
                    {
                        kv.Value.Language = kv.Key;
                        return kv.Value;
                    }
                );
            }
        }

        // This gets stored in MongoDB
        [BsonDictionaryOptions(DictionaryRepresentation.Document)]
        public Dictionary<string, T> TranslationKeys { get; private set; }

        public T GetTranslation(LanguageEnum languageEnum)
        {
            return Translation.GetValueOrDefault(languageEnum);
        }
    }


    public interface ITranslation
    {
        public LanguageEnum Language { get; set; }
    }

    public abstract class EntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public virtual Guid Id { get; set; }
        public virtual void SetId(Guid key)
        {
            if (key.Equals(default)) throw new Exception("Index not found");
            Id = key;
        }

        public static T CreateReference<T>(Guid id) where T : EntityBase
        {
            var entity = (T)Activator.CreateInstance(typeof(T), true);
            entity.SetId(id);
            return entity;
        }
    }



    public class MyEntity : Translatable<MyEntityTranslation>
    {
        public DateTime CreateAt { get; set; }
    }

    public class MyEntityTranslation : ITranslation
    {
        [BsonRepresentation(BsonType.String)]
        public LanguageEnum Language { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}