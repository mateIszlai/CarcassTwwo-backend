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

        public int Amount { get; set; }
        public int Remaining { get; set; }
    }
}
