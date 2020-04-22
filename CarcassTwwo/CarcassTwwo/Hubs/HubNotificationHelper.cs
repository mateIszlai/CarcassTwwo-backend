using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Hubs
{
    public class HubNotificationHelper : IHubNotificationHelper
    {
        IHubContext<LobbyHub> _hubContext { get; }
        private readonly IConnectionManager _connectionManager;

        public HubNotificationHelper(IHubContext<LobbyHub> hubContext, IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
            _hubContext = hubContext;
        }

        public void SendNotificationToAll(string message)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetOnlineUsers()
        {
            throw new NotImplementedException();
        }

        public async Task<Task> SendNotification(string username)
        {
            HashSet<string> connections = _connectionManager.GetConnections(username);

            try
            {
                if(connections != null && connections.Count > 0)
                {
                    foreach(var conn in connections)
                    {
                        try
                        {
                            await _hubContext.Clients.Clients(conn).SendAsync("socket", null);
                        }
                        catch
                        {
                            throw new Exception("ERROR: No connections found");
                        }
                    }
                }
                return Task.CompletedTask;
            }
            catch
            {
                throw new Exception("ERROR");
            }
        }

        public Task<Task> SendNotificationParallel(string username)
        {
            throw new NotImplementedException();
        }
    }
}
