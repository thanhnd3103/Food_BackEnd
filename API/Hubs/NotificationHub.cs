using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string order, string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", order, message);
        }
    }
}
