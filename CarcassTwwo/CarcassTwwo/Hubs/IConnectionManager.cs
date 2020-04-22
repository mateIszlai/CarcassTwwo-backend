using System.Collections.Generic;

namespace CarcassTwwo.Hubs
{
    public interface IConnectionManager
    {
        void AddConnection(string username, string connectionId);
        void RemoveConnection(string connectionId);
        HashSet<string> GetConnections(string username);
        IEnumerable<string> OnlineUsers { get; }
    }
}