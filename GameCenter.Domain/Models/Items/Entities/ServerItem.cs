using System.IO;
using MongoDB.Bson.Serialization.Attributes;

namespace GameCenter.Domain.Models.Items.Entities
{
    [BsonDiscriminator]
    public class ServerItem : Item
    {
        public virtual string PathUrl => Path.Combine(Type.ToString(), Id.ToString(), MainFile);
        public string MainFile { get; set; }
    }
}
