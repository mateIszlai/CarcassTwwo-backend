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
            CanPlaceMeeple = true;
        }
        public abstract void PlaceMeeple(Client owner, int field, Card card);
        public List<Meeple> RemoveMeeples()
        {
            List<Meeple> removeableMeeples = new List <Meeple>();

            foreach(var meeple in Meeples)
            {
                removeableMeeples.Add(meeple);
                meeple.Card.RemoveMeeple();
                meeple.Owner.MeepleCount++;
                meeple.Owner.Meeples.Remove(meeple);
            }
            Meeples.Clear();
            return removeableMeeples;
        }
    }
}
