using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models.Places
{
    public class GrassLand : Place
    {
        public HashSet<int> SurroundingCities { get; private set; }
        public HashSet<int> Roads { get; private set; }
        public int Size { get { return CardIds.Count; }}

        public HashSet<int> CardIds { get; private set; }
        public GrassLand(int id, int cardId) : base(id)
        {
            Roads = new HashSet<int>();
            SurroundingCities = new HashSet<int>();
            CardIds = new HashSet<int> { cardId };
        }

        public GrassLand(int id) : base(id)
        {
            Roads = new HashSet<int>();
            SurroundingCities = new HashSet<int>();
            CardIds = new HashSet<int>();
        }

        public void ExpandLand (int cardId)
        {
            CardIds.Add(cardId);
        }

        public void AddNewCity(int id)
        {
            SurroundingCities.Add(id);
        }

        public override void PlaceMeeple(Client owner, int field, Card card)
        {
            if (!card.HasMeeple)
            {
                var peasant = new Meeple(MeepleType.PEASANT, owner, field, card, Id);
                Meeples.Add(peasant);
                owner.Meeples.Add(peasant);
                card.AddMeeple(peasant, field);
                owner.MeepleCount--;
            }
        }
    }
}
