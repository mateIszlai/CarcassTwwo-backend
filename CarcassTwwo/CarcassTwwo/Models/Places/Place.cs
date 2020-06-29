using System.Collections.Generic;

namespace CarcassTwwo.Models.Places
{
    public abstract class Place
    {
        public int Id { get; private set; }
        public List<Meeple> Meeples { get; private set; }
        public bool CanPlaceMeeple
        { 
            get { return Meeples.Count == 0; }
        }
        public bool IsCounted { get; set; }

        public Place(int id)
        {
            Id = id;
            Meeples = new List<Meeple>();
            IsCounted = false;
        }
        public abstract void PlaceMeeple(Client owner, int field, Card card);
        public List<Meeple> RemoveMeeples()
        {
            List<Meeple> removeableMeeples = new List <Meeple>();

            Meeples.ForEach(meeple =>
            {
                removeableMeeples.Add(meeple);
                meeple.Owner.MeepleCount++;
            });

            Meeples.Clear();
            return removeableMeeples;
        }
    }
}
