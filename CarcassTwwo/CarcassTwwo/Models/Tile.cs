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
        public Field Field1 { get; set; }
        public Field Field2 { get; set; }
        public Field Field3 { get; set; }
        public Field Field4 { get; set; }
        public Field Field5 { get; set; }
        public Field Field6 { get; set; }
        public Field Field7 { get; set; }
        public Field Field8 { get; set; }
        public Field Field9 { get; set; }

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

        public Dictionary<int, Field> FieldTypes { get; }

        public bool HasCrest { get; set; }

        public int Amount { get; set; }
        public int Remaining { get; set; }
        public Tile()
        {
            FieldTypes = new Dictionary<int, Field>();
            FieldTypes.Add(1, Field1);
            FieldTypes.Add(2, Field2);
            FieldTypes.Add(3, Field3);
            FieldTypes.Add(4, Field4);
            FieldTypes.Add(5, Field5);
            FieldTypes.Add(6, Field6);
            FieldTypes.Add(7, Field7);
            FieldTypes.Add(8, Field8);
            FieldTypes.Add(9, Field9);

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
