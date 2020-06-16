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
            var group = _manager.GetGroup(groupName);
            var card = group.Game.PlaceFirstCard();
            var cardToSend = new CardToSend(card.Tile.Id,card.Id);
            cardToSend.CoordinatesWithRotations.Add(new CoordinatesWithRotation { Coordinate = card.Coordinate, Rotations = new List<int> { 0 } });
            await Clients.Group(groupName).SendAsync("StartGame", "The game is started", cardToSend);
        }

        public void Ready(string groupName)
        {
            _manager.GetConnections(groupName).First(player => player.ConnectionId == Context.ConnectionId).Ready = true;

            StartTurn(groupName);

        }

        public async void StartTurn(string groupName)
        {

            if (_manager.GetConnections(groupName).Where(c => !c.Ready).Count() != 0)
                return;
            var group = _manager.GetGroup(groupName);

            var playerInfos = group.Game.GeneratePlayerInfos();

            await Clients.Group(groupName).SendAsync("UpdatePlayers", playerInfos);
            var player = group.Game.PickPlayer();
            var card = group.Game.PickRandomCard();
            if(card == null)
            {
                var scores = _manager.GetGroup(groupName).Game.CheckEndScores();
                var winner = _manager.GetGroup(groupName).Game.CheckWinner();
                await Clients.Group(groupName).SendAsync("GameOver", "Game over!", scores, winner);

            } else
            {
                var cardToSend = group.Game.GenerateCardToSend(card);

                await Clients.Client(player.ConnectionId).SendAsync("Turn", cardToSend);
            }
        }

        public async void EndPlacement(string groupName, CardToRecieve card)
        {
            var group = _manager.GetGroup(groupName);
            group.Game.PlaceCard(card);
            if(_manager.GetGroup(groupName).Game.LastPlayer.MeepleCount > 0)
                await Clients.Client(Context.ConnectionId).SendAsync("PlaceMeeple", group.Game.GenerateMeeplePlaces(card.CardId));
            else
            {
                EndTurn(groupName, -1, card);
            }

        }
        public async void EndTurn(string groupName, int placeOfMeeple, CardToRecieve card)
        {
            var group = _manager.GetGroup(groupName);
            if(placeOfMeeple > 0) group.Game.PlaceMeeple(placeOfMeeple, card);
            group.Game.CheckScores();
            var meeplesToRemove = group.Game.GetRemovableMeeples();

            await Clients.Group(groupName).SendAsync("UpdatePlayers", group.Game.GeneratePlayerInfos());
            await Clients.GroupExcept(groupName, Context.ConnectionId).SendAsync("RefreshBoard", card, meeplesToRemove);
            StartTurn(groupName);
            

           
            
        }

        public async Task<string> GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}
