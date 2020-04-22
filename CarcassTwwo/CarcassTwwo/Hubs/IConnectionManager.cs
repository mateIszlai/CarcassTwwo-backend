using CarcassTwwo.Models;
using System.Collections.Generic;

namespace CarcassTwwo.Hubs
{
    public interface IConnectionManager
    {
        void AddConnection(string username, string connectionId);
        void RemoveConnection(string connectionId);
        HashSet<Client> GetConnections(string username);
        IEnumerable<string> OnlineUsers { get; }
    }
}