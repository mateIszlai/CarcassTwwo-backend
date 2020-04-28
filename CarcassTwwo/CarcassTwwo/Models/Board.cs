using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models
{
    public class Board
    {
        public Dictionary<Coordinate, Card> CardCoordinates { get; set; }
        public List<Coordinate> AvailableCoordinates { get; set; }
        public void AddAvailableCoordinate(Coordinate coordinate)
        {
            AvailableCoordinates.Add(coordinate);
        }

        public void RemoveFromCoordinates(Coordinate coordinate)
        {
            AvailableCoordinates.Remove(coordinate);
        }
    }


    public struct Coordinate
    {
        public int x { get; set; }
        public int y { get; set; }
    }
}
