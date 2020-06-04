using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models.Places
{
    public class City
    {
        public List<Meeple> Knights { get; set; }
        public List<Coordinate> CityTiles { get; set; }
        public bool IsFinished { get; set; }
        public City()
        {
            Knights = new List<Meeple>();
            CityTiles = new List<Coordinate>();
        }

        public void BuildCity(Coordinate coordinate)
        {
            CityTiles.Add(coordinate);
        }

        public void PlaceKnight(Coordinate field, Client owner)
        {
            Knights.Add(new Meeple(field, "Knight", owner));
        }

        public void CheckStateOfCity()
        {
            //TODO if city is finished then IsFinished = true;
        }
    }
}
