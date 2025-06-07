using Microsoft.Extensions.DependencyInjection;
using SGTC.Common.Sessions;
using SGTC.Common.Sessions.Interfaces;
using System;
using System.Security.Claims;

namespace SGTC.Core.Apps.Interfaces
{
    public interface IAppManager
    {
        IUserSession GetUserSession();
        void UpdateUserSession(IUserSession updateUser);
        void UpdateUserSession(Action<UserSession> updateUser);
        void SetuserSessionByAuth(ClaimsPrincipal claimsPrincipal);
        IServiceScope CreateScope<TResponse>();
        TResponse CreateScope<TResponse>(Func<IServiceScope, IServiceProvider, TResponse> action);
        void CreateScope(Action<IServiceScope, IServiceProvider> action);
        void ExecuteAsync(string description, Action<IServiceScope, IServiceProvider> action);
    }
}