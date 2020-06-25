using CarcassTwwo.Models.Places;
using System.Collections.Generic;
using System.Linq;

namespace CarcassTwwo.Models.Requests.Handlers
{
    public class RoadHandler : AbstractHandler
    {
        private IRoadScoreCounter _roadScoreCounter;
        private HashSet<Road> _roads;

        public HashSet<Road> Roads
        {
            get { return new HashSet<Road>(_roads); }
            private set { _roads = value; }
        }


        private IRoadAdder _roadAdder;

        public RoadHandler(IRoadAdder roadAdder, IBoard board, IRoadScoreCounter roadScoreCounter) : base(board)
        {
            Roads = new HashSet<Road>();
            _roadAdder = roadAdder;
            _roadScoreCounter = roadScoreCounter;
        }

        public override List<Meeple> HandleScore(List<Meeple> meeples)
        {
            foreach (var road in _roads)
            {
                if (!road.IsOpen && !road.IsCounted)
                {
                    _roadScoreCounter.CheckOwnerOfRoad(road);
                    meeples.AddRange(road.RemoveMeeples());
                    road.IsCounted = true;
                }
            }
            return base.HandleScore(meeples);
        }

        public override void HandleEndScore()
        {
            foreach (var road in _roads)
            {
                if (!road.IsCounted && !road.CanPlaceMeeple)
                {
                    _roadScoreCounter.CheckOwnerOfRoad(road);
                }
            }
            base.HandleEndScore();
        }

        public override void HandleMeeplePlacement(int placeOfMeeple, Card placedCard, Client owner)
        {
            var meeplePlace = placedCard.Tile.Fields[placeOfMeeple - 1];

            if (meeplePlace.Name == "Road")
            {
                var road = _roads.First(m => m.Id == meeplePlace.PlaceId);

                if (road.CanPlaceMeeple)
                    road.PlaceMeeple(owner, placeOfMeeple, placedCard);
            }

            base.HandleMeeplePlacement(placeOfMeeple, placedCard, owner);
        }

        public override int HandlePlacement(Card topCard, Card botCard, Card leftCard, Card rightCard, Card card, int landCounts, int id, bool roadClosed, Coordinate[] surroundingCoords)
        {
            var roadsCount = 0;
            var sideRoadsCount = card.Sides.Count(s => s.Name == "Road");
            if (sideRoadsCount != 0)
                roadsCount = roadClosed ? sideRoadsCount : 1;

            if (roadsCount == 0)
                return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed, surroundingCoords);

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
                    _roads.Add(road);
                }

                var tempId = id;
                foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Road").Select(s => s.Key))
                {
                    card.SetField(side, tempId);
                    _roadAdder.AddRoadToLand(side, tempId, card);
                    if (roadsCount != 1)
                    {
                        tempId--;
                    }
                }
                return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed, surroundingCoords);
            }

            if (roadsAround.Count == 1)
            {
                var around = roadsAround.First();
                var road = _roads.First(r => r.Id == around.Value);
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
                    _roadAdder.AddRoadToLand(around.Key, road.Id, card);
                    return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed, surroundingCoords);
                }
                foreach (var side in card.Tile.Sides.Where(s => s.Value.Name == "Road" && !visitedSides.Contains(s.Key)).Select(t => t.Key))
                {
                    id++;
                    var roadToAdd = new Road(id);
                    roadToAdd.ExpandRoad(new RoadPart(card.Id) { LeftOpen = false, RightOpen = true });
                    card.SetField(side, roadToAdd.Id);
                    _roadAdder.AddRoadToLand(side, roadToAdd.Id, card);
                    _roads.Add(roadToAdd);
                }
                return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed, surroundingCoords);
            }

            if (roadClosed || roadsAround.Values.Distinct().Count() == 1)
            {
                foreach (var around in roadsAround)
                {
                    var road = _roads.First(r => r.Id == around.Value);
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
                    _roadAdder.AddRoadToLand(around.Key, road.Id, card);
                }

                foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Road" && !visitedSides.Contains(f.Key)).Select(s => s.Key))
                {
                    id++;
                    var road = new Road(id);
                    var roadPart = new RoadPart(card.Id);
                    roadPart.LeftOpen = false;
                    road.ExpandRoad(roadPart);
                    _roads.Add(road);
                    card.SetField(side, id);
                    _roadAdder.AddRoadToLand(side, id, card);
                }
                return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed, surroundingCoords);
            }

            id++;
            var newRoad = new Road(id);

            foreach (var around in roadsAround)
            {
                visitedSides.Add(around.Key);
                var road = _roads.First(r => r.Id == around.Value);
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
                _roadAdder.AddRoadToLand(around.Key, newRoad.Id, card);

                foreach (var rp in road.RoadParts)
                {
                    newRoad.ExpandRoad(rp);
                    var roadCard = _board.CardCoordinates.Values.FirstOrDefault(r => r.Id == rp.CardId);
                    foreach (var side in roadCard.Tile.Sides)
                    {
                        if (side.Value.PlaceId == road.Id)
                            roadCard.SetField(side.Key, newRoad.Id);
                        if (side.Value.Name == "Land")
                            _roadAdder.ChangeRoadIdInLand(side.Value.PlaceId, road.Id, newRoad.Id);
                    }
                }
                _roads.Remove(road);
            }

            _roads.Add(newRoad);
            return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed, surroundingCoords);
        }
    }
}
