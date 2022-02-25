using Microsoft.AspNetCore.SignalR;

namespace Application.Common.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly IHubContext<NotificationHub> hub;

        public NotificationHub(IHubContext<NotificationHub> hub)
        {
            this.hub = hub;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
