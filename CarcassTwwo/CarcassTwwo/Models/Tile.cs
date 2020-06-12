using CarcassTwwo.Models.Places;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models
{
    public class Tile
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public LandType Field1 { get; set; }
        public LandType Field2 { get; set; }
        public LandType Field3 { get; set; }
        public LandType Field4 { get; set; }
        public LandType Field5 { get; set; }
        public LandType Field6 { get; set; }
        public LandType Field7 { get; set; }
        public LandType Field8 { get; set; }
        public LandType Field9 { get; set; }

        public LandType Top1 { get; set; }
        public LandType Top2 { get; set; }
        public LandType Top3 { get; set; }
        public LandType Left1 { get; set; }
        public LandType Left2 { get; set; }
        public LandType Left3 { get; set; }
        public LandType Bottom1 { get; set; }
        public LandType Bottom2 { get; set; }
        public LandType Bottom3 { get; set; }
        public LandType Right1 { get; set; }
        public LandType Right2 { get; set; }
        public LandType Right3 { get; set; }

        public Dictionary<Side, LandType> Sides { 
            get
            {
                return new Dictionary<Side, LandType>
                {
                    { Side.TOPLEFT, Field1 },
                    { Side.TOP, Field2 },
                    { Side.TOPRIGHT, Field3 },
                    { Side.MIDDLELEFT, Field4 },
                    { Side.MIDDLE, Field5 },
                    { Side.MIDDLERIGHT, Field6 },
                    { Side.BOTTOMLEFT, Field7 },
                    { Side.BOTTOM, Field8 },
                    { Side.BOTTOMRIGHT, Field9 }
                };
            }
        }

        public bool HasCrest { get; set; }

        public int Amount { get; set; }
        public int Remaining { get; set; }
        public Tile()
        { 
            /*Field1.Coordinate = new Coordinate { x = 0, y = 0 };
            Field2.Coordinate = new Coordinate { x = 1, y = 0 };
            Field3.Coordinate = new Coordinate { x = 2, y = 0 };
            Field4.Coordinate = new Coordinate { x = 0, y = 1 };
            Field5.Coordinate = new Coordinate { x = 1, y = 1 };
            Field6.Coordinate = new Coordinate { x = 2, y = 1 };
            Field7.Coordinate = new Coordinate { x = 0, y = 2 };
            Field8.Coordinate = new Coordinate { x = 1, y = 2 };
            Field9.Coordinate = new Coordinate { x = 2, y = 2 };*/
        }
    }
}
