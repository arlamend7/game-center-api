using SGTC.Common.Sessions;
using SGTC.Common.Sessions.Interfaces;
using SGTC.Core.Apps.Interfaces;
using System.Security.Claims;
using System.Text;

namespace SGTC.Core.Apps
{
    internal class AppManager : IAppManager
    {
        private UserSession UserSession;
        private readonly IServiceProvider _serviceProvider;

        public AppManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            UserSession = new UserSession();
        }
        public IUserSession GetUserSession() => UserSession;

        public void UpdateUserSession(Action<UserSession> updateUser)
        {
            var user = new UserSession();

            updateUser(UserSession);
        }

        public void SetuserSessionByAuth(ClaimsPrincipal claimsPrincipal)
        {
            if(claimsPrincipal == null) return;

            var user = new UserSession();

            var properties = typeof(UserSession).GetProperties();
            foreach (var claim in claimsPrincipal.Claims)
            {
                var property = properties.FirstOrDefault(x => x.Name == claim.Type);
                if (property == null) continue;

                if (property.PropertyType == typeof(string) && claim.Value.EndsWith("="))
                {
                    property.SetValue(user, Encoding.UTF8.GetString(Convert.FromBase64String(claim.Value)));
                }
                else if (property.PropertyType == typeof(Guid))
                {
                    property.SetValue(user, Guid.Parse(claim.Value));

                }
                else
                {
                    property.SetValue(user, Convert.ChangeType(claim.Value, property.PropertyType));
                }

            }

            UpdateUserSession(user);
        }

        public void UpdateUserSession(IUserSession updateUser)
        {
            UserSession = (UserSession)updateUser;
        }

        public TResponse CreateScope<TResponse>(Func<IServiceScope, IServiceProvider, TResponse> action)
        {
            TResponse response = default;
            CreateScope((scope, provider) =>
            {
                response = action(scope, provider);
            });

            return response;
        }

        public void CreateScope(Action<IServiceScope, IServiceProvider> action)
        {
            using var scope = _serviceProvider.CreateScope();
            var appManager = scope.ServiceProvider.GetService<IAppManager>();

            appManager.UpdateUserSession(UserSession);

            action(scope, scope.ServiceProvider);
        }

        public IServiceScope CreateScope<TResponse>()
        {
            return _serviceProvider.CreateScope();
        }

        public void ExecuteAsync(string description, Action<IServiceScope, IServiceProvider> action)
        {
            Task.Run(() =>
            {
                Console.WriteLine(description);
                CreateScope((scope, provider) => action(scope, provider));
            });
        }
    }
}
