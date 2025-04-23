using System.Collections.Generic;
using GameCenter.Domain.Models.GameSaves.Entities;
using GameCenter.Domain.Models.Items.Entities;

namespace GameCenter.Domain.Services.Interfaces
{
    public interface IItemsAppService
    {
        IEnumerable<Item> GetItems();
        IEnumerable<GameSave> GetGameSaves();
    }
}