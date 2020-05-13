﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models
{
    public class Meeple
    {
        public Coordinate Field { get; set; }
        public string Type { get; set; }
        public Meeple(Coordinate field, string type)
        {
            Field = field;
            Type = type;
        }
    }
}
