using CarcassTwwo.Models.Places;
using System.Collections.Generic;
using System.Linq;

namespace CarcassTwwo.Models.Requests.Handlers
{
    public class MonasteryHandle : AbstractHandler
    {
        private HashSet<Monastery> _monasteries;

        public HashSet<Monastery> Monasteries
        {
            get { return new HashSet<Monastery>(_monasteries); }
            private set { _monasteries = value; }
        }

        public MonasteryHandle(IBoard board) : base(board)
        {
            Monasteries = new HashSet<Monastery>();
        }

        public override int Handle(Card topCard, Card botCard, Card leftCard, Card rightCard, Card card, int landCounts, int id, bool roadClosed, Coordinate[] surroundingCoords)
        {
            SetNearMonasteries(card.Coordinate);
            if (surroundingCoords == null)
                return base.Handle(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed, surroundingCoords);
            
            id++;
            var monastery = new Monastery(id, card.Coordinate);
            card.MonasteryId = id;
            surroundingCoords.ToList().ForEach(coord => {
                if (_board.CardCoordinates.ContainsKey(coord))
                    monastery.SurroundingCoordinates.Remove(coord);
            });
            _monasteries.Add(monastery);
            card.SetField(Side.MIDDLE, id);
            return base.Handle(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed, surroundingCoords);
        }

        private void SetNearMonasteries(Coordinate coordinate)
        {
            var monasteries = _monasteries.Where(m => m.SurroundingCoordinates.Contains(coordinate)).ToList();
            monasteries.ForEach(m => m.SurroundingCoordinates.Remove(coordinate));
        }
    }
}
