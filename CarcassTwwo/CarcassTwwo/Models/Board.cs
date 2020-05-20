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
        public List<City> Cities { get; set; }
        public List<GrassLand> GrassLands { get; set; }
        public List<Monastery> Monasteries { get; set; }
        public List<Road> Roads { get; set; }

        public Board()
        {
            CardCoordinates = new Dictionary<Coordinate, Card>();
            AvailableCoordinates = new Dictionary<RequiredCard, Coordinate>();
            Cities = new List<City>();
            GrassLands = new List<GrassLand>();
            Monasteries = new List<Monastery>();
            Roads = new List<Road>();
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
            var card = new Card();
            if(!CardCoordinates.TryGetValue(coord, out card))
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

        public void SetRegions(Card card)
        {
            if (card.TopIsFree)
            {
                var top = new Coordinate() { x = card.Coordinate.x, y = card.Coordinate.y + 1 };
                switch (card.Top.Name)
                {
                    case "City":
                        CreateCity(top);
                        break;
                    case "Road":
                        CreateRoad(top);
                        break;
                    case "Land":
                        CreateLand(top);
                        break;

                }
            }
            if (card.LeftIsFree)
            {
                var left = new Coordinate() { x = card.Coordinate.x -1, y = card.Coordinate.y };
                switch(card.Left.Name){
                    case "City":
                        CreateCity(left);
                        break;
                    case "Road":
                        CreateRoad(left);
                        break;
                    case "Land":
                        CreateLand(left);
                        break;
                }
                
            }
            if (card.BottomIsFree)
            {
                var bottom = new Coordinate() { x = card.Coordinate.x, y = card.Coordinate.y - 1 };
                switch (card.Bottom.Name)
                {
                    case "City":
                        CreateCity(bottom);
                        break;
                    case "Road":
                        CreateRoad(bottom);
                        break;
                    case "Land":
                        CreateLand(bottom);
                        break;

                }
            }
            if (card.RightIsFree)
            {
                var right = new Coordinate() { x = card.Coordinate.x + 1, y = card.Coordinate.y };
                switch (card.Right.Name)
                {
                    case "City":
                        CreateCity(right);
                        break;
                    case "Road":
                        CreateRoad(right);
                        break;
                    case "Land":
                        CreateLand(right);
                        break;
                }
            }
        }

        private void CreateLand(Coordinate top)
        {
            throw new NotImplementedException();
        }

        private void CreateRoad(Coordinate top)
        {
            throw new NotImplementedException();
        }

        public void CreateCity(Coordinate coordinate)
        {
            foreach(City city in Cities)
            {
                
            }
        }
    }
}
