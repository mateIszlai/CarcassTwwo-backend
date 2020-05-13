using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models.Places
{
    public class Road
    {
        public List<Meeple> HighwayMen { get; set; }
        public List<Coordinate> RoadTiles { get; set; }
        public bool IsFinished { get; set; }
        public Road()
        {
            HighwayMen = new List<Meeple>();
            RoadTiles = new List<Coordinate>();
        }

        public void ExpandRoad(Coordinate coordinate)
        {
            RoadTiles.Add(coordinate);
        }

        public void PlaceHighwayMan(Coordinate field)
        {
            HighwayMen.Add(new Meeple(field, "Highwayman"));
        }

        public void CheckStateOfRoad()
        {
            //TODO if road is finished then IsFinished = true;
        }
    }
}
