using System.Linq;
using GameCenter.Domain.Models.Players.Entities;
using GameCenter.Domain.Services.Interfaces;
using GameCenter.Infra;
using MongoDB.Driver;
using SGTC.Utilities.Encryptors.Interfaces;

namespace GameCenter.Domain.Services
{
    internal class UserAppService : IUserAppService
    {
        private readonly IMongoCollection<User> _usersDb;
        private readonly IAesOperation _aesOperation;

        public UserAppService(IMongoDbContext mongoDb, IAesOperation aesOperation)
        {
            _usersDb = mongoDb.Database.GetCollection<User>("users");
            _aesOperation = aesOperation;
        }

        public bool ValidTag(string nickname, string tag)
        {
            return !_usersDb.Find(x => x.NickName == nickname && x.Tag == tag).Any();
        }

        public bool AddUser(User user)
        {
            var anotherUser = _usersDb.Find(x => x.Email == user.Email).Any();  
            if (anotherUser)
            {
                return false;
            }

            user.Password = _aesOperation.Encrypt(user.Password);

            _usersDb.InsertOne(user);
            return true;
        }

        public User Login(string email, string password)
        {
            password = _aesOperation.Encrypt(password);
            return _usersDb.Find(x => x.Email == email && x.Password == password).FirstOrDefault();
        }

        public bool IsCorrectPin(string email, string pin)
        {
            return _usersDb.Find(x => x.Email == email && x.Pin == pin).Any();
        }

        public User ChangePassword(string email, string pin, string newPassword)
        {
            newPassword = _aesOperation.Encrypt(newPassword);

            var user = _usersDb.Find(x => x.Email == email && x.Pin == pin).FirstOrDefault();
            _usersDb.UpdateOne(x => x.Email == email && x.Pin == pin, Builders<User>.Update.Set(x => x.Password, newPassword));

            return user;
        }

        public User GetUser(string nickname, string tag)
        {
            return _usersDb.Find(x => x.NickName == nickname && x.Tag == tag).FirstOrDefault();
        }
    }
}
