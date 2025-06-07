using SGTC.Common.Sessions.Interfaces;
using System;

namespace SGTC.Common.Sessions
{
    public class UserSession : IUserSession
    {
        public Guid UserId { get; set; }
        public string NickName { get; set; }
        public string Tag { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }

    }
}