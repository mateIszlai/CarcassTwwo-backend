using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models.Requests
{
    public class CardToRecieve
    {
        public int CardId { get; set; }
        public string Rotation { get; set; }
        public Coordinate Coordinate { get; set; }
    }
}
