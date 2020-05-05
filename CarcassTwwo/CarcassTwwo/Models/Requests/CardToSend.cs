using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models.Requests
{
    public class CardToSend
    {
        public CardToSend (int tileId)
        {
            TileId = tileId;
            CoordinatesWithRotations = new List<CoordinatesWithRotation>();
        }
        public int TileId { get; private set; }
        public List<CoordinatesWithRotation> CoordinatesWithRotations { get; set; }
    }
}
