using System.Collections.Generic;

namespace CarcassTwwo.Models.Places
{
    public class RoadPart
    {
        public List<int> CardIds { get; set; }
        public bool LeftOpen { get; set; }
        public bool RightOpen { get; set; }
    }
}
