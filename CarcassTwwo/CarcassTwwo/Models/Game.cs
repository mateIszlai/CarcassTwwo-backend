﻿using CarcassTwwo.Models.Places;
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

        public int GameId { get; set; }
        public bool IsOver { get; set; }
        public bool IsStarted { get; set; }
        public int RoundsLeft { get; set; }
        public List<Client> Players { get; set; }
        public string WinnerName { get; set; }
        public Client LastPlayer { get; set; }

        public Game(HashSet<Client> players)
        {
            Players = players.ToList();
            IsOver = false;
            IsStarted = true;
            LastPlayer = Players[Players.Count - 1];
            DataSeeder.SeedLandTypes();
            DataSeeder.SeedTiles();
            _cards = GenerateDeck();
            _gameboard = new Board(players);
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

        public Client CheckWinner()
        {
            return _gameboard.CheckWinner();
        }

        public Client PickPlayer()
        {
            var lastPlayerIndex = Players.FindIndex(c => c.Equals(LastPlayer));
            LastPlayer = lastPlayerIndex == Players.Count - 1 ? Players[0] : Players[lastPlayerIndex + 1];
            return LastPlayer;
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
            _cards.Remove(card);
            card.Coordinate = placedCard.Coordinate;
            card.Rotate(placedCard.Rotation);
            _gameboard.CardCoordinates.Add(placedCard.Coordinate, card);
            _gameboard.RemoveFromAvailableCoordinates(placedCard.Coordinate);
            _gameboard.SetSideOccupation(placedCard.Coordinate);
            _gameboard.AddAvailableCoordinates(card);
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


        internal List<int> GenerateMeeplePlaces(int cardId)
        {
            return _gameboard.GetMeeplePlaces(cardId);
        }

        public bool SidesMatches(RequiredCard req, LandType[] sides)
        {
            bool topIsGood, leftIsGood, bottomIsGood, rightIsGood;
            topIsGood = leftIsGood = bottomIsGood = rightIsGood = true;

            if(req.Top != null)
            {
                topIsGood = sides[1].Name == req.Top.Name ? true : false;
            }
            if(req.Left != null)
            {
                leftIsGood = sides[3].Name == req.Left.Name ? true : false;
            }
            if (req.Bottom != null)
            {
                bottomIsGood = sides[7].Name == req.Bottom.Name ? true : false;
            }
            if (req.Right != null)
            {
                rightIsGood = sides[5].Name == req.Right.Name ? true : false;
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

        public void PlaceMeeple(int placeOfMeeple, CardToRecieve placedCard)
        {
            var card = _gameboard.CardCoordinates.Values.First(c => c.Id == placedCard.CardId);
            _gameboard.PlaceMeeple(placeOfMeeple, card, LastPlayer);
        }
        public Dictionary<Client,int> CheckScores()
        {
            return _gameboard.CountScores();
        }
        internal Dictionary<Client, int> CheckEndScores()
        {
            return _gameboard.CountEndScores();
        }

        internal List<Meeple> GetRemovableMeeples()
        {
            return _gameboard.RemovableMeeples;
        }
    }
}
