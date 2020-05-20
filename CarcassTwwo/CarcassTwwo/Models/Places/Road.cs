using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models.Places
{
    public class Road : Place
    {
        private HashSet<RoadPart> _roadParts;
        public bool IsOpen { get {return _roadParts.Any(r => r.LeftOpen || r.RightOpen); } }
        public Road(int id) : base(id)
        {
            _roadParts = new HashSet<RoadPart>();
        }

        public void ExpandRoad(RoadPart roadPart)
        {
            _roadParts.Add(roadPart);
        }

        public void SetSides(List<RoadPart> roadParts)
        {
            foreach (var roadPart in roadParts)
            {
                var part = _roadParts.FirstOrDefault(r => r.CardIds == roadPart.CardIds);
                if(part != null)
                {
                    part.LeftOpen = roadPart.LeftOpen;
                    part.RightOpen = roadPart.RightOpen;
                }
            }
        }

        public override void PlaceMeeple(Client owner)
        {
            Meeples.Add(new Meeple(MeepleType.HIGHWAYMAN, owner));
        }
    }
}
