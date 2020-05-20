using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models.Places
{
    public class GrassLand
    {
        //Grasslands are counted only at the end of the game.
        public int SurroundingCities { get; set; }
        //only the finished cities count
        public List<Meeple> Peasants { get; set; }
        public GrassLand()
        {
            SurroundingCities = 0;
            Peasants = new List<Meeple>();
        }

        public void AddNewCity()
        {
            SurroundingCities++;
        }

    }
}
