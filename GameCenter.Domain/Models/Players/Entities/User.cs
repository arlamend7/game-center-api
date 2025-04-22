using System;
using System.Linq;
using GameCenter.Common.Entities;

namespace GameCenter.Domain.Models.Players.Entities
{
    public class User : EntityBase
    {
        public string NickName { get; set; }
        public string FullName  => string.Join(" ", new[] { FirstName, MiddleName, LastName}.Where(x => string.IsNullOrWhiteSpace(x)));
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Tag { get; set; }
        public DateTime Birthdate { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Pin { get; set; }
        public UserPreference Preferences { get; set; } = new UserPreference();
    }
}
