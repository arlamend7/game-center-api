using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

public class RoomHub : Hub
{
    [Authorize]
    public override async Task OnConnectedAsync()
    {
        var roomId = Context.GetHttpContext()?.Request.Query["roomId"].ToString();

        if (!string.IsNullOrEmpty(roomId))
        {
            // Add the connection to a group based on the roomId
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            Console.WriteLine($"Connection {Context.ConnectionId} joined room {roomId}");
        }
        else
        {
            Console.WriteLine("No roomId provided in the URL query string.");
        }

        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string message)
    {
        var roomId = Context.GetHttpContext()?.Request.Query["roomId"].ToString();
        if (!string.IsNullOrEmpty(roomId))
        {
            // Broadcast the message to the group based on roomId
            await Clients.Group(roomId).SendAsync("SendMessage", message);
            Console.WriteLine($"Message sent to room {roomId}: {message}");
        }
        else
        {
            Console.WriteLine("Cannot send message, no roomId found in URL.");
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var roomId = Context.GetHttpContext()?.Request.Query["roomId"].ToString();

        if (!string.IsNullOrEmpty(roomId))
        {
            // Remove the connection from the group
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            Console.WriteLine($"Connection {Context.ConnectionId} left room {roomId}");
        }

        await base.OnDisconnectedAsync(exception);
    }
}
