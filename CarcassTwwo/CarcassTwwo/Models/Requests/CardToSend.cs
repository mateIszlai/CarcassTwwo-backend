using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models.Requests
{
    public class CardToSend
    {
        public CardToSend (int tileId, int cardId)
        {
            TileId = tileId;
            CardId = cardId;
            CoordinatesWithRotations = new List<CoordinatesWithRotation>();
            PossibleMeepleSlots = new List<int>();
        }

        public int CardId { get; set; }
        public int TileId { get; private set; }
        public List<CoordinatesWithRotation> CoordinatesWithRotations { get; set; }
        public List<int> PossibleMeepleSlots { get; set; }
    }
}
