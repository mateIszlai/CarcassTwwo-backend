using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace CarcassTwwo.Hubs
{
    public class LobbyHub : Hub
    {
        private IConnectionManager _manager;
        public LobbyHub(IConnectionManager manager)
        {
            _manager = manager;
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var user = Context.UserIdentifier;
            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }

        public string GetConnectionId()
        {
            var httpContext = Context.GetHttpContext();
            var username = httpContext.Request.Query["username"];
            _manager.AddConnection(username, Context.ConnectionId);

            return Context.ConnectionId;
        }
    }
}
