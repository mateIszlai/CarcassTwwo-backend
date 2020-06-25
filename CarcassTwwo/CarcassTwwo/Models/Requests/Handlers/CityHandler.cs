using CarcassTwwo.Models.Places;
using System.Collections.Generic;
using System.Linq;

namespace CarcassTwwo.Models.Requests.Handlers
{
    public class CityHandler : AbstractHandler, ICityCounter
    {
        private ICityScoreCounter _cityScoreCounter;
        private HashSet<City> _cities;

        public HashSet<City> Cities
        {
            get { return new HashSet<City>(_cities); }
            private set { _cities = value; }
        }


        private ICityAdder _cityAdder;

        public CityHandler(ICityAdder cityAdder, IBoard board, ICityScoreCounter cityScoreCounter): base(board)
        {
            Cities = new HashSet<City>();
            _cityAdder = cityAdder;
            _cityScoreCounter = cityScoreCounter;
        }

        public override List<Meeple> HandleScore(List<Meeple> meeples)
        {
            foreach (var city in _cities)
            {
                if (!city.IsOpen && !city.IsCounted)
                {
                    _cityScoreCounter.CheckOwnerOfCity(city);
                    meeples.AddRange(city.RemoveMeeples());
                    city.IsCounted = true;
                }
            }
            return base.HandleScore(meeples);
        }

        public override void HandleEndScore()
        {
            foreach (var city in _cities)
            {
                if (!city.CanPlaceMeeple && !city.IsCounted)
                {
                    _cityScoreCounter.CheckOwnerOfCity(city);
                }
            }
            base.HandleEndScore();
        }

        public override void HandleMeeplePlacement(int placeOfMeeple, Card placedCard, Client owner)
        {
            var meeplePlace = placedCard.Tile.Fields[placeOfMeeple - 1];

            if (meeplePlace.Name == "City")
            {
                var city = _cities.First(m => m.Id == meeplePlace.PlaceId);

                if (city.CanPlaceMeeple)
                    city.PlaceMeeple(owner, placeOfMeeple, placedCard);
            }

            base.HandleMeeplePlacement(placeOfMeeple, placedCard, owner);
        }

        public override int HandlePlacement(Card topCard, Card botCard, Card leftCard, Card rightCard, Card card, int landCounts, int id, bool roadClosed, Coordinate[] surroundingCoords)
        {
            int cityCounts = GetCityCount(card);

            if (cityCounts == 0)
                return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed, surroundingCoords);

            if (cityCounts == 2)
            {
                id =  PlaceTwoCity(topCard, botCard, leftCard, rightCard, card, id);
                return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed, surroundingCoords);
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
                _cities.Add(city);
                foreach (var side in card.Tile.Sides)
                {
                    if (side.Value.Name == "City")
                        card.SetField(side.Key, id);
                }
                _cityAdder.AddCityToLand(card, landCounts, city.Id);
                return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed, surroundingCoords);
            }

            if (citiesAround.Values.Distinct().Count() == 1)
            {
                var city = _cities.First(c => c.Id == citiesAround.First().Value);
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
                _cityAdder.AddCityToLand(card, landCounts, city.Id);
                return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed, surroundingCoords);
            }

            id++;
            var newCity = new City(id);
            foreach (var around in citiesAround)
            {
                var city = _cities.First(c => c.Id == around.Value);
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
                            _cityAdder.ChangeCityIdInLand(side.Value.PlaceId, city.Id, newCity.Id);
                        }
                    }
                }
                _cities.Remove(city);
            }

            newCity.ExpandCity(cityPart);
            _cities.Add(newCity);

            foreach (var side in card.Tile.Sides)
            {
                if (side.Value.Name == "City")
                    card.SetField(side.Key, newCity.Id);
            }
            
            _cityAdder.AddCityToLand(card, landCounts, newCity.Id);
            return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed, surroundingCoords);
        }

        private int PlaceTwoCity(Card topCard, Card botCard, Card leftCard, Card rightCard, Card card, int id)
        {
            if (card.Top.Name == "City")
            {
                if (topCard != null)
                {
                    var city = _cities.FirstOrDefault(c => c.Id == topCard.Bottom.PlaceId);
                    card.SetField(Side.TOP, city.Id);
                    if (city.GetCityPartByCardId(card.Id) == null)
                        city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false, HasCrest = card.HasCrest });
                    city.SetSides(topCard.Id, Side.BOTTOM);
                    _cityAdder.AddCityToLand(card.Tile.Field5.PlaceId, city.Id);
                }
                else
                {
                    id++;
                    var city = new City(id);
                    city.ExpandCity(new CityPart(card.Id) { TopIsOpen = true, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false, HasCrest = card.HasCrest });
                    _cities.Add(city);
                    card.SetField(Side.TOP, id);
                    _cityAdder.AddCityToLand(card.Tile.Field5.PlaceId, city.Id);
                }

            }

            if (card.Left.Name == "City")
            {

                if (leftCard != null)
                {
                    var city = _cities.FirstOrDefault(c => c.Id == leftCard.Right.PlaceId);
                    card.SetField(Side.MIDDLELEFT, city.Id);
                    if (city.GetCityPartByCardId(card.Id) == null)
                        city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false, HasCrest = card.HasCrest });
                    city.SetSides(leftCard.Id, Side.MIDDLERIGHT);
                    _cityAdder.AddCityToLand(card.Tile.Field5.PlaceId, city.Id);
                }
                else
                {
                    id++;
                    var city = new City(id);
                    city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = true, BottomIsOpen = false, RightIsOpen = false, HasCrest = card.HasCrest });
                    _cities.Add(city);
                    card.SetField(Side.MIDDLELEFT, id);
                    _cityAdder.AddCityToLand(card.Tile.Field5.PlaceId, city.Id);
                }
            }

            if (card.Bottom.Name == "City")
            {
                if (botCard != null)
                {
                    var city = _cities.FirstOrDefault(c => c.Id == botCard.Top.PlaceId);
                    card.SetField(Side.BOTTOM, city.Id);
                    if (city.GetCityPartByCardId(card.Id) == null)
                        city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false, HasCrest = card.HasCrest });
                    city.SetSides(botCard.Id, Side.TOP);
                    _cityAdder.AddCityToLand(card.Tile.Field5.PlaceId, city.Id);
                }
                else
                {
                    id++;
                    var city = new City(id);
                    city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = true, RightIsOpen = false, HasCrest = card.HasCrest });
                    _cities.Add(city);
                    card.SetField(Side.BOTTOM, id);
                    _cityAdder.AddCityToLand(card.Tile.Field5.PlaceId, city.Id);
                }
            }

            if (card.Right.Name == "City")
            {

                if (rightCard != null)
                {
                    var city = _cities.FirstOrDefault(c => c.Id == rightCard.Left.PlaceId);
                    card.SetField(Side.MIDDLERIGHT, city.Id);
                    if (city.GetCityPartByCardId(card.Id) == null)
                        city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false, HasCrest = card.HasCrest });
                    city.SetSides(rightCard.Id, Side.MIDDLELEFT);
                    _cityAdder.AddCityToLand(card.Tile.Field5.PlaceId, city.Id);
                }
                else
                {
                    id++;
                    var city = new City(id);
                    city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = true, HasCrest = card.HasCrest });
                    _cities.Add(city);
                    card.SetField(Side.MIDDLERIGHT, id);
                    _cityAdder.AddCityToLand(card.Tile.Field5.PlaceId, city.Id);
                }
            }

            return id;
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

        public int GetFinishedCitiesOfLand(int[] cityIds)
        {
            return cityIds.Aggregate(0, (prev, next) => {
                if (!_cities.First(c => c.Id == next).IsOpen)
                    prev++;
                return prev;
            });

            /*
            int cityCount = 0;
            foreach (var cityId in cityIds)
            {
                if (!_cities.First(c => c.Id == cityId).IsOpen)
                    cityCount++;
            }

            return cityCount;
            */
        }
    }
}