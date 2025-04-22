using GameCenter.Domain.Models.Base;

namespace GameCenter.Domain.Models.Items.Games.Entities
{
    public interface ITranslatable<T>
    {
        public Translation<T> Translation { get; set; }
    }
}
