using CarcassTwwo.Models.Places;
using System;
using System.Collections.Generic;
using System.Linq;

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
            if (card.Tile.Field5.LandType.Name == "Monastery")
            {
                id++;
                _monasteries.Add(new Monastery(id, coordinate) { CardId = card.Id });
                card.MonasteryId = id;
            }

            var Places = new Dictionary<Side, int>();

            foreach (var city in PlaceCity(topCoord, botCoord, leftCoord, rightCoord, card))
                Places.Add(city.Key, city.Value);


            foreach (var road in PlaceRoad(topCoord, botCoord, leftCoord, rightCoord, card, roadClosed))
                Places.Add(road.Key, road.Value);

            // TODO PlaceLands


            foreach (var place in Places)
            {
                switch (place.Key)
                {
                    case Side.TOP:
                        card.Top.PlaceId = place.Value;
                        break;
                    case Side.LEFT:
                        card.Left.PlaceId = place.Value;
                        break;
                    case Side.BOTTOM:
                        card.Bottom.PlaceId = place.Value;
                        break;
                    case Side.RIGHT:
                        card.Top.PlaceId = place.Value;
                        break;
                }
            }
        }

        private Dictionary<Side, int> PlaceRoad(Coordinate topCoord, Coordinate botCoord, Coordinate leftCoord, Coordinate rightCoord, Card card, bool roadClosed)
        {
            var roadsCount = roadClosed ? card.Sides.Count(l => l.Name == "Road") : 1;

            var roadsAround = new Dictionary<Side, int>();

            if (CardCoordinates.TryGetValue(topCoord, out Card topCard) && topCard.Bottom.Name == "Road")
                roadsAround.Add(Side.TOP, topCard.Bottom.PlaceId);
            if (CardCoordinates.TryGetValue(leftCoord, out Card leftCard) && leftCard.Right.Name == "Road")
                roadsAround.Add(Side.LEFT, leftCard.Right.PlaceId);
            if (CardCoordinates.TryGetValue(botCoord, out Card botCard) && botCard.Top.Name == "Road")
                roadsAround.Add(Side.BOTTOM, botCard.Top.PlaceId);
            if (CardCoordinates.TryGetValue(rightCoord, out Card rightCard) && rightCard.Left.Name == "Road")
                roadsAround.Add(Side.RIGHT, rightCard.Left.PlaceId);

            if(roadsAround.Count == 0)
            {
                if (roadsCount != 1)
                {
                    for(int i = 0; i < roadsCount; i++)
                    {
                        id++;
                        var road = new Road(id);
                        var roadPart = new RoadPart(card.Id);
                        roadPart.LeftOpen = false;
                        road.ExpandRoad(roadPart);
                        _roads.Add(road);
                    }
                }
                else
                {
                    id++;
                    var road = new Road(id);
                    road.ExpandRoad(new RoadPart(card.Id));
                    _roads.Add(road);
                }
            }
            else
            {
                id++;
                var newRoad = new Road(id);

                foreach (var around in roadsAround)
                {
                    var road = _roads.First(r => r.Id == around.Value);
                    switch (around.Key)
                    {
                        case Side.TOP:
                            road.SetSides(topCard.Id);
                            break;
                        case Side.LEFT:
                            road.SetSides(leftCard.Id);
                            break;
                        case Side.BOTTOM:
                            road.SetSides(botCard.Id);
                            break;
                        case Side.RIGHT:
                            road.SetSides(rightCard.Id);
                            break;
                    }
                    if (roadClosed)
                    {
                        road.ExpandRoad(new RoadPart(card.Id) { LeftOpen = false, RightOpen = false });
                    }
                    else
                    {
                        roadsAround[around.Key] = newRoad.Id;
                        foreach (var rp in road.RoadParts)
                        {
                            newRoad.ExpandRoad(rp);
                            var roadCard = CardCoordinates.Values.FirstOrDefault(r => r.Id == rp.CardId);
                            if (roadCard.Top.PlaceId == road.Id)
                                roadCard.Top.PlaceId = newRoad.Id;
                            if (roadCard.Left.PlaceId == road.Id)
                                roadCard.Left.PlaceId = newRoad.Id;
                            if (roadCard.Bottom.PlaceId == road.Id)
                                roadCard.Bottom.PlaceId = newRoad.Id;
                            if (roadCard.Right.PlaceId == road.Id)
                                roadCard.Right.PlaceId = newRoad.Id;
                        }
                        _roads.Remove(road);
                    }
                    roadsCount--;
                }

                if(roadsCount >= 0)
                {
                    id--;
                    for(int i = 0; i < roadsCount; i++)
                    {
                        id++;
                        var road = new Road(id);
                        var roadPart = new RoadPart(card.Id);
                        roadPart.LeftOpen = false;
                        road.ExpandRoad(roadPart);
                        _roads.Add(road);
                    }
                }
                else
                {
                    _roads.Add(newRoad);
                }
            }

            return roadsAround;
        }

        private Dictionary<Side, int> PlaceCity(Coordinate topCoord, Coordinate botCoord, Coordinate leftCoord, Coordinate rightCoord, Card card)
        {
            int cityCounts = GetCityCount(card);
            if (cityCounts == 2)
            {
                return PlaceTwoCity( topCoord,  botCoord,  leftCoord,  rightCoord,  card);
            }

            var citiesAround = new Dictionary<Side, int>();

            if (CardCoordinates.TryGetValue(topCoord, out Card topCard) && topCard.Bottom.Name == "City")
                citiesAround.Add(Side.TOP, topCard.Bottom.PlaceId);
            if (CardCoordinates.TryGetValue(leftCoord, out Card leftCard) && leftCard.Right.Name == "City")
                citiesAround.Add(Side.LEFT, leftCard.Right.PlaceId);
            if (CardCoordinates.TryGetValue(botCoord, out Card botCard) && botCard.Top.Name == "City")
                citiesAround.Add(Side.BOTTOM, botCard.Top.PlaceId);
            if (CardCoordinates.TryGetValue(rightCoord, out Card rightCard) && rightCard.Left.Name == "City")
                citiesAround.Add(Side.RIGHT, rightCard.Left.PlaceId);

            var cityPart = new CityPart(card.Id);
            if (card.Top.Name != "City")
                cityPart.TopIsOpen = false;
            if (card.Left.Name != "City")
                cityPart.LeftIsOpen = false;
            if (card.Bottom.Name != "City")
                cityPart.BottomIsOpen = false;
            if (card.Right.Name != "City")
                cityPart.RightIsOpen = false;

            if (citiesAround.Count == 0)
            {
                id++;
                var city = new City(id);
                city.ExpandCity(cityPart);
                _cities.Add(city);
                if (card.Top.Name == "City")
                    citiesAround.Add(Side.TOP, id);
                else if (card.Left.Name == "City")
                    citiesAround.Add(Side.LEFT, id);
                else if (card.Bottom.Name == "City")
                    citiesAround.Add(Side.BOTTOM, id);
                else
                    citiesAround.Add(Side.RIGHT, id);
            }
            else
            {
                id++;
                var newCity = new City(id);
                foreach (var around in citiesAround)
                {
                    var city = _cities.FirstOrDefault(c => c.Id == around.Value);
                    switch (around.Key)
                    {
                        case Side.TOP:
                            city.SetSides(topCard.Id, Side.BOTTOM);
                            cityPart.TopIsOpen = false;
                            break;
                        case Side.LEFT:
                            city.SetSides(leftCard.Id, Side.RIGHT);
                            cityPart.LeftIsOpen = false;
                            break;
                        case Side.BOTTOM:
                            city.SetSides(botCard.Id, Side.TOP);
                            cityPart.BottomIsOpen = false;
                            break;
                        case Side.RIGHT:
                            city.SetSides(rightCard.Id, Side.BOTTOM);
                            cityPart.LeftIsOpen = false;
                            break;
                    }
                    if (citiesAround.Count == 1)
                    {
                        city.ExpandCity(cityPart);
                        id--;
                    }
                    else
                    {
                        citiesAround[around.Key] = newCity.Id;
                        foreach (var cp in city.CityParts)
                        {
                            newCity.ExpandCity(cp);
                            var cityCard = CardCoordinates.Values.FirstOrDefault(c => c.Id == cp.CardId);
                            if (cityCard.Top.PlaceId == city.Id)
                                cityCard.Top.PlaceId = newCity.Id;
                            if (cityCard.Left.PlaceId == city.Id)
                                cityCard.Left.PlaceId = newCity.Id;
                            if (cityCard.Bottom.PlaceId == city.Id)
                                cityCard.Bottom.PlaceId = newCity.Id;
                            if (cityCard.Right.PlaceId == city.Id)
                                cityCard.Right.PlaceId = newCity.Id;
                        }
                        _cities.Remove(city);
                    }
                }
                if (citiesAround.Count > 1)
                {
                    _cities.Add(newCity);
                }
            }
            return citiesAround;
        }

        private Dictionary<Side, int> PlaceTwoCity(Coordinate topCoord, Coordinate botCoord, Coordinate leftCoord, Coordinate rightCoord, Card card)
        {
            var toModify = new Dictionary<Side, int>();
            if (card.Top.Name == "City")
            {

                if (CardCoordinates.TryGetValue(topCoord, out Card topCard))
                {
                    var city = _cities.FirstOrDefault(c => c.Id == topCard.Bottom.PlaceId);
                    toModify.Add(Side.TOP, city.Id);
                    if (city.GetCityPartByCardId(card.Id) == null)
                        city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false });
                    city.SetSides(topCard.Id, Side.BOTTOM);
                }
                else
                {
                    id++;
                    var city = new City(id);
                    city.ExpandCity(new CityPart(card.Id) { TopIsOpen = true, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false });
                    _cities.Add(city);
                    toModify.Add(Side.TOP, id);
                }
            }

            if (card.Left.Name == "City")
            {

                if (CardCoordinates.TryGetValue(leftCoord, out Card leftCard))
                {
                    var city = _cities.FirstOrDefault(c => c.Id == leftCard.Right.PlaceId);
                    toModify.Add(Side.LEFT, city.Id);
                    if (city.GetCityPartByCardId(card.Id) == null)
                        city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false });
                    city.SetSides(leftCard.Id, Side.RIGHT);
                }
                else
                {
                    id++;
                    var city = new City(id);
                    city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = true, BottomIsOpen = false, RightIsOpen = false });
                    _cities.Add(city);
                    toModify.Add(Side.LEFT, id);

                }
            }

            if(card.Bottom.Name == "City")
            {
                if (CardCoordinates.TryGetValue(botCoord, out Card botCard))
                {
                    var city = _cities.FirstOrDefault(c => c.Id == botCard.Top.PlaceId);
                    toModify.Add(Side.BOTTOM, city.Id);
                    if (city.GetCityPartByCardId(card.Id) == null)
                        city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false });
                    city.SetSides(botCard.Id, Side.TOP);
                }
                else
                {
                    id++;
                    var city = new City(id);
                    city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = true, RightIsOpen = false });
                    _cities.Add(city);
                    toModify.Add(Side.BOTTOM, id);

                }
            }

            if (card.Right.Name == "City")
            {

                if (CardCoordinates.TryGetValue(rightCoord, out Card rightCard))
                {
                    var city = _cities.FirstOrDefault(c => c.Id == rightCard.Right.PlaceId);
                    toModify.Add(Side.RIGHT, city.Id);
                    if (city.GetCityPartByCardId(card.Id) == null)
                        city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false });
                    city.SetSides(rightCard.Id, Side.LEFT);
                }
                else
                {
                    id++;
                    var city = new City(id);
                    city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = true });
                    _cities.Add(city);
                    toModify.Add(Side.RIGHT, id);

                }
            }

            return toModify;
        }

        private int GetCityCount(Card card)
        {
            if (card.Sides.Count(s => s.Name == "City") == 0)
                return 0;
            if(card.Tile.Field5.LandType.Name == "Land")
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
