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

        private void SetNearMonasteries(Coordinate coordinate)
        {
            var monasteries = _monasteries.Where(m => m.SurroundingCoordinates.Contains(coordinate)).ToList();
            monasteries.ForEach(m => m.SurroundingCoordinates.Remove(coordinate));
        }
    }
}
