using CarcassTwwo.Models.Places;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models.Requests
{
    public class RoadHandler : AbstractHandler
    {
        public HashSet<Road> Roads { get; set; }

        private ILandHandler _landHandler;
        private IBoard _board;

        public RoadHandler(ILandHandler landHandler, IBoard board)
        {
            Roads = new HashSet<Road>();
            _board = board;
            _landHandler = landHandler;
        }
    }
}
