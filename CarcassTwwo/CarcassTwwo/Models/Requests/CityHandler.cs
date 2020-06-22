using CarcassTwwo.Models.Places;
using System.Collections.Generic;
using System.Linq;

namespace CarcassTwwo.Models.Requests
{
    public class CityHandler : AbstractHandler
    {
        public HashSet<City> Cities { get; private set; }

        private ILandHandler _landHandler;
        private IBoard _board;

        public CityHandler(ILandHandler landHandler, IBoard board)
        {
            Cities = new HashSet<City>();
            _landHandler = landHandler;
            _board = board;
        }

        public override int Handle(Card topCard, Card botCard, Card leftCard, Card rightCard, Card card, int landCounts, int id, bool roadClosed)
        {
            int cityCounts = GetCityCount(card);

            if (cityCounts == 0)
                return base.Handle(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed);

            if (cityCounts == 2)
            {
                id =  PlaceTwoCity(topCard, botCard, leftCard, rightCard, card, id);
                return base.Handle(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed);
            }

            var citiesAround = new Dictionary<Side, int>();

            if (topCard != null && topCard.Bottom.Name == "City")
                citiesAround.Add(Side.TOP, topCard.Bottom.PlaceId);
            if (leftCard != null && leftCard.Right.Name == "City")
                citiesAround.Add(Side.MIDDLELEFT, leftCard.Right.PlaceId);
            if (botCard != null && botCard.Top.Name == "City")
                citiesAround.Add(Side.BOTTOM, botCard.Top.PlaceId);
            if (rightCard != null && rightCard.Left.Name == "City")
                citiesAround.Add(Side.MIDDLERIGHT, rightCard.Left.PlaceId);

            var cityPart = new CityPart(card.Id);
            cityPart.HasCrest = card.HasCrest;
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
                Cities.Add(city);
                foreach (var side in card.Tile.Sides)
                {
                    if (side.Value.Name == "City")
                        card.SetField(side.Key, id);
                }
                AddCityToLand(card, landCounts, city.Id);
                return base.Handle(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed);
            }

            if (citiesAround.Values.Distinct().Count() == 1)
            {
                var city = Cities.First(c => c.Id == citiesAround.First().Value);
                foreach (var around in citiesAround)
                {
                    switch (around.Key)
                    {
                        case Side.TOP:
                            cityPart.TopIsOpen = false;
                            city.SetSides(topCard.Id, Side.BOTTOM);
                            break;
                        case Side.MIDDLELEFT:
                            cityPart.LeftIsOpen = false;
                            city.SetSides(leftCard.Id, Side.MIDDLERIGHT);
                            break;
                        case Side.BOTTOM:
                            cityPart.BottomIsOpen = false;
                            city.SetSides(botCard.Id, Side.TOP);
                            break;
                        case Side.MIDDLERIGHT:
                            cityPart.RightIsOpen = false;
                            city.SetSides(rightCard.Id, Side.MIDDLELEFT);
                            break;
                    }
                }
                city.ExpandCity(cityPart);
                card.Tile.Sides.Where(s => s.Value.Name == "City").Select(t => t.Key).ToList().ForEach(side => card.SetField(side, city.Id));
                AddCityToLand(card, landCounts, city.Id);
                return base.Handle(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed);
            }

            id++;
            var newCity = new City(id);
            foreach (var around in citiesAround)
            {
                var city = Cities.First(c => c.Id == around.Value);
                switch (around.Key)
                {
                    case Side.TOP:
                        cityPart.TopIsOpen = false;
                        city.SetSides(topCard.Id, Side.BOTTOM);
                        break;
                    case Side.MIDDLELEFT:
                        cityPart.LeftIsOpen = false;
                        city.SetSides(leftCard.Id, Side.MIDDLERIGHT);
                        break;
                    case Side.BOTTOM:
                        cityPart.BottomIsOpen = false;
                        city.SetSides(botCard.Id, Side.TOP);
                        break;
                    case Side.MIDDLERIGHT:
                        cityPart.RightIsOpen = false;
                        city.SetSides(rightCard.Id, Side.MIDDLELEFT);
                        break;
                }

                foreach (var cp in city.CityParts)
                {
                    newCity.ExpandCity(cp);
                    var cityCard = _board.CardCoordinates.Values.First(c => c.Id == cp.CardId);
                    foreach (var side in cityCard.Tile.Sides)
                    {
                        if (side.Value.PlaceId == city.Id)
                            cityCard.SetField(side.Key, newCity.Id);
                        if (side.Value.Name == "Land")
                        {
                            var land = _landHandler.Lands.First(l => l.Id == side.Value.PlaceId);
                            if (land.SurroundingCities.Remove(city.Id))
                                land.SurroundingCities.Add(newCity.Id);
                        }
                    }
                }
                Cities.Remove(city);
            }

            newCity.ExpandCity(cityPart);
            Cities.Add(newCity);

            foreach (var side in card.Tile.Sides)
            {
                if (side.Value.Name == "City")
                    card.SetField(side.Key, newCity.Id);
            }
            
            AddCityToLand(card, landCounts, newCity.Id);
            return base.Handle(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed);
        }

        private int PlaceTwoCity(Card topCard, Card botCard, Card leftCard, Card rightCard, Card card, int id)
        {
            if (card.Top.Name == "City")
            {
                if (topCard != null)
                {
                    var city = Cities.FirstOrDefault(c => c.Id == topCard.Bottom.PlaceId);
                    card.SetField(Side.TOP, city.Id);
                    if (city.GetCityPartByCardId(card.Id) == null)
                        city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false, HasCrest = card.HasCrest });
                    city.SetSides(topCard.Id, Side.BOTTOM);
                    _landHandler.Lands.First(l => l.Id == card.Tile.Field5.PlaceId).SurroundingCities.Add(city.Id);
                }
                else
                {
                    id++;
                    var city = new City(id);
                    city.ExpandCity(new CityPart(card.Id) { TopIsOpen = true, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false, HasCrest = card.HasCrest });
                    Cities.Add(city);
                    card.SetField(Side.TOP, id);
                    _landHandler.Lands.First(l => l.Id == card.Tile.Field5.PlaceId).SurroundingCities.Add(city.Id);
                }

            }

            if (card.Left.Name == "City")
            {

                if (leftCard != null)
                {
                    var city = Cities.FirstOrDefault(c => c.Id == leftCard.Right.PlaceId);
                    card.SetField(Side.MIDDLELEFT, city.Id);
                    if (city.GetCityPartByCardId(card.Id) == null)
                        city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false, HasCrest = card.HasCrest });
                    city.SetSides(leftCard.Id, Side.MIDDLERIGHT);
                    _landHandler.Lands.First(l => l.Id == card.Tile.Field5.PlaceId).SurroundingCities.Add(city.Id);

                }
                else
                {
                    id++;
                    var city = new City(id);
                    city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = true, BottomIsOpen = false, RightIsOpen = false, HasCrest = card.HasCrest });
                    Cities.Add(city);
                    card.SetField(Side.MIDDLELEFT, id);
                    _landHandler.Lands.First(l => l.Id == card.Tile.Field5.PlaceId).SurroundingCities.Add(city.Id);
                }
            }

            if (card.Bottom.Name == "City")
            {
                if (botCard != null)
                {
                    var city = Cities.FirstOrDefault(c => c.Id == botCard.Top.PlaceId);
                    card.SetField(Side.BOTTOM, city.Id);
                    if (city.GetCityPartByCardId(card.Id) == null)
                        city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false, HasCrest = card.HasCrest });
                    city.SetSides(botCard.Id, Side.TOP);
                    _landHandler.Lands.First(l => l.Id == card.Tile.Field5.PlaceId).SurroundingCities.Add(city.Id);

                }
                else
                {
                    id++;
                    var city = new City(id);
                    city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = true, RightIsOpen = false, HasCrest = card.HasCrest });
                    Cities.Add(city);
                    card.SetField(Side.BOTTOM, id);
                    _landHandler.Lands.First(l => l.Id == card.Tile.Field5.PlaceId).SurroundingCities.Add(city.Id);

                }
            }

            if (card.Right.Name == "City")
            {

                if (rightCard != null)
                {
                    var city = Cities.FirstOrDefault(c => c.Id == rightCard.Left.PlaceId);
                    card.SetField(Side.MIDDLERIGHT, city.Id);
                    if (city.GetCityPartByCardId(card.Id) == null)
                        city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false, HasCrest = card.HasCrest });
                    city.SetSides(rightCard.Id, Side.MIDDLELEFT);
                    _landHandler.Lands.First(l => l.Id == card.Tile.Field5.PlaceId).SurroundingCities.Add(city.Id);

                }
                else
                {
                    id++;
                    var city = new City(id);
                    city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = true, HasCrest = card.HasCrest });
                    Cities.Add(city);
                    card.SetField(Side.MIDDLERIGHT, id);
                    _landHandler.Lands.First(l => l.Id == card.Tile.Field5.PlaceId).SurroundingCities.Add(city.Id);
                }
            }

            return id;
        }


        private void AddCityToLand(Card card, int landCounts, int cityId)
        {
            if (landCounts == 0)
                return;
            if (landCounts == 1)
            {
                _landHandler.Lands.First(l => l.Id == card.Tile.Sides.First(t => t.Value.Name == "Land").Value.PlaceId).SurroundingCities.Add(cityId);
                return;
            }
            if (card.Sides.Count(s => s.Name == "Road") < 2)
            {
                var lands = new HashSet<int>();
                foreach (var side in card.Tile.Sides.Where(s => s.Value.Name == "Land"))
                {
                    if (!lands.Contains(side.Value.PlaceId))
                    {
                        lands.Add(side.Value.PlaceId);
                        _landHandler.Lands.First(l => l.Id == side.Value.PlaceId).SurroundingCities.Add(cityId);
                    }
                }
                return;
            }
            var closedLands = GetSidesClosedByRoads(card);
            if (closedLands.Count != 0)
            {
                _landHandler.Lands.First(l => l.Id == card.Tile.Sides.First(s => s.Value.Name == "Land" && !closedLands.Contains(s.Key)).Value.PlaceId).SurroundingCities.Add(cityId);
                return;
            }
            if (card.Top.Name == "City" || card.Left.Name == "City")
                _landHandler.Lands.First(l => l.Id == card.Tile.Field1.PlaceId).SurroundingCities.Add(cityId);
            else
                _landHandler.Lands.First(l => l.Id == card.Tile.Field9.PlaceId).SurroundingCities.Add(cityId);
        }

        private HashSet<Side> GetSidesClosedByRoads(Card card)
        {
            var sides = new HashSet<Side>();
            if (card.Top.Name == "Road" && card.Left.Name == "Road")
                sides.Add(Side.TOPLEFT);

            if (card.Left.Name == "Road" && card.Bottom.Name == "Road")
                sides.Add(Side.BOTTOMLEFT);

            if (card.Bottom.Name == "Road" && card.Right.Name == "Road")
                sides.Add(Side.BOTTOMRIGHT);

            if (card.Right.Name == "Road" && card.Top.Name == "Road")
                sides.Add(Side.TOPRIGHT);

            return sides;
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
    }
}
