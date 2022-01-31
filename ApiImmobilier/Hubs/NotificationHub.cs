using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiImmobilier.Hubs
{
    public class NotificationHub : Hub
    {

        public Task SendNotificationToAll()
        {
            return Clients.All.SendAsync("UpdateData", true);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception es)
        {
            await Clients.All.SendAsync("UserDisConnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(es);
        }
    }
}
