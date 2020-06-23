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

        private IBoard _board;


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

        private GrassLand MergeLands(HashSet<int> around, int cardId, int id)
        {
            id++;
            var newLand = new GrassLand(id);
            newLand.ExpandLand(cardId);
            foreach (var landId in around)
            {
                var land = Lands.First(l => l.Id == landId);
                newLand.Meeples.AddRange(land.Meeples);
                land.Roads.ToList().ForEach(r => newLand.Roads.Add(r));
                land.SurroundingCities.ToList().ForEach(s => newLand.SurroundingCities.Add(s));
                foreach (var landCardId in land.CardIds)
                {
                    newLand.ExpandLand(landCardId);
                    var landCard = _board.CardCoordinates.Values.First(c => c.Id == landCardId);
                    foreach (var side in landCard.Tile.Sides.Where(s => s.Value.Name == "Land"))
                    {
                        if (side.Value.PlaceId == land.Id)
                            landCard.SetField(side.Key, newLand.Id);
                    }
                }
                Lands.Remove(land);
            }
            Lands.Add(newLand);
            return newLand;
        }


    }
}
