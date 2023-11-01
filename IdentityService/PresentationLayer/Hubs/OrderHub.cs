using Microsoft.AspNetCore.SignalR;

namespace PresentationLayer.Hubs
{
    public class OrderHub : Hub
    {
        public async Task JoinRoom(string username, string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("ReceiveMessage", "Server", $"{username} has joined");
        }

        public async Task SendMessage(string message, string username, string roomId)
        {
            await Clients.Group(roomId).SendAsync("ReceiveMessage", username, message);
        }
    }
}