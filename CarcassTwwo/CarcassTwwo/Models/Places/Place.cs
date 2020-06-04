using System.Collections.Generic;

namespace CarcassTwwo.Models.Places
{
    public abstract class Place
    {
        public int Id { get; private set; }
        public List<Meeple> Meeples { get; private set; }
        public bool CanPlaceMeeple { get; set; }

        public Place(int id)
        {
            Id = id;
            Meeples = new List<Meeple>();
        }
        public abstract void PlaceMeeple(Client owner, int field, Card card);
        public void RemoveMeeples()
        {
            Meeples.Clear();
        }
    }
}
