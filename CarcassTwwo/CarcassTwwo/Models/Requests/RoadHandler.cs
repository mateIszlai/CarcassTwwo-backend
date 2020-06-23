using CarcassTwwo.Models.Places;
using System.Collections.Generic;
using System.Linq;

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

        public override int Handle(Card topCard, Card botCard, Card leftCard, Card rightCard, Card card, int landCounts, int id, bool roadClosed)
        {
            var roadsCount = 0;
            var sideRoadsCount = card.Sides.Count(s => s.Name == "Road");
            if (sideRoadsCount != 0)
                roadsCount = roadClosed ? sideRoadsCount : 1;

            if (roadsCount == 0)
                return base.Handle(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed);

            var roadsAround = new Dictionary<Side, int>();

            var visitedSides = new HashSet<Side>();


            if (topCard != null && topCard.Bottom.Name == "Road")
                roadsAround.Add(Side.TOP, topCard.Bottom.PlaceId);
            if (leftCard != null && leftCard.Right.Name == "Road")
                roadsAround.Add(Side.MIDDLELEFT, leftCard.Right.PlaceId);
            if (botCard != null && botCard.Top.Name == "Road")
                roadsAround.Add(Side.BOTTOM, botCard.Top.PlaceId);
            if (rightCard != null && rightCard.Left.Name == "Road")
                roadsAround.Add(Side.MIDDLERIGHT, rightCard.Left.PlaceId);

            if (roadsAround.Count == 0)
            {
                for (int i = 0; i < roadsCount; i++)
                {
                    id++;
                    var road = new Road(id);
                    var roadPart = new RoadPart(card.Id);
                    if (roadClosed)
                        roadPart.LeftOpen = false;

                    road.ExpandRoad(roadPart);
                    Roads.Add(road);
                }

                var tempId = id;
                foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Road").Select(s => s.Key))
                {
                    card.SetField(side, tempId);
                    AddRoadToLand(side, tempId, card);
                    if (roadsCount != 1)
                    {
                        tempId--;
                    }
                }
                return base.Handle(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed);
            }

            if (roadsAround.Count == 1)
            {
                var around = roadsAround.First();
                var road = Roads.First(r => r.Id == around.Value);
                switch (around.Key)
                {
                    case Side.TOP:
                        road.SetSides(topCard.Id);
                        break;
                    case Side.MIDDLELEFT:
                        road.SetSides(leftCard.Id);
                        break;
                    case Side.BOTTOM:
                        road.SetSides(botCard.Id);
                        break;
                    case Side.MIDDLERIGHT:
                        road.SetSides(rightCard.Id);
                        break;
                }
                road.ExpandRoad(new RoadPart(card.Id) { LeftOpen = false, RightOpen = true });
                card.SetField(around.Key, road.Id);
                visitedSides.Add(around.Key);
                if (!roadClosed)
                {
                    card.Tile.Sides.Where(s => s.Value.Name == "Road").Select(t => t.Key).ToList().ForEach(r => card.SetField(r, road.Id));
                    AddRoadToLand(around.Key, road.Id, card);
                    return base.Handle(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed);
                }
                foreach (var side in card.Tile.Sides.Where(s => s.Value.Name == "Road" && !visitedSides.Contains(s.Key)).Select(t => t.Key))
                {
                    id++;
                    var roadToAdd = new Road(id);
                    roadToAdd.ExpandRoad(new RoadPart(card.Id) { LeftOpen = false, RightOpen = true });
                    card.SetField(side, roadToAdd.Id);
                    AddRoadToLand(side, roadToAdd.Id, card);
                    Roads.Add(roadToAdd);
                }
                return base.Handle(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed);
            }

            if (roadClosed || roadsAround.Values.Distinct().Count() == 1)
            {
                foreach (var around in roadsAround)
                {
                    var road = Roads.First(r => r.Id == around.Value);
                    visitedSides.Add(around.Key);
                    switch (around.Key)
                    {
                        case Side.TOP:
                            road.SetSides(topCard.Id);
                            break;
                        case Side.MIDDLELEFT:
                            road.SetSides(leftCard.Id);
                            break;
                        case Side.BOTTOM:
                            road.SetSides(botCard.Id);
                            break;
                        case Side.MIDDLERIGHT:
                            road.SetSides(rightCard.Id);
                            break;
                    }

                    if (road.RoadParts.Count(rp => rp.CardId == card.Id) == 0)
                        road.ExpandRoad(new RoadPart(card.Id) { LeftOpen = false, RightOpen = false });

                    card.SetField(around.Key, road.Id);
                    AddRoadToLand(around.Key, road.Id, card);
                }

                foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Road" && !visitedSides.Contains(f.Key)).Select(s => s.Key))
                {
                    id++;
                    var road = new Road(id);
                    var roadPart = new RoadPart(card.Id);
                    roadPart.LeftOpen = false;
                    road.ExpandRoad(roadPart);
                    Roads.Add(road);
                    card.SetField(side, id);
                    AddRoadToLand(side, id, card);
                }
                return base.Handle(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed);
            }

            id++;
            var newRoad = new Road(id);

            foreach (var around in roadsAround)
            {
                visitedSides.Add(around.Key);
                var road = Roads.First(r => r.Id == around.Value);
                switch (around.Key)
                {
                    case Side.TOP:
                        road.SetSides(topCard.Id);
                        break;
                    case Side.MIDDLELEFT:
                        road.SetSides(leftCard.Id);
                        break;
                    case Side.BOTTOM:
                        road.SetSides(botCard.Id);
                        break;
                    case Side.MIDDLERIGHT:
                        road.SetSides(rightCard.Id);
                        break;
                }


                card.SetField(around.Key, newRoad.Id);
                AddRoadToLand(around.Key, newRoad.Id, card);

                foreach (var rp in road.RoadParts)
                {
                    newRoad.ExpandRoad(rp);
                    var roadCard = _board.CardCoordinates.Values.FirstOrDefault(r => r.Id == rp.CardId);
                    foreach (var side in roadCard.Tile.Sides)
                    {
                        if (side.Value.PlaceId == road.Id)
                            roadCard.SetField(side.Key, newRoad.Id);
                        if (side.Value.Name == "Land")
                        {
                            var land = _landHandler.Lands.First(l => l.Id == side.Value.PlaceId);
                            if (land.Roads.Remove(road.Id))
                                land.Roads.Add(newRoad.Id);
                        }
                    }
                }
                Roads.Remove(road);
            }

            Roads.Add(newRoad);
            return base.Handle(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed);
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
