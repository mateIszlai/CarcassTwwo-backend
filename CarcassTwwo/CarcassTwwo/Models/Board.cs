using CarcassTwwo.Models.Places;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models
{
    public class Board
    {
        public Dictionary<Coordinate, Card> CardCoordinates { get; set; }
        public Dictionary<RequiredCard, Coordinate> AvailableCoordinates { get; set; }

        private HashSet<City> _cities;
        private HashSet<GrassLand> _grassLands;
        private HashSet<Monastery> _monasteries;
        private HashSet<Road> _roads;


        private int id = 0;

        public Board()
        {
            CardCoordinates = new Dictionary<Coordinate, Card>();
            AvailableCoordinates = new Dictionary<RequiredCard, Coordinate>();
            _cities = new HashSet<City>();
            _grassLands = new HashSet<GrassLand>();
            _monasteries = new HashSet<Monastery>();
            _roads = new HashSet<Road>();
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
            if(!item.Equals(default(KeyValuePair<RequiredCard, Coordinate>)))
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
            {
                throw new ArgumentException();
            }

            var monasteries = _monasteries.Where(m => m.SurroundingCoordinates.Contains(coordinate)).ToList();
            monasteries.ForEach(m => m.SurroundingCoordinates.Remove(coordinate));

            bool roadClosed = SetRoadClosed(card.Tile.Field5.LandType);
            int landCounts = GetLandCount(card);

            
            if(card.Tile.Field5.LandType.Name == "Monastery")
            {
                _monasteries.Add(new Monastery(id++, coordinate));
            }

            if (card.TopIsFree)
            {

            }
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
            if (card.Top.Name == "City" && card.Bottom.Name == "City" && card.Tile.Field5.LandType.Name == "City" ||
                card.Left.Name == "City" && card.Right.Name == "City" && card.Tile.Field5.LandType.Name == "City") 
                return card.Sides.Count(s => s.Name == "Land");
            
            if (card.Sides.Count(l => l.Name == "Land") >= 2)
                return 1;

            return 0;
        }

        private bool SetRoadClosed(LandType landType)
        {
            var names = new string[] { "Monastery", "City", "Other" };
            return names.Contains(landType.Name);
        }
    }
}
