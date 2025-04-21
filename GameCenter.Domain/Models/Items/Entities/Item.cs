using System.IO;
using GameCenter.Common.Entities;
using GameCenter.Domain.Enums;
using GameCenter.Domain.Models.Games.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace GameCenter.Domain.Models.Items.Entities
{

    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(Game), typeof(ServerItem))] // add all derived types
    public class Item : EntityBase, ITranslatable<ItemTranslation>
    {
        public ItemTypeEnum Type { get; set; }
        public Translation<ItemTranslation> Translation { get; set; }

    }
    [BsonDiscriminator]
    public class ServerItem : Item
    {
        public string PatUrl => Path.Combine(Type.ToString(), Id.ToString(), MainFile);
        public string MainFile { get; set; }
    }
    
    public class ItemTranslation
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
    }
}
