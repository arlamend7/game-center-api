using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;


namespace SaendMe.Api.signalR
{
    public class ConnectionHub : Hub
    {

        [Authorize]
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public async Task Request(string group, string message)
        {
            await Clients.Group(group).SendAsync("requests", new { message, group });
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task CurrentLocation(string[] groups, string salesId)
        {
            var isSales = Context.GetHttpContext()?.Request.Query["type"].ToString() == "Sales";

            if (!isSales)
            {
                foreach (var group in groups)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, group);
                }
            }
            else
            {
                foreach (var group in groups)
                {
                    await Clients.Group(group).SendAsync("salesLocation", new { group, salesId });
                }
            }

        }

        public Task AddLocation(string group)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

        public Task RemoveLocation(string group)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
        }
    }
}