using Microsoft.AspNetCore.SignalR;

namespace GameCenter.API.signals
{
    public class ConnectFourHub : Hub
    {
        public async Task Play(int column)
        {
            await Clients.Others.SendAsync("Opponent", column);
        }

        public async Task Winner(string user)
        {
            await Clients.Others.SendAsync("Winner", user);
        }
    }

    public class PlayDto
    {
        public string User { get; set; }
        public string Column { get; set; }
    }
}
