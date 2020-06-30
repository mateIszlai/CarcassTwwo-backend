using CarcassTwwo.Models.Requests.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarcassTwwo.Models
{
    public class Board : IBoard
    {
        private CityHandler _cityHandler;
        private LandHandler _landHandler;
        private MonasteryHandler _monasteryHandle;
        private RoadHandler _roadHandler;

        public Dictionary<Coordinate, Card> CardCoordinates { get; set; }
        public Dictionary<RequiredCard, Coordinate> AvailableCoordinates { get; set; }

        public List <Meeple> RemovableMeeples = new List<Meeple>();

        public ScoreBoard ScoreBoard { get; set; }

        public Board(HashSet<Client> clients)
        {
            CardCoordinates = new Dictionary<Coordinate, Card>();
            AvailableCoordinates = new Dictionary<RequiredCard, Coordinate>();
            ScoreBoard = new ScoreBoard(clients);
            _monasteryHandle = new MonasteryHandler(this, ScoreBoard);
            _landHandler = new LandHandler(this, ScoreBoard);
            _cityHandler = new CityHandler(_landHandler, this, ScoreBoard);
            _roadHandler = new RoadHandler(_landHandler, this, ScoreBoard);
            _landHandler.SetCityCounter(_cityHandler);
            _monasteryHandle.SetNext(_landHandler).SetNext(_cityHandler).SetNext(_roadHandler);
        }

        public void AddAvailableCoordinates(Card card)
        {
            if (card.TopIsFree)
            {
                var top = new RequiredCard(new Coordinate { x = card.Coordinate.x, y = card.Coordinate.y + 1 }, CardCoordinates);
                RemoveFromAvailableCoordinates(top.Coordinate);
                AvailableCoordinates.Add(top, top.Coordinate);
            }

            if (card.LeftIsFree)
            {
                var left = new RequiredCard(new Coordinate { x = card.Coordinate.x - 1, y = card.Coordinate.y }, CardCoordinates);
                RemoveFromAvailableCoordinates(left.Coordinate);
                AvailableCoordinates.Add(left, left.Coordinate);
            }

            if (card.BottomIsFree)
            {
                var bottom = new RequiredCard(new Coordinate { x = card.Coordinate.x, y = card.Coordinate.y - 1 }, CardCoordinates);
                RemoveFromAvailableCoordinates(bottom.Coordinate);
                AvailableCoordinates.Add(bottom, bottom.Coordinate);
            }


            if (card.RightIsFree)
            {
                var right = new RequiredCard(new Coordinate { x = card.Coordinate.x + 1, y = card.Coordinate.y }, CardCoordinates);
                RemoveFromAvailableCoordinates(right.Coordinate);
                AvailableCoordinates.Add(right, right.Coordinate);
            }
        }

        public void RemoveFromAvailableCoordinates(Coordinate coordinate)
        {
            var item = AvailableCoordinates.FirstOrDefault(kvp => kvp.Value.Equals(coordinate));
            if (!item.Equals(default(KeyValuePair<RequiredCard, Coordinate>)))
                AvailableCoordinates.Remove(item.Key);
        }

        public void SetSideOccupation(Coordinate coord)
        {
            if (!CardCoordinates.TryGetValue(coord, out Card card))
            {
                throw new ArgumentException();
            }

            var vertical = new Coordinate { x = card.Coordinate.x, y = card.Coordinate.y + 1 };
            card.TopIsFree = CardCoordinates.ContainsKey(vertical) ? false : true;

            vertical.y = card.Coordinate.y - 1;
            card.BottomIsFree = CardCoordinates.ContainsKey(vertical) ? false : true;

            var horizontal = new Coordinate { x = card.Coordinate.x + 1, y = card.Coordinate.y };
            card.RightIsFree = CardCoordinates.ContainsKey(horizontal) ? false : true;

            horizontal.x = card.Coordinate.x - 1;
            card.LeftIsFree = CardCoordinates.ContainsKey(horizontal) ? false : true;

        }

        public void SetRegions(Coordinate coordinate)
        {
            if (!CardCoordinates.TryGetValue(coordinate, out Card card))
                throw new ArgumentException();
            
            bool roadClosed = SetRoadClosed(card);
            int landCounts = GetLandCount(card);
            CardCoordinates.TryGetValue(new Coordinate { x = coordinate.x, y = coordinate.y + 1 }, out Card topCard);
            CardCoordinates.TryGetValue(new Coordinate { x = coordinate.x, y = coordinate.y - 1 }, out Card botCard);
            CardCoordinates.TryGetValue(new Coordinate { x = coordinate.x + 1, y = coordinate.y }, out Card rightCard);
            CardCoordinates.TryGetValue(new Coordinate { x = coordinate.x - 1, y = coordinate.y }, out Card leftCard);
            Coordinate[] surroundingCoordinates = null;
            if(card.Tile.Field5.Name == "Monastery")
                surroundingCoordinates = new Coordinate[]
            {
                new Coordinate { x = coordinate.x - 1, y = coordinate.y + 1 },
                new Coordinate { x = coordinate.x, y = coordinate.y + 1 },
                new Coordinate { x = coordinate.x + 1, y = coordinate.y + 1 },
                new Coordinate { x = coordinate.x - 1, y = coordinate.y},
                new Coordinate { x = coordinate.x + 1, y = coordinate.y},
                new Coordinate { x = coordinate.x - 1, y = coordinate.y - 1},
                new Coordinate { x = coordinate.x, y = coordinate.y - 1},
                new Coordinate { x = coordinate.x + 1, y = coordinate.y - 1}
            };

            _monasteryHandle.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, roadClosed, surroundingCoordinates);
        }

        internal List<int> GetMeeplePlaces(int cardId)
        {
            var places = new List<int>();
            var card = CardCoordinates.First(c => c.Value.Id == cardId).Value;
            var ids = new List<int>();
            foreach (var id in card.PlaceIds)
            {
                switch(card.Tile.Sides.Where(s => s.Value.PlaceId == id).Select(t => t.Value.Name).First())
                {
                    case "City":
                        if (_cityHandler.Cities.First(c => c.Id == id).Meeples.Count == 0)
                            ids.Add(id);
                        break;
                    case "Road":
                        if (_roadHandler.Roads.First(r => r.Id == id).Meeples.Count == 0)
                            ids.Add(id);
                        break;
                    case "Land":
                        if (_landHandler.Lands.First(l => l.Id == id).Meeples.Count == 0)
                            ids.Add(id);
                        break;
                    case "Monastery":
                        ids.Add(id);
                        break;

                }
            }

            for (int i = 0; i < 9; i++)
            {
                if (ids.Contains(card.Tile.FieldsPlaceIds[i]))
                    places.Add(i + 1);
            }

            if (places.Contains(5) &&
                card.Tile.Field5.Name != "Monastery" &&
                card.Tile.Field5.Name != "Road" && GetCityCount(card) != 2 &&
                card.Sides.Count(s => s.Name == "City") <= 2 &&
                !(card.Tile.Field5.Name == "City" && GetLandCount(card) == 2 && card.Sides.Count(s => s.Name == "Road") == 0))
                places.Remove(5);

            return places;
        }

        private int GetCityCount(Card card)
        {
            if (card.Sides.Count(s => s.Name == "City") == 0)
                return 0;
            if (card.Tile.Field5.Name == "Land")
            {
                return card.Sides.Count(s => s.Name == "City");
            }
            return 1;
        }

        private int GetLandCount(Card card)
        {
            var roadCount = card.Sides.Count(s => s.Name == "Road");
            if (roadCount != 0)
            {
                if (card.Sides.Count(s => s.Name == "City") == 3)
                    return 2;
                return roadCount;
            }
            if (card.Top.Name == "City" && card.Bottom.Name == "City" && card.Tile.Field5.Name == "City" ||
                card.Left.Name == "City" && card.Right.Name == "City" && card.Tile.Field5.Name == "City")
                return card.Sides.Count(s => s.Name == "Land");

            if (card.Sides.Count(l => l.Name == "Land") >= 2)
                return 1;

            return 0;
        }

        private bool SetRoadClosed(Card card)
        {
            var names = new string[] { "Monastery", "Other" };
            return names.Contains(card.Tile.Field5.Name) || card.Sides.Count(s => s.Name == "City") == 3;
        }

        internal void PlaceMeeple(int placeOfMeeple, int cardId, Client owner)
        {
            var placedCard = CardCoordinates.First(c => c.Value.Id == cardId).Value;
            _monasteryHandle.HandleMeeplePlacement(placeOfMeeple, placedCard, owner);
        }

        internal void CountScores()
        {
            RemovableMeeples.Clear();
            _monasteryHandle.HandleScore(RemovableMeeples);
        }

        internal Dictionary<Client,int> CountEndScores()
        {
            _monasteryHandle.HandleEndScore();
            return ScoreBoard.Players;
        }

        internal Client CheckWinner()
        {
            return ScoreBoard.GetWinner();
        }
    }
}
