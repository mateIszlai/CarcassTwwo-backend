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
        private Board _gameboard;
        private ScoreBoard _scoreBoard;

        public int GameId { get; set; }
        public bool IsOver { get; set; }
        public bool IsStarted { get; set; }
        public int RoundsLeft { get; set; }
        public List<Client> Players { get; set; }
        public string WinnerName { get; set; }
        public Client LastPlayer { get; set; }

        public Game(HashSet<Client> players)
        {
            _gameboard = new Board();
            _scoreBoard = new ScoreBoard(players);
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

            return lastPlayerIndex == Players.Count - 1 ? Players[0] : Players[lastPlayerIndex + 1];
        }

        public Card PickRandomCard()
        {
            return _cards.Count == 0 ? null : _cards[rnd.Next(_cards.Count)];

        }
        public Card PlaceFirstCard()
        {
            var card = GetStarterCard();
            var cardToRecieve = new CardToRecieve() { 
                CardId = card.Id, 
                Coordinate = new Coordinate { x = 0, y = 0 }, 
                Rotation = "0" 
            };
            PlaceCard(cardToRecieve);
            return card;
        }

        public void PlaceCard(CardToRecieve placedCard)
        {
            var card = _cards.First(c => c.Id == placedCard.CardId);
            card.Coordinate = placedCard.Coordinate;
            var sides = card.Rotations[placedCard.Rotation];
            card.Top = sides[0];
            card.Left = sides[1];
            card.Bottom = sides[2];
            card.Right = sides[3];
            _gameboard.CardCoordinates.Add(placedCard.Coordinate, card);
            _gameboard.RemoveFromAvailableCoordinates(placedCard.Coordinate);
            _gameboard.SetSideOccupation(placedCard.Coordinate);
            _gameboard.AddAvailableCoordinates(card);
            _cards.Remove(card);
            _gameboard.SetRegions(placedCard.Coordinate);
        }


        public Dictionary<Coordinate, List<int>> GetPossiblePlacements(Card card)
        {
            var placementsWithRotations = new Dictionary<Coordinate,List<int>>();

            var rot0 = card.Rotations["0"];
            var rot90 = card.Rotations["90"];
            var rot180 = card.Rotations["180"];
            var rot270 = card.Rotations["270"];

            foreach(var place in _gameboard.AvailableCoordinates)
            {
                if(SidesMatches(place.Key, rot0) == true)
                {
                    if (placementsWithRotations.ContainsKey(place.Value))
                    {
                        placementsWithRotations[place.Value].Add(0);
                    }
                    else 
                        placementsWithRotations.Add(place.Value, new List<int> { 0 });
                }

                if (SidesMatches(place.Key, rot90) == true)
                {
                    if (placementsWithRotations.ContainsKey(place.Value))
                    {
                        placementsWithRotations[place.Value].Add(90);
                    }
                    else
                        placementsWithRotations.Add(place.Value, new List<int> { 90 });
                }

                if (SidesMatches(place.Key, rot180) == true)
                {
                    if (placementsWithRotations.ContainsKey(place.Value))
                    {
                        placementsWithRotations[place.Value].Add(180);
                    }
                    else
                        placementsWithRotations.Add(place.Value, new List<int> { 180 });
                }

                if (SidesMatches(place.Key, rot270) == true)
                {
                    if (placementsWithRotations.ContainsKey(place.Value))
                    {
                        placementsWithRotations[place.Value].Add(270);
                    }
                    else
                        placementsWithRotations.Add(place.Value, new List<int> { 270 });
                }
            }

            return placementsWithRotations;

        }

        public bool SidesMatches(RequiredCard req, List<LandType> sides)
        {
            bool topIsGood, leftIsGood, bottomIsGood, rightIsGood;
            topIsGood = leftIsGood = bottomIsGood = rightIsGood = true;

            if(req.Top != null)
            {
                topIsGood = sides[0] == req.Top ? true : false;
            }
            if(req.Left != null)
            {
                leftIsGood = sides[1] == req.Left ? true : false;
            }
            if (req.Bottom != null)
            {
                bottomIsGood = sides[2] == req.Bottom ? true : false;
            }
            if (req.Right != null)
            {
                rightIsGood = sides[3] == req.Right ? true : false;
            }

            return topIsGood && leftIsGood && bottomIsGood && rightIsGood;
        }

        public CardToSend GenerateCardToSend(Card card)
        {
            CardToSend cardToSend = new CardToSend(card.Tile.Id, card.Id);
            var possiblePlacesOfCard = GetPossiblePlacements(card);

            foreach(var placement in possiblePlacesOfCard)
            {
                cardToSend.CoordinatesWithRotations.Add(new CoordinatesWithRotation { Coordinate = placement.Key, Rotations = placement.Value});
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
