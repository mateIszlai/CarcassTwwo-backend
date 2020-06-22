using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models
{
    public interface IBoard
    {
         Dictionary<Coordinate, Card> CardCoordinates { get; set; }
    }
}
