using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace GameCenter.Common.Entities
{
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
}
