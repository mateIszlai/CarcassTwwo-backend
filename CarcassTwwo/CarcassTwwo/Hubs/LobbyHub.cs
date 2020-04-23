using CarcassTwwo.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Hubs
{
    public class LobbyHub : Hub
    {
        private IConnectionManager _manager;

        public HashSet<string> _roomCodes { get; private set; }
        public Dictionary<string, LocalGroup> groups = new Dictionary<string, LocalGroup>();

        public LobbyHub(IConnectionManager manager)
        {
            _manager = manager;
            _roomCodes = new HashSet<string>();
        }

        public async Task<string> CreateGroup(string username)
        {
            var groupName = GenerateRoomString();
            await AddToGroup(groupName,username);
            return groupName;
        }

        public string GenerateRoomString()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const int length = 6;
            string roomCode;
            do
            {
                roomCode = new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            } while (_roomCodes.Contains(roomCode));
            return roomCode;
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task AddToGroup(string groupName, string userName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            
            if (!groups.ContainsKey(groupName))
            {
                groups.Add(groupName, new LocalGroup { Name = groupName, OwnerName = userName });
            } 

            groups[groupName].Members.Add(new Client { Name = userName, ConnectionId = Context.ConnectionId });
            
            await Clients.Group(groupName).SendAsync(
                "Send", 
                $"{Context.ConnectionId} has joined the group {groupName}.", 
                groups[groupName].Members
                );
        }

        public async Task RemoveFromGroup(string groupName, string userName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            foreach(var client in groups[groupName].Members)
            {
                if(client.Name == userName)
                {
                    groups[groupName].Members.Remove(client);
                }
            }

            await Clients.Group(groupName).SendAsync(
                "Send", 
                $"{Context.ConnectionId} has left the group {groupName}.", 
                groups[groupName].Members);
    
        }

        public void RemoveGroup(string groupName)
        {
            groups.Remove(groupName);
        }

        public List<Client> GetGroupMembers(string groupName)
        {
            return groups[groupName].Members;
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
