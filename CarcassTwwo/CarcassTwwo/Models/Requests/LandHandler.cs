using CarcassTwwo.Models.Places;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models.Requests
{
    public class LandHandler : AbstractHandler, ILandHandler
    {
        public HashSet<GrassLand> Lands { get; set; }

        private HashSet<int> LandIdsAround(Dictionary<Side, HashSet<int>> landsAround)
        {
            var lands = new HashSet<int>();
            foreach (var land in landsAround)
            {
                foreach (var id in land.Value)
                {
                    lands.Add(id);
                }
            }
            return lands;
        }

        private HashSet<Side> GetSidesClosedByRoads(Card card)
        {
            var sides = new HashSet<Side>();
            if (card.Top.Name == "Road" && card.Left.Name == "Road")
                sides.Add(Side.TOPLEFT);

            if (card.Left.Name == "Road" && card.Bottom.Name == "Road")
                sides.Add(Side.BOTTOMLEFT);

            if (card.Bottom.Name == "Road" && card.Right.Name == "Road")
                sides.Add(Side.BOTTOMRIGHT);

            if (card.Right.Name == "Road" && card.Top.Name == "Road")
                sides.Add(Side.TOPRIGHT);

            return sides;
        }

    }
}
