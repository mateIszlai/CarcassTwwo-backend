using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models.Places
{
    public class Monastery
    {
        public Meeple Monk { get; set; }
        public List<Coordinate> SurroundingTiles { get; set; }
        public bool IsFinished { get; set; }
        public Monastery()
        {
            SurroundingTiles = new List<Coordinate>();
        }

        public void PlaceMonk(Coordinate field, Client player)
        {
            Monk = new Meeple(field, "Monk",player);
        }

        public void ExpandMonastery(Coordinate coordinate)
        {
            SurroundingTiles.Add(coordinate);
        }

        public void CheckStateOfMonastery()
        {
            //TODO if monastery is surrounded then IsFinished = true;
        }
    }
}
