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
