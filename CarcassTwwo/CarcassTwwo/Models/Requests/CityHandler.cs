using CarcassTwwo.Models.Places;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models.Requests
{
    public class CityHandler : AbstractHandler
    {
        public HashSet<City> Cities { get; private set; }

        private ILandHandler _landHandler;

        public CityHandler(ILandHandler landHandler)
        {
            Cities = new HashSet<City>();
            _landHandler = landHandler;
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
