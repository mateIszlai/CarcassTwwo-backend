using System.Collections.Generic;

namespace CarcassTwwo.Models
{
    public interface IBoard
    {
         Dictionary<Coordinate, Card> CardCoordinates { get; set; }
    }
}
