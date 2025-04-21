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
        public IActionResult Get()
        {
            var values = _itemsAppService.GetItems();
            return Ok(values);
        }

        [HttpGet("Games")]
        public IActionResult GetGames()
        {
            var values = _itemsAppService.GetGames();
            return Ok(values);
        }

    }
}
