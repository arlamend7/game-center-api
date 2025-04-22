using GameCenter.Domain.Models.Items.Entities;
using GameCenter.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameCenter.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsAppService _itemsAppService;

        public ItemsController(IItemsAppService itemsAppService)
        {
            _itemsAppService = itemsAppService;
        }

        [HttpGet()]
        public IEnumerable<Item> Get()
        {
            var values = _itemsAppService.GetItems();
            return values;
        }

    }
}
