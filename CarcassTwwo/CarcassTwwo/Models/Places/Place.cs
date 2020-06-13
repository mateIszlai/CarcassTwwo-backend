using System.Collections.Generic;

namespace CarcassTwwo.Models.Places
{
    public abstract class Place
    {
        public int Id { get; private set; }
        public List<Meeple> Meeples { get; private set; }
        public bool CanPlaceMeeple { get; set; }
        public bool IsCounted { get; set; }

        public Place(int id)
        {
            Id = id;
            Meeples = new List<Meeple>();
            IsCounted = false;
        }
        public abstract void PlaceMeeple(Client owner, int field, Card card);
        public void RemoveMeeples()
        {
            foreach(var meeple in Meeples)
            {
                meeple.Card.RemoveMeeple();
                meeple.Owner.MeepleCount++;
                meeple.Owner.Meeples.Remove(meeple);
            }
            Meeples.Clear();
        }
    }
}
