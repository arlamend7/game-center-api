using System.Collections.Generic;
using GameCenter.Domain.Models.Games.Entities;
using GameCenter.Domain.Models.Items.Entities;

namespace GameCenter.Domain.Services.Interfaces
{
    public interface IItemsAppService
    {
        IEnumerable<Item> GetItems();
    }
}