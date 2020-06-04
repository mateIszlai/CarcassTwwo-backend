using CarcassTwwo.Models;
using CarcassTwwo.Models.Requests;
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
            var card = _manager.GetGroup(groupName).Game.PlaceFirstCard();
            var cardToSend = new CardToSend(card.Tile.Id,card.Id);
            cardToSend.CoordinatesWithRotations.Add(new CoordinatesWithRotation { Coordinate = card.Coordinate, Rotations = new List<int> { 0 } });
            await Clients.Group(groupName).SendAsync("StartGame", "The game is started", cardToSend);
            StartTurn(groupName);
        }

        public async void StartTurn(string groupName)
        {
            var group = _manager.GetGroup(groupName);
            var player = group.Game.PickPlayer();
            var card = group.Game.PickRandomCard();
            if(card == null)
            {
                await Clients.Group(groupName).SendAsync("GameOver", "Game over!");

            } else
            {
                var cardToSend = group.Game.GenerateCardToSend(card);
                await Clients.Client(player.ConnectionId).SendAsync("Turn", cardToSend);
            }
        }

        public async void EndPlacement(string groupName, CardToRecieve card)
        {
            _manager.GetGroup(groupName).Game.PlaceCard(card);
            await Clients.Client(Context.ConnectionId).SendAsync("PlaceMeeple", new List<int> { 1, 2, 3, 4, 5, 7, 8, 9 });
        }
        public async void EndTurn(string groupName, int placeOfMeeple, CardToRecieve card)
        {
            _manager.GetGroup(groupName).Game.PlaceMeeple(placeOfMeeple, card);
            await Clients.GroupExcept(groupName, Context.ConnectionId).SendAsync("RefreshBoard", card);
            StartTurn(groupName);
        }
    }
}
