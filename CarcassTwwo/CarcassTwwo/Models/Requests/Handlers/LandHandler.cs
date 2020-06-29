using CarcassTwwo.Models.Places;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarcassTwwo.Models.Requests.Handlers
{
    public class LandHandler : AbstractHandler, IRoadAdder, ICityAdder
    {
        private ILandScoreCounter _landScoreCounter;
        private ICityCounter _cityCounter;
        private HashSet<GrassLand> _lands;

        public HashSet<GrassLand> Lands
        {
            get { return new HashSet<GrassLand>(_lands); }
            private set { _lands = value; }
        }

        private readonly Side[] LEFT = new Side[] { Side.BOTTOMLEFT, Side.MIDDLELEFT, Side.TOPLEFT };
        private readonly Side[] RIGHT = new Side[] { Side.BOTTOMRIGHT, Side.MIDDLERIGHT, Side.TOPRIGHT };
        private readonly Side[] TOP = new Side[] { Side.TOPLEFT, Side.TOPRIGHT, Side.TOP };
        private readonly Side[] BOTTOM = new Side[] { Side.BOTTOMRIGHT, Side.BOTTOM, Side.BOTTOMLEFT };

        public LandHandler(IBoard board, ILandScoreCounter landScoreCounter) : base(board)
        {
            Lands = new HashSet<GrassLand>();
            _landScoreCounter = landScoreCounter;
        }

        public void SetCityCounter(ICityCounter cityCounter)
        {
            _cityCounter = cityCounter;
        }

        public override void HandleEndScore()
        {
            foreach (var land in _lands)
            {
                int finishedCities = _cityCounter.GetFinishedCitiesOfLand(land.SurroundingCities.ToArray());
                _landScoreCounter.CheckOwnerOfLand(land, finishedCities);
            }
            base.HandleEndScore();
        }

        public override void HandleMeeplePlacement(int placeOfMeeple, Card placedCard, Client owner)
        {
            var meeplePlace = placedCard.Tile.Fields[placeOfMeeple - 1];

            if (meeplePlace.Name == "Land")
            {
                var land = _lands.First(m => m.Id == meeplePlace.PlaceId);

                if (land.CanPlaceMeeple)
                    land.PlaceMeeple(owner, placeOfMeeple, placedCard);
                return;
            }

            base.HandleMeeplePlacement(placeOfMeeple, placedCard, owner);
        }

        public override object HandlePlacement(Card topCard, Card botCard, Card leftCard, Card rightCard, Card card, int landCounts, bool roadClosed, Coordinate[] surroundingCoords)
        {
            var landsAround = new Dictionary<Side, HashSet<int>>();

            if (landCounts == 0)
                return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, roadClosed, surroundingCoords);

            if (topCard != null)
            {
                switch (topCard.Bottom.Name)
                {
                    case "Land":
                        landsAround.Add(Side.TOP, new HashSet<int> { topCard.Bottom.PlaceId });
                        if (landsAround.ContainsKey(Side.TOPLEFT))
                            landsAround[Side.TOPLEFT].Add(topCard.Tile.Field7.PlaceId);
                        else
                            landsAround.Add(Side.TOPLEFT, new HashSet<int> { topCard.Tile.Field7.PlaceId });

                        if (landsAround.ContainsKey(Side.TOPRIGHT))
                            landsAround[Side.TOPRIGHT].Add(topCard.Tile.Field9.PlaceId);
                        else
                            landsAround.Add(Side.TOPRIGHT, new HashSet<int> { topCard.Tile.Field9.PlaceId });
                        break;

                    case "Road":
                        if (landsAround.ContainsKey(Side.TOPLEFT))
                            landsAround[Side.TOPLEFT].Add(topCard.Tile.Field7.PlaceId);
                        else
                            landsAround.Add(Side.TOPLEFT, new HashSet<int> { topCard.Tile.Field7.PlaceId });

                        if (landsAround.ContainsKey(Side.TOPRIGHT))
                            landsAround[Side.TOPRIGHT].Add(topCard.Tile.Field9.PlaceId);
                        else
                            landsAround.Add(Side.TOPRIGHT, new HashSet<int> { topCard.Tile.Field9.PlaceId });
                        break;
                }
            }

            if (leftCard != null)
            {
                switch (leftCard.Right.Name)
                {
                    case "Land":
                        landsAround.Add(Side.MIDDLELEFT, new HashSet<int> { leftCard.Right.PlaceId });
                        if (landsAround.ContainsKey(Side.TOPLEFT))
                            landsAround[Side.TOPLEFT].Add(leftCard.Tile.Field3.PlaceId);
                        else
                            landsAround.Add(Side.TOPLEFT, new HashSet<int> { leftCard.Tile.Field3.PlaceId });

                        if (landsAround.ContainsKey(Side.BOTTOMLEFT))
                            landsAround[Side.BOTTOMLEFT].Add(leftCard.Tile.Field9.PlaceId);
                        else
                            landsAround.Add(Side.BOTTOMLEFT, new HashSet<int> { leftCard.Tile.Field9.PlaceId });
                        break;

                    case "Road":
                        if (landsAround.ContainsKey(Side.TOPLEFT))
                            landsAround[Side.TOPLEFT].Add(leftCard.Tile.Field3.PlaceId);
                        else
                            landsAround.Add(Side.TOPLEFT, new HashSet<int> { leftCard.Tile.Field3.PlaceId });

                        if (landsAround.ContainsKey(Side.BOTTOMLEFT))
                            landsAround[Side.BOTTOMLEFT].Add(leftCard.Tile.Field9.PlaceId);
                        else
                            landsAround.Add(Side.BOTTOMLEFT, new HashSet<int> { leftCard.Tile.Field9.PlaceId });
                        break;
                }
            }

            if (botCard != null)
            {
                switch (botCard.Top.Name)
                {
                    case "Land":
                        landsAround.Add(Side.BOTTOM, new HashSet<int> { botCard.Top.PlaceId });
                        if (landsAround.ContainsKey(Side.BOTTOMLEFT))
                            landsAround[Side.BOTTOMLEFT].Add(botCard.Tile.Field1.PlaceId);
                        else
                            landsAround.Add(Side.BOTTOMLEFT, new HashSet<int> { botCard.Tile.Field1.PlaceId });

                        if (landsAround.ContainsKey(Side.BOTTOMRIGHT))
                            landsAround[Side.BOTTOMRIGHT].Add(botCard.Tile.Field3.PlaceId);
                        else
                            landsAround.Add(Side.BOTTOMRIGHT, new HashSet<int> { botCard.Tile.Field3.PlaceId });
                        break;

                    case "Road":
                        if (landsAround.ContainsKey(Side.BOTTOMLEFT))
                            landsAround[Side.BOTTOMLEFT].Add(botCard.Tile.Field1.PlaceId);
                        else
                            landsAround.Add(Side.BOTTOMLEFT, new HashSet<int> { botCard.Tile.Field1.PlaceId });

                        if (landsAround.ContainsKey(Side.BOTTOMRIGHT))
                            landsAround[Side.BOTTOMRIGHT].Add(botCard.Tile.Field3.PlaceId);
                        else
                            landsAround.Add(Side.BOTTOMRIGHT, new HashSet<int> { botCard.Tile.Field3.PlaceId });
                        break;
                }
            }

            if (rightCard != null)
            {
                switch (rightCard.Left.Name)
                {
                    case "Land":
                        landsAround.Add(Side.MIDDLERIGHT, new HashSet<int> { rightCard.Left.PlaceId });
                        if (landsAround.ContainsKey(Side.TOPRIGHT))
                            landsAround[Side.TOPRIGHT].Add(rightCard.Tile.Field1.PlaceId);
                        else
                            landsAround.Add(Side.TOPRIGHT, new HashSet<int> { rightCard.Tile.Field1.PlaceId });

                        if (landsAround.ContainsKey(Side.BOTTOMRIGHT))
                            landsAround[Side.BOTTOMRIGHT].Add(rightCard.Tile.Field7.PlaceId);
                        else
                            landsAround.Add(Side.BOTTOMRIGHT, new HashSet<int> { rightCard.Tile.Field7.PlaceId });
                        break;

                    case "Road":
                        if (landsAround.ContainsKey(Side.TOPRIGHT))
                            landsAround[Side.TOPRIGHT].Add(rightCard.Tile.Field1.PlaceId);
                        else
                            landsAround.Add(Side.TOPRIGHT, new HashSet<int> { rightCard.Tile.Field1.PlaceId });

                        if (landsAround.ContainsKey(Side.BOTTOMRIGHT))
                            landsAround[Side.BOTTOMRIGHT].Add(rightCard.Tile.Field7.PlaceId);
                        else
                            landsAround.Add(Side.BOTTOMRIGHT, new HashSet<int> { rightCard.Tile.Field7.PlaceId });
                        break;
                }
            }

            if (landsAround.Count == 0)
            {
                if (landCounts == 1)
                {
                    id++;
                    _lands.Add(new GrassLand(id, card.Id));
                    foreach (var field in card.Tile.Sides)
                    {
                        if (field.Value.Name == "Land")
                            card.SetField(field.Key, id);
                    }
                    return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, roadClosed, surroundingCoords);
                }

                var closedSides = GetSidesClosedByRoads(card);

                if (closedSides.Count != 0)
                {
                    id++;
                    var newLand = new GrassLand(id, card.Id);
                    _lands.Add(newLand);
                    foreach (var side in card.Tile.Sides.Where(s => s.Value.Name == "Land").Select(t => t.Key))
                    {
                        if (closedSides.Contains(side))
                        {
                            id++;
                            _lands.Add(new GrassLand(id, card.Id));
                            card.SetField(side, id);
                        }
                        card.SetField(side, newLand.Id);
                    }
                    return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, roadClosed, surroundingCoords);
                }

                if (card.Sides.Count(s => s.Name == "City") == 3)
                {
                    foreach (var side in card.Tile.Sides.Where(s => s.Value.Name == "Land").Select(t => t.Key))
                    {
                        id++;
                        _lands.Add(new GrassLand(id, card.Id));
                        card.SetField(side, id);
                    }
                    return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, roadClosed, surroundingCoords);
                }

                for (int i = 0; i < 2; i++)
                {
                    id++;
                    _lands.Add(new GrassLand(id, card.Id));
                }

                if (card.Top.Name == "Road" && card.Bottom.Name == "Road" || card.Top.Name == "City" && card.Bottom.Name == "City")
                {
                    foreach (var side in card.Tile.Sides.Where(s => s.Value.Name == "Land").Select(t => t.Key))
                    {
                        if (LEFT.Contains(side))
                            card.SetField(side, id);
                        else
                            card.SetField(side, id - 1);
                    }
                    return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, roadClosed, surroundingCoords);
                }

                foreach (var side in card.Tile.Sides.Where(s => s.Value.Name == "Land").Select(t => t.Key))
                {
                    if (TOP.Contains(side))
                        card.SetField(side, id);
                    else
                        card.SetField(side, id - 1);
                }
                return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, roadClosed, surroundingCoords);
            }


            if (landsAround.Count == 3 && (landsAround.ContainsKey(Side.TOP) ||
                landsAround.ContainsKey(Side.MIDDLELEFT) ||
                landsAround.ContainsKey(Side.BOTTOM) ||
                landsAround.ContainsKey(Side.MIDDLERIGHT)))
            {
                var land = _lands.FirstOrDefault(l => l.Id == landsAround.First().Value.First());
                land.ExpandLand(card.Id);
                if (landCounts == 1)
                {
                    foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Land").Select(s => s.Key))
                    {
                        card.SetField(side, land.Id);
                    }
                    return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, roadClosed, surroundingCoords);
                }

                foreach (var l in landsAround)
                {
                    card.SetField(l.Key, l.Value.First());
                }

                if (landCounts == 2)
                {
                    id++;
                    _lands.Add(new GrassLand(id, card.Id));
                    if (card.Top.Name == "Road" && card.Bottom.Name == "Road"
                        || card.Left.Name == "Road" && card.Right.Name == "Road"
                        || card.Tile.Field5.Name == "City")
                    {
                        foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Land").Select(s => s.Key))
                        {
                            if (!landsAround.ContainsKey(side))
                                card.SetField(side, id);
                        }
                        return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, roadClosed, surroundingCoords);
                    }

                    var closedSide = GetSidesClosedByRoads(card).First();
                    card.SetField(closedSide, id);
                    foreach (var field in card.Tile.Sides)
                    {
                        if (field.Key != closedSide && field.Value.Name == "Land")
                            card.SetField(field.Key, land.Id);
                    }
                    return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, roadClosed, surroundingCoords);
                }

                foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Land").Select(s => s.Key))
                {
                    if (!landsAround.ContainsKey(side))
                    {
                        id++;
                        _lands.Add(new GrassLand(id, card.Id));
                        card.SetField(side, id);
                    }
                }
                return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, roadClosed, surroundingCoords);
            }

            if (landCounts == 1)
            {
                var surroundingLandIds = LandIdsAround(landsAround);
                var land = _lands.First(l => l.Id == surroundingLandIds.First());

                if (surroundingLandIds.Count == 1)
                    land.ExpandLand(card.Id);
                else
                {
                    land = MergeLands(surroundingLandIds, card.Id, id);
                    id = land.Id;
                }

                foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Land").Select(s => s.Key))
                {
                    card.SetField(side, land.Id);
                }

                return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, roadClosed, surroundingCoords);
            }

            var cornerClosedLands = GetSidesClosedByRoads(card);

            if (cornerClosedLands.Count == 0 && card.Tile.Field5.Name == "City")
            {
                foreach (var around in landsAround)
                {
                    var land = _lands.First(l => l.Id == around.Value.First());
                    land.ExpandLand(card.Id);
                    card.SetField(around.Key, land.Id);
                }
                return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, roadClosed, surroundingCoords);
            }

            var visitedSides = new HashSet<Side>();

            if (cornerClosedLands.Count != 0)
            {
                var notCorners = new HashSet<int>();
                foreach (var around in landsAround)
                {
                    if (cornerClosedLands.Contains(around.Key))
                    {
                        if (around.Value.Count == 1)
                        {
                            var gland = _lands.First(l => l.Id == around.Value.First());
                            visitedSides.Add(around.Key);
                            gland.ExpandLand(card.Id);
                            card.SetField(around.Key, gland.Id);
                            continue;
                        }
                        var landToAdd = MergeLands(around.Value, card.Id, id);
                        id = landToAdd.Id;
                        _lands.Add(landToAdd);
                        card.SetField(around.Key, landToAdd.Id);
                        visitedSides.Add(around.Key);
                        continue;
                    }
                    foreach (var landId in around.Value)
                        notCorners.Add(landId);
                }

                GrassLand land;

                if (notCorners.Count != 0)
                {
                    land = _lands.First(l => l.Id == notCorners.First());
                    if (notCorners.Count == 1)
                        land.ExpandLand(card.Id);
                    else if (notCorners.Count > 1)
                    {
                        land = MergeLands(notCorners, card.Id, id);
                        id = land.Id;
                    }
                }
                else
                {
                    id++;
                    land = new GrassLand(id, card.Id);
                    _lands.Add(land);
                }


                foreach (var side in card.Tile.Sides.Where(s => !visitedSides.Contains(s.Key) && s.Value.Name == "Land").Select(t => t.Key))
                {
                    if (cornerClosedLands.Contains(side))
                    {
                        id++;
                        var newLand = new GrassLand(id, card.Id);
                        card.SetField(side, newLand.Id);
                        _lands.Add(newLand);
                        continue;
                    }

                    card.SetField(side, land.Id);
                }

                return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, roadClosed, surroundingCoords);
            }


            var lefts = new HashSet<int>();
            var rights = new HashSet<int>();
            var bottoms = new HashSet<int>();
            var tops = new HashSet<int>();

            foreach (var around in landsAround)
            {
                foreach (var landId in around.Value)
                {
                    if (card.Top.Name == "Road" && card.Bottom.Name == "Road")
                    {
                        if (LEFT.Contains(around.Key))
                            lefts.Add(landId);
                        else
                            rights.Add(landId);
                        continue;
                    }
                    if (TOP.Contains(around.Key))
                        tops.Add(landId);
                    else
                        bottoms.Add(landId);
                }
            }

            if (lefts.Count != 0)
            {
                var land = _lands.First(l => l.Id == lefts.First());
                if (lefts.Count == 1)
                {
                    land.ExpandLand(card.Id);
                }
                else
                {
                    land = MergeLands(lefts, card.Id, id);
                    id = land.Id;
                }

                foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Land" && LEFT.Contains(f.Key)).Select(s => s.Key))
                {
                    card.SetField(side, land.Id);
                }

            }

            if (rights.Count != 0)
            {
                var land = _lands.First(l => l.Id == rights.First());
                if (rights.Count == 1)
                {
                    land.ExpandLand(card.Id);
                }
                else
                {
                    land = MergeLands(rights, card.Id, id);
                    id = land.Id;
                }

                foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Land" && RIGHT.Contains(f.Key)).Select(s => s.Key))
                {
                    card.SetField(side, land.Id);
                }
            }

            if (tops.Count != 0)
            {
                var land = _lands.First(l => l.Id == tops.First());
                if (tops.Count == 1)
                {
                    land.ExpandLand(card.Id);
                }
                else
                {
                    land = MergeLands(tops, card.Id, id);
                    id = land.Id;
                }

                foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Land" && TOP.Contains(f.Key)).Select(s => s.Key))
                {
                    card.SetField(side, land.Id);
                }
            }

            if (bottoms.Count != 0)
            {
                var land = _lands.First(l => l.Id == bottoms.First());
                if (bottoms.Count == 1)
                {
                    land.ExpandLand(card.Id);
                }
                else
                {
                    land = MergeLands(bottoms, card.Id, id);
                    id = land.Id;
                }

                foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Land" && BOTTOM.Contains(f.Key)).Select(s => s.Key))
                {
                    card.SetField(side, land.Id);
                }
            }
            return base.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, roadClosed, surroundingCoords);
        }

        private HashSet<int> LandIdsAround(Dictionary<Side, HashSet<int>> landsAround)
        {
            var lands = new HashSet<int>();
            foreach (var land in landsAround)
            {
                foreach (var id in land.Value)
                {
                    lands.Add(id);
                }
            }
            return lands;
        }

        private HashSet<Side> GetSidesClosedByRoads(Card card)
        {
            var sides = new HashSet<Side>();
            if (card.Top.Name == "Road" && card.Left.Name == "Road")
                sides.Add(Side.TOPLEFT);

            if (card.Left.Name == "Road" && card.Bottom.Name == "Road")
                sides.Add(Side.BOTTOMLEFT);

            if (card.Bottom.Name == "Road" && card.Right.Name == "Road")
                sides.Add(Side.BOTTOMRIGHT);

            if (card.Right.Name == "Road" && card.Top.Name == "Road")
                sides.Add(Side.TOPRIGHT);

            return sides;
        }

        private GrassLand MergeLands(HashSet<int> around, int cardId, int id)
        {
            id++;
            var newLand = new GrassLand(id);
            newLand.ExpandLand(cardId);
            foreach (var landId in around)
            {
                var land = _lands.First(l => l.Id == landId);
                newLand.Meeples.AddRange(land.Meeples);
                land.Roads.ToList().ForEach(r => newLand.Roads.Add(r));
                land.SurroundingCities.ToList().ForEach(s => newLand.SurroundingCities.Add(s));
                foreach (var landCardId in land.CardIds)
                {
                    newLand.ExpandLand(landCardId);
                    var landCard = _board.CardCoordinates.Values.First(c => c.Id == landCardId);
                    foreach (var side in landCard.Tile.Sides.Where(s => s.Value.Name == "Land"))
                    {
                        if (side.Value.PlaceId == land.Id)
                            landCard.SetField(side.Key, newLand.Id);
                    }
                }
                _lands.Remove(land);
            }
            _lands.Add(newLand);
            return newLand;
        }

        public void AddRoadToLand(Side side, int tempId, Card card)
        {
            switch (side)
            {
                case Side.TOP:
                    _lands.First(l => l.Id == card.Tile.Field1.PlaceId).Roads.Add(tempId);
                    _lands.First(l => l.Id == card.Tile.Field3.PlaceId).Roads.Add(tempId);
                    break;
                case Side.MIDDLELEFT:
                    _lands.First(l => l.Id == card.Tile.Field1.PlaceId).Roads.Add(tempId);
                    _lands.First(l => l.Id == card.Tile.Field7.PlaceId).Roads.Add(tempId);
                    break;
                case Side.BOTTOM:
                    _lands.First(l => l.Id == card.Tile.Field7.PlaceId).Roads.Add(tempId);
                    _lands.First(l => l.Id == card.Tile.Field9.PlaceId).Roads.Add(tempId);
                    break;
                case Side.MIDDLERIGHT:
                    _lands.First(l => l.Id == card.Tile.Field3.PlaceId).Roads.Add(tempId);
                    _lands.First(l => l.Id == card.Tile.Field9.PlaceId).Roads.Add(tempId);
                    break;
            }
        }

        public void ChangeRoadIdInLand(int landId, int roadIdToChange, int newRoadId)
        {
            var land = _lands.First(l => l.Id == landId);
            if (land.Roads.Remove(roadIdToChange))
                land.Roads.Add(newRoadId);
        }

        public void AddCityToLand(Card card, int landCounts, int cityId)
        {
            if (landCounts == 0)
                return;
            if (landCounts == 1)
            {
                _lands.First(l => l.Id == card.Tile.Sides.First(t => t.Value.Name == "Land").Value.PlaceId).SurroundingCities.Add(cityId);
                return;
            }
            if (card.Sides.Count(s => s.Name == "Road") < 2)
            {
                var lands = new HashSet<int>();
                foreach (var side in card.Tile.Sides.Where(s => s.Value.Name == "Land"))
                {
                    if (!lands.Contains(side.Value.PlaceId))
                    {
                        lands.Add(side.Value.PlaceId);
                        _lands.First(l => l.Id == side.Value.PlaceId).SurroundingCities.Add(cityId);
                    }
                }
                return;
            }
            var closedLands = GetSidesClosedByRoads(card);
            if (closedLands.Count != 0)
            {
                _lands.First(l => l.Id == card.Tile.Sides.First(s => s.Value.Name == "Land" && !closedLands.Contains(s.Key)).Value.PlaceId).SurroundingCities.Add(cityId);
                return;
            }
            if (card.Top.Name == "City" || card.Left.Name == "City")
                _lands.First(l => l.Id == card.Tile.Field1.PlaceId).SurroundingCities.Add(cityId);
            else
                _lands.First(l => l.Id == card.Tile.Field9.PlaceId).SurroundingCities.Add(cityId);
        }

        public void ChangeCityIdInLand(int landId, int cityIdToChange, int newCityId)
        {
            var land = _lands.First(l => l.Id == landId);
            if (land.Roads.Remove(cityIdToChange))
                land.Roads.Add(newCityId);
        }

        public void AddCityToLand(int landId, int cityId)
        {
            _lands.First(l => l.Id == landId).SurroundingCities.Add(cityId);
        }
    }
}