using CarcassTwwo.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarcassTwwo.Models
{
    public class Game
    {
        private List<Card> _cards;
        private static Random rnd = new Random();

        public int GameId { get; set; }
        public bool IsOver { get; set; }
        public bool IsStarted { get; set; }
        public int RoundsLeft { get; set; }
        public List<Client> Players { get; set; }
        public string WinnerName { get; set; }
        public Client LastPlayer { get; set; }
        public Board GameBoard { get; set; }

        public Game(HashSet<Client> players)
        {
            GameBoard = new Board();
            Players = players.ToList();
            IsOver = false;
            IsStarted = true;
            LastPlayer = Players[Players.Count - 1];
            DataSeeder.SeedLandTypes();
            DataSeeder.SeedTiles();
            _cards = GenerateDeck();
        }

        public void ShuffleCards(int times)
        {
            for(int i = 0; i < times; i++)
            {
                var n = _cards.Count;
                while (n > 1)
                {
                    int k = rnd.Next(n);
                    n--;
                    var card = _cards[k];
                    _cards[k] = _cards[n];
                    _cards[n] = card;
                }
            }
        }

        private List<Card> GenerateDeck()
        {
            List<Card> cards = new List<Card>();
            var id = 1;
            foreach(var tile in DataSeeder.tiles)
            {
                for(int i = 0; i< tile.Amount; i++)
                {
                    cards.Add(new Card(tile, id));
                    id++;
                }
            }
            return cards;
        }

        public void Play()
        {
            //TODO
            //params:
            //eg. card position, player, board size...
        }

        public void CheckWinner(List<Client> players)
        {
            int maxPoint = 0;
            foreach (var player in players)
            {
                if (player.Points > maxPoint)
                {
                    maxPoint = player.Points;
                    WinnerName = player.Name;
                }
            }
        }

        public Client PickPlayer()
        {
            var lastPlayerIndex = Players.FindIndex(c => c.Equals(LastPlayer));

            if (lastPlayerIndex == Players.Count - 1)
                LastPlayer = Players[0];
            else
                LastPlayer = Players[lastPlayerIndex + 1];

            return LastPlayer;
        }

        public Card PickRandomCard()
        {
            return _cards[rnd.Next(_cards.Count)];
        }
        public Card PlaceFirstCard()
        {
            var card = GetStarterCard();
            PlaceCard(new Coordinate { x = 0, y = 0 }, card.Id);
            return card;
        }

        public void PlaceCard(Coordinate coordinate, int cardId)
        {
            var card = _cards.First(c => c.Id == cardId);
            GameBoard.CardCoordinates.Add(coordinate, card);
            card.Coordinate = coordinate;

            GameBoard.RemoveFromAvailableCoordinates(coordinate);
            card.SetSideOccupation(card, GameBoard);
            GameBoard.AddAvailableCoordinates(card);
            _cards.Remove(card);
        }


        public Dictionary<RequiredCard, Coordinate> GetPossiblePlacements(Card card)
        {
            return GameBoard.AvailableCoordinates;
            /* this will check if the card can be placed. later.
            AvailableCoordinates contains ALL coordinates.
            we will only need those, that our card can be placed on.
            get those requiredCards, that matches our card 
             */

        }

        public CardToSend GenerateCardToSend(Card card)
        {
            CardToSend cardToSend = new CardToSend(card.Tile.Id);
            // Ezt még meg kell csinálni, itt adná hozzá a megfelelő helyeket és forgatásokat.
            foreach(var coordinate in GameBoard.AvailableCoordinates)
            {
                cardToSend.CoordinatesWithRotations.Add(new CoordinatesWithRotation { Coordinate = coordinate.Value, Rotations = new List<int> { 0, 90, 180, 270 } });
            }
            return cardToSend;
        }

        public Card GetStarterCard()
        {
            return _cards.First(c => c.Tile.Id == 20);
        }

        public Card GetFirstCard()
        {
            return _cards[0];
        }
    }
}
