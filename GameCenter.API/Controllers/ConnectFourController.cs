using Microsoft.AspNetCore.Mvc;

namespace GameCenter.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectFourController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ConnectFourController> _logger;

        public ConnectFourController(ILogger<ConnectFourController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
