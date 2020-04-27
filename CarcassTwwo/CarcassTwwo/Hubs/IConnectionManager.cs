using CarcassTwwo.Models;
using System.Collections.Generic;

namespace CarcassTwwo.Hubs
{
    public interface IConnectionManager
    {
        void AddConnection(string groupName, string username, bool isOwner, string connectionId);
        void RemoveConnection(string connectionId);
        HashSet<Client> GetConnections(string groupName);
        HashSet<string> GetGroupNames();
        void RemoveGroup(string groupName);
    }
}