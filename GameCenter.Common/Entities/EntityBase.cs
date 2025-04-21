using System;

namespace GameCenter.Common.Entities
{
    public abstract class EntityBase
    {
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
