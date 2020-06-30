using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarcassTwwo
{
    internal interface IHubNotificationHelper
    {
        void SendNotificationToAll(string message);
        IEnumerable<string> GetOnlineUsers();
        Task<Task> SendNotificationParallel(string username);
    }
}