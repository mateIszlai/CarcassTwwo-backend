﻿using CarcassTwwo.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Hubs
{
    public class LobbyHub : Hub
    {
        private static readonly ConnectionManager _manager = new ConnectionManager();

        public async Task<string> CreateGroup(string username)
        {
            var groupName = GenerateRoomString();
            await AddToGroup(groupName, username, true);
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
            } while (_manager.GetGroupNames().Contains(roomCode));
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

        public async Task AddToGroup(string groupName, string userName, bool isOwner)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            _manager.AddConnection(groupName, userName, isOwner, Context.ConnectionId);
            await Clients.Group(groupName).SendAsync(
                "Send",
                $"{Context.ConnectionId} has joined the group {groupName}.",
                _manager.GetConnections(groupName).Select(c => c.Name)
                ) ;
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            _manager.RemoveConnection(Context.ConnectionId);

            await Clients.Group(groupName).SendAsync(
                "Send", 
                $"{Context.ConnectionId} has left the group {groupName}.", 
                _manager.GetConnections(groupName).Select(c => c.Name));
    
        }

        public async void GetGroupMembers(string groupName)
        {
            await Clients.Group(groupName).SendAsync("GroupNames",
                _manager.GetConnections(groupName));
        }

        public void RemoveGroup(string groupName)
        {
            _manager.RemoveGroup(groupName);
        }

        public async void StartGame(string groupName)
        {
            _manager.StartGame(groupName);
            await Clients.Group(groupName).SendAsync("StartGame", "The game is started");
            StartTurn(groupName);
        }

        public async void StartTurn(string groupName)
        {
            var group = _manager.GetGroup(groupName);
            var player = group.Game.PickPlayer();
            await Clients.Client(player.ConnectionId).SendAsync("Turn", "This is your turn", true);
        }
        public async void EndTurn(string groupName)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("EndTurn", "Your turn is ended, waiting for the others", false);
            StartTurn(groupName);
        }
    }
}
