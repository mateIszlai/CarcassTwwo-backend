using CarcassTwwo.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Hubs
{
    public class ConnectionManager : IConnectionManager
    {
        private static Dictionary<string, HashSet<Client>> userMap = new Dictionary<string, HashSet<Client>>();
        public IEnumerable<string> OnlineUsers{ get { return userMap.Keys; }}
        IHubContext<LobbyHub> _hubContext { get; }

        public ConnectionManager(IHubContext<LobbyHub> context)
        {
            _hubContext = context;
        }

        public void AddConnection(string username, string connectionId)
        {
            lock (userMap)
            {
                if (!userMap.ContainsKey(username))
                {
                    userMap[username] = new HashSet<Client>();
                }
                userMap[username].Add(new Client { ConnectionId = connectionId, Name = username, IsPlaying = false });
            }
        }

        public HashSet<Client> GetConnections(string username)
        {
            var conn = new HashSet<Client>();
            try
            {
                lock (userMap)
                {
                    conn = userMap[username];
                }
            }
            catch
            {
                conn = null;
            }
            return conn;
        }

        public void RemoveConnection(string connectionId)
        {
            lock (userMap)
            {
                foreach(var username in userMap.Keys)
                {
                    if (userMap.ContainsKey(username))
                    {
                        foreach(var client in userMap[username])
                        {
                            if(client.ConnectionId == connectionId)
                            {
                                userMap.Remove(connectionId);
                                break;
                            }
                        }
                    }
                }
            }
        }

    }
}
