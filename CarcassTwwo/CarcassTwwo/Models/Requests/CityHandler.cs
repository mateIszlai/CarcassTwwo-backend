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
