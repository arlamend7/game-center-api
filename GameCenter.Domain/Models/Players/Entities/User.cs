using System;
using System.Collections.Generic;
using System.Text;

namespace GameCenter.Domain.Models.Players.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public string Email { get; set; }
        public string Pin { get; set; }

    }
}
