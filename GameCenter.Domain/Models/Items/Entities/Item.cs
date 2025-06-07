using System;
using GameCenter.Domain.Enums;
using GameCenter.Domain.Models.Base;
using GameCenter.Domain.Models.Items.Games.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace GameCenter.Domain.Models.Items.Entities
{

    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(Game), typeof(ServerItem))] // add all derived types
    public class Item : EntityBase, ITranslatable<ItemTranslation>
    {
        public ItemTypeEnum Type { get; set; }
        public Translation<ItemTranslation> Translation { get; set; }
        public string ImageUrl {get; set;}
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
