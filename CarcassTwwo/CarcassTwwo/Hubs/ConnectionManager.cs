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
        private readonly Dictionary<string, HashSet<Client>> _groups = new Dictionary<string, HashSet<Client>>();


        public void AddConnection(string groupName, string username, bool isOwner, string connectionId)
        {
            lock (_groups)
            {
                if (!_groups.ContainsKey(groupName))
                {
                    _groups[groupName] = new HashSet<Client>();
                }
                _groups[groupName].Add(new Client { ConnectionId = connectionId, Name = username, IsOwner = isOwner, IsPlaying = false });
            }
        }

        public HashSet<Client> GetConnections(string groupName)
        {
            HashSet<Client> connections;
            if (_groups.TryGetValue(groupName, out connections))
            {
                return connections;
            }

            return new HashSet<Client>();
        }

        public HashSet<string> GetGroupIds()
        {
            return _groups.Keys.ToHashSet();
        }

        public void RemoveConnection(string connectionId)
        {
            lock (_groups)
            {

                foreach(var groupId in _groups.Keys)
                {
                    _groups[groupId].RemoveWhere(client => client.ConnectionId == connectionId);
                    if (_groups[groupId].Count == 0)
                        _groups.Remove(groupId);
                }
            }
        }

        public void RemoveGroup(string groupName)
        {
            _groups.Remove(groupName);
        }
    }
}
