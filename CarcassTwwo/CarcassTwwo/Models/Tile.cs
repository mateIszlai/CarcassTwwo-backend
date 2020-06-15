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
        public List<LandType> Fields { get { return new List<LandType> { Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9 }; } }

        public LandType Field1 { get; set; }
        public LandType Field2 { get; set; }
        public LandType Field3 { get; set; }
        public LandType Field4 { get; set; }
        public LandType Field5 { get; set; }
        public LandType Field6 { get; set; }
        public LandType Field7 { get; set; }
        public LandType Field8 { get; set; }
        public LandType Field9 { get; set; }

        public int[] FieldsPlaceIds
        {
            get
            {
                return new int[] { Field1.PlaceId, Field2.PlaceId, Field3.PlaceId, Field4.PlaceId, Field5.PlaceId, Field6.PlaceId, Field7.PlaceId, Field8.PlaceId, Field9.PlaceId };
            }
        }

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
