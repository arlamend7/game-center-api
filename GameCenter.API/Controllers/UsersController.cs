using GameCenter.Domain.Models.Players.Entities;
using GameCenter.Domain.Services.Interfaces;
using GameCenter.Utilities.TokenBuilder;
using Microsoft.AspNetCore.Mvc;

namespace GameCenter.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserAppService _usersAppService;
        private readonly ITokenBuilderConfigurated _tokenBuilder;

        public UsersController(IUserAppService usersAppService, ITokenBuilderConfigurated tokenBuilder)
        {
            _usersAppService = usersAppService;
            _tokenBuilder = tokenBuilder;
        }


        [HttpGet]
        public IActionResult GetUser(string email, string tag)
        {
            var user = _usersAppService.GetUser(email, tag);
            return Ok(new
            {
                user.Id,
                user.NickName,
                user.Tag,
                user.FullName,
            });
        }

        [HttpGet("correct-pin")]
        public IActionResult IsCorrectPin(string email, string pin)
        {
            return Ok(_usersAppService.IsCorrectPin(email, pin));
        }

        [HttpGet("valid-tag")]
        public IActionResult validateTag(string nickname, string tag)
        {
            return Ok(_usersAppService.ValidTag(nickname, tag));
        }

        [HttpHead]
        public IActionResult Login(string email, string password)
        {
            var user = _usersAppService.Login(email, password);
            if (user == null) {
                return BadRequest();
            }

            return Ok(new
            {
                Token = CreateToken(user),
                user = new
                {
                    user.Id,
                    user.FullName,
                    user.FirstName,
                    user.MiddleName,
                    user.LastName,
                    user.Tag,
                    user.Email,
                    user.Preferences
                }
            });
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            var result = _usersAppService.AddUser(user);

            if(!result)
            {
                return BadRequest();
            }

            return Ok(new
            {
                Token = CreateToken(user),
                user = new
                {
                    user.Id,
                    user.FullName,
                    user.FirstName,
                    user.MiddleName,
                    user.LastName,
                    user.Tag,
                    user.Email,
                    user.Preferences
                }
            });
        }

        [HttpPut]
        public IActionResult Update(string email, string pin, string password)
        {
            var user = _usersAppService.ChangePassword(email, pin, password);


            return Ok(new
            {
                Token = CreateToken(user),
                user = new
                {
                    user.Id,
                    user.FullName,
                    user.FirstName,
                    user.MiddleName,
                    user.LastName,
                    user.Tag,
                    user.Email,
                    user.Preferences
                }
            });
        }


        private string CreateToken(User user) {
            _tokenBuilder.AddInformation("UserId", user.Id.ToString());
            _tokenBuilder.AddInformation("email", user.FullName.ToString());
            _tokenBuilder.AddInformation("UserTag", user.Tag.ToString());
            return _tokenBuilder.Build();
        }
      
    }
}
