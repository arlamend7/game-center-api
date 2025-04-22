using GameCenter.Domain.Models.Players.Entities;

namespace GameCenter.Domain.Services.Interfaces
{
    public interface IUserAppService
    {
        bool AddUser(User user);
        User ChangePassword(string email, string pin, string newPassword);
        bool IsCorrectPin(string email, string pin);
        User Login(string email, string password);
        User GetUser(string nickname, string pin);
        bool ValidTag(string nickname, string tag);
    }
}