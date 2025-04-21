using GameCenter.Common.Entities;

namespace GameCenter.Domain.Models.Games.Entities
{
    public interface ITranslatable<T>
    {
        public Translation<T> Translation { get; set; }
    }
}
