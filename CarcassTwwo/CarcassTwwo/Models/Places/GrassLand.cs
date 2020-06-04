using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models.Places
{
    public class GrassLand : Place
    {
        //Grasslands are counted only at the end of the game.
        public List<int> SurroundingCities { get; set; }
        public int Size { get { return _cardIds.Count; }}

        public List<int> _cardIds { get; private set; }
        //only the finished cities count
        public List<Meeple> Peasants { get; set; }
        public GrassLand(int id) : base(id)
        {
            SurroundingCities = new List<int>();
            Peasants = new List<Meeple>();
            _cardIds = new List<int>();
        }

        public void ExpandLand (int cardId)
        {
            _cardIds.Add(cardId);
        }

        public void AddNewCity(int id)
        {
            SurroundingCities.Add(id);
        }

        public override void PlaceMeeple(Client owner, int field, Card card)
        {
            if (!card.HasMeeple)
            {
                var peasant = new Meeple(MeepleType.PEASANT, owner, field, card.Id, Id);
                Meeples.Add(peasant);
                card.AddMeeple(peasant, field);
                CanPlaceMeeple = false;
            }
        }
    }
}
