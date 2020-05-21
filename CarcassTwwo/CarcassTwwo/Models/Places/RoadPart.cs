using System.Collections.Generic;

namespace CarcassTwwo.Models.Places
{
    public class RoadPart
    {
        public int CardId { get; private set; }
        public bool LeftOpen { get; set; }
        public bool RightOpen { get; set; }
        public List<int> Lands { get; set; }

        public RoadPart(int cardId)
        {
            CardId = cardId;
            LeftOpen = RightOpen = true;
            Lands = new List<int>();
        }
    }
}
