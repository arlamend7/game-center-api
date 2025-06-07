
using System;

namespace SGTC.Common.Sessions.Interfaces
{
    public interface IUserSession
    {
        public Guid UserId { get;  }
        public string NickName { get;  }
        public string Tag { get;  }
        public string Email { get;  }
        public string FullName { get;  }
    }
}