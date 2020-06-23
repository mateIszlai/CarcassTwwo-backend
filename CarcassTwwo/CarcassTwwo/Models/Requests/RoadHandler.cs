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

        private void AddRoadToLand(Side side, int tempId, Card card)
        {
            switch (side)
            {
                case Side.TOP:
                    _landHandler.Lands.First(l => l.Id == card.Tile.Field1.PlaceId).Roads.Add(tempId);
                    _landHandler.Lands.First(l => l.Id == card.Tile.Field3.PlaceId).Roads.Add(tempId);
                    break;
                case Side.MIDDLELEFT:
                    _landHandler.Lands.First(l => l.Id == card.Tile.Field1.PlaceId).Roads.Add(tempId);
                    _landHandler.Lands.First(l => l.Id == card.Tile.Field7.PlaceId).Roads.Add(tempId);
                    break;
                case Side.BOTTOM:
                    _landHandler.Lands.First(l => l.Id == card.Tile.Field7.PlaceId).Roads.Add(tempId);
                    _landHandler.Lands.First(l => l.Id == card.Tile.Field9.PlaceId).Roads.Add(tempId);
                    break;
                case Side.MIDDLERIGHT:
                    _landHandler.Lands.First(l => l.Id == card.Tile.Field3.PlaceId).Roads.Add(tempId);
                    _landHandler.Lands.First(l => l.Id == card.Tile.Field9.PlaceId).Roads.Add(tempId);
                    break;
            }
        }

    }
}
