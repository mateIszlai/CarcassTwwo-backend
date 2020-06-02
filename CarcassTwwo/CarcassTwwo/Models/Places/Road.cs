using System.Collections.Generic;
using System.Linq;

namespace CarcassTwwo.Models.Places
{
    public class Road : Place
    {
        public HashSet<RoadPart> RoadParts { get; private set; }
        public bool IsOpen { get {return RoadParts.Any(r => r.LeftOpen || r.RightOpen); } }
        public Road(int id) : base(id)
        {
            RoadParts = new HashSet<RoadPart> ();
        }

        public void ExpandRoad(RoadPart roadPart)
        {
            RoadParts.Add(roadPart);
        }

        public void SetSides(int cardId)
        {
            var roadPart = RoadParts.First(r => r.CardId == cardId);
            if (roadPart.LeftOpen)
                roadPart.LeftOpen = false;
            else
                roadPart.RightOpen = false;
        }

        public override void PlaceMeeple(Client owner)
        {
            Meeples.Add(new Meeple(MeepleType.HIGHWAYMAN, owner));
        }
    }
}
