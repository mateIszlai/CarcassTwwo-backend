﻿using CarcassTwwo.Models;
using System.Collections.Generic;
using System.Linq;

namespace CarcassTwwo.Hubs
{
    public class ConnectionManager : IConnectionManager
    {
        private readonly Dictionary<string, Group> _groups = new Dictionary<string, Group>();

        public void AddConnection(string groupName, string username, bool isOwner, string connectionId)
        {
            lock (_groups)
            {
                if (!_groups.ContainsKey(groupName))
                {
                    _groups[groupName] = new Group();
                }
                _groups[groupName].Members.Add(new Client { ConnectionId = connectionId, Name = username, IsOwner = isOwner, IsPlaying = false });
            }
        }

        public HashSet<Client> GetConnections(string groupName)
        {
            Group group;
            if (_groups.TryGetValue(groupName, out group))
            {
                return group.Members;
            }

            return new HashSet<Client>();
        }

        public void StartGame(string groupName)
        {
            Group group;
            if (_groups.TryGetValue(groupName, out group))
            {
                group.Game = new Game(group.Members);
                group.Game.ShuffleCards(5);
            }
        }

        public HashSet<string> GetGroupNames()
        {
            return _groups.Keys.ToHashSet();
        }

        public void RemoveConnection(string connectionId)
        {
            lock (_groups)
            {

                foreach(var groupName in _groups.Keys)
                {
                    _groups[groupName].Members.RemoveWhere(client => client.ConnectionId == connectionId);
                    if (_groups[groupName].Members.Count == 0)
                        _groups.Remove(groupName);
                }
            }
        }

        public Group GetGroup(string groupName)
        {
            Group group;
            if (_groups.TryGetValue(groupName, out group))
                return group;
            return null;
        }

        public void RemoveGroup(string groupName)
        {
            _groups.Remove(groupName);
        }

    }
}
