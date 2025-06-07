using Microsoft.AspNetCore.Mvc.Filters;
using SGTC.Core.Apps.Interfaces;

namespace SGTC.Signature.API.Authorization
{
    public class AppUpdateManagerMiddleware : IResourceFilter
    {
        private readonly IAppManager _appManager;

        public AppUpdateManagerMiddleware(IAppManager appManager)
        {
            _appManager = appManager;
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            // Implement if needed, or keep empty
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            _appManager.SetuserSessionByAuth(context.HttpContext.User);
        }
    }
}