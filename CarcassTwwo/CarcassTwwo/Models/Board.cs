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
            // If there is monastery near the card set them
            var monasteries = _monasteries.Where(m => m.SurroundingCoordinates.Contains(coordinate)).ToList();
            monasteries.ForEach(m => m.SurroundingCoordinates.Remove(coordinate));

            bool roadClosed = SetRoadClosed(card.Tile.Field5.LandType);
            int landCounts = GetLandCount(card);
            var topCoord = new Coordinate { x = coordinate.x, y = coordinate.y + 1 };
            var botCoord = new Coordinate { x = coordinate.x, y = coordinate.y - 1 };
            var rightCoord = new Coordinate { x = coordinate.x + 1, y = coordinate.y };
            var leftCoord = new Coordinate { x = coordinate.x - 1, y = coordinate.y };

            var modifiedCityParts = new List<CityPart>();

            if (card.Tile.Field5.LandType.Name == "Monastery")
            {
                id++;
                _monasteries.Add(new Monastery(id, coordinate));
                card.MonasteryId = id;
            }

            PlaceCity(topCoord, botCoord, leftCoord, rightCoord, card);
        }

        private void PlaceCity(Coordinate topCoord, Coordinate botCoord, Coordinate leftCoord, Coordinate rightCoord, Card card)
        {

            int cityCounts = GetCityCount(card);
            if (cityCounts == 2)
            {
                PlaceTwoCity( topCoord,  botCoord,  leftCoord,  rightCoord,  card);
            }



        }

        private void PlaceTwoCity(Coordinate topCoord, Coordinate botCoord, Coordinate leftCoord, Coordinate rightCoord, Card card)
        {
            if (card.Top.Name == "City")
            {

                if (CardCoordinates.TryGetValue(topCoord, out Card topCard))
                {
                    var city = _cities.FirstOrDefault(c => c.Id == topCard.Bottom.PlaceId);
                    card.Top.PlaceId = city.Id;
                    if (city.GetCityPartByCardId(card.Id) == null)
                        city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false });
                    city.SetSides(topCard.Id, Side.BOTTOM);
                }
                else
                {
                    id++;
                    _cities.Add(new City(id, card.Id));
                    card.Top.PlaceId = id;
                }
            }
            if (card.Left.Name == "City")
            {

                if (CardCoordinates.TryGetValue(leftCoord, out Card leftCard))
                {
                    var city = _cities.FirstOrDefault(c => c.Id == leftCard.Right.PlaceId);
                    card.Left.PlaceId = city.Id;
                    if (city.GetCityPartByCardId(card.Id) == null)
                        city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false });
                    city.SetSides(leftCard.Id, Side.RIGHT);
                }
                else
                {
                    id++;
                    _cities.Add(new City(id, card.Id));
                    card.Left.PlaceId = id;
                }
            }
            if (card.Right.Name == "City")
            {

                if (CardCoordinates.TryGetValue(rightCoord, out Card rightCard))
                {
                    var city = _cities.FirstOrDefault(c => c.Id == rightCard.Right.PlaceId);
                    card.Left.PlaceId = city.Id;
                    if (city.GetCityPartByCardId(card.Id) == null)
                        city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false });
                    city.SetSides(rightCard.Id, Side.LEFT);
                }
                else
                {
                    id++;
                    _cities.Add(new City(id, card.Id));
                    card.Right.PlaceId = id;
                }
            }
            if (CardCoordinates.TryGetValue(botCoord, out Card botCard))
            {
                var city = _cities.FirstOrDefault(c => c.Id == botCard.Top.PlaceId);
                card.Bottom.PlaceId = city.Id;
                if (city.GetCityPartByCardId(card.Id) == null)
                    city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false });
                city.SetSides(botCard.Id, Side.BOTTOM);
            }
            else
            {
                id++;
                _cities.Add(new City(id, card.Id));
                card.Bottom.PlaceId = id;
            };
        }

        private int GetCityCount(Card card)
        {
            if (card.Sides.Count(s => s.Name == "City") == 0)
                return 0;
            if(card.Tile.Field5.LandType.Name == "Land")
            {
                if (card.Top.Name == "City" && card.Bottom.Name == "City" || card.Left.Name == "City" && card.Right.Name == "City")
                    return 2;
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
