using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models.Places
{
    public class GrassLand : Place
    {

        //Grasslands are counted only at the end of the game.
        public List<int> SurroundingCities { get; private set; }

        public List<int> Roads { get; private set; }
        public int Size { get { return _cardIds.Count; }}

        private List<int> _cardIds;
        //only the finished cities count
        public List<Meeple> Peasants { get; set; }
        public GrassLand(int id, int cardId) : base(id)
        {
            Roads = new List<int>();
            SurroundingCities = new List<int>();
            Peasants = new List<Meeple>();
            _cardIds = new List<int> { cardId };
        }

        public GrassLand(int id) : base(id)
        {
            Roads = new List<int>();
            SurroundingCities = new List<int>();
            Peasants = new List<Meeple>();
            _cardIds = new List<int>();
        }

        public void ExpandLand (int cardId)
        {
            _cardIds.Add(cardId);
        }

        public override void PlaceMeeple(Client owner)
        {
            Meeples.Add(new Meeple(MeepleType.PEASANT, owner));
        }
    }
}
