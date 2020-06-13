using CarcassTwwo.Models.Places;
using CarcassTwwo.Models.Requests;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarcassTwwo.Models
{
    public class Board
    {
        public Dictionary<Coordinate, Card> CardCoordinates { get; set; }
        public Dictionary<RequiredCard, Coordinate> AvailableCoordinates { get; set; }

        private HashSet<City> _cities;
        private HashSet<GrassLand> _grassLands;
        private HashSet<Monastery> _monasteries;
        private HashSet<Road> _roads;

        public ScoreBoard ScoreBoard { get; set; }

        private int id = 0;

        public Board(HashSet<Client> clients)
        {
            CardCoordinates = new Dictionary<Coordinate, Card>();
            AvailableCoordinates = new Dictionary<RequiredCard, Coordinate>();
            _cities = new HashSet<City>();
            _grassLands = new HashSet<GrassLand>();
            _monasteries = new HashSet<Monastery>();
            _roads = new HashSet<Road>();
            ScoreBoard = new ScoreBoard(clients);
        }

        public void AddAvailableCoordinates(Card card)
        {
            if (card.TopIsFree)
            {
                var top = new RequiredCard(new Coordinate { x = card.Coordinate.x, y = card.Coordinate.y + 1 }, CardCoordinates);
                RemoveFromAvailableCoordinates(top.Coordinate);
                AvailableCoordinates.Add(top, top.Coordinate);
            }

            if (card.LeftIsFree)
            {
                var left = new RequiredCard(new Coordinate { x = card.Coordinate.x - 1, y = card.Coordinate.y }, CardCoordinates);
                RemoveFromAvailableCoordinates(left.Coordinate);
                AvailableCoordinates.Add(left, left.Coordinate);
            }

            if (card.BottomIsFree)
            {
                var bottom = new RequiredCard(new Coordinate { x = card.Coordinate.x, y = card.Coordinate.y - 1 }, CardCoordinates);
                RemoveFromAvailableCoordinates(bottom.Coordinate);
                AvailableCoordinates.Add(bottom, bottom.Coordinate);
            }


            if (card.RightIsFree)
            {
                var right = new RequiredCard(new Coordinate { x = card.Coordinate.x + 1, y = card.Coordinate.y }, CardCoordinates);
                RemoveFromAvailableCoordinates(right.Coordinate);
                AvailableCoordinates.Add(right, right.Coordinate);
            }
        }

        public void RemoveFromAvailableCoordinates(Coordinate coordinate)
        {
            var item = AvailableCoordinates.FirstOrDefault(kvp => kvp.Value.Equals(coordinate));
            if (!item.Equals(default(KeyValuePair<RequiredCard, Coordinate>)))
                AvailableCoordinates.Remove(item.Key);
        }

        public void SetSideOccupation(Coordinate coord)
        {
            if (!CardCoordinates.TryGetValue(coord, out Card card))
            {
                throw new ArgumentException();
            }

            var vertical = new Coordinate { x = card.Coordinate.x, y = card.Coordinate.y + 1 };
            card.TopIsFree = CardCoordinates.ContainsKey(vertical) ? false : true;

            vertical.y = card.Coordinate.y - 1;
            card.BottomIsFree = CardCoordinates.ContainsKey(vertical) ? false : true;

            var horizontal = new Coordinate { x = card.Coordinate.x + 1, y = card.Coordinate.y };
            card.RightIsFree = CardCoordinates.ContainsKey(horizontal) ? false : true;

            horizontal.x = card.Coordinate.x - 1;
            card.LeftIsFree = CardCoordinates.ContainsKey(horizontal) ? false : true;

        }

        public void SetRegions(Coordinate coordinate)
        {
            if (!CardCoordinates.TryGetValue(coordinate, out Card card))
            {
                throw new ArgumentException();
            }
            // If there is monastery near the card set them
            var monasteries = _monasteries.Where(m => m.SurroundingCoordinates.Contains(coordinate)).ToList();
            monasteries.ForEach(m => m.SurroundingCoordinates.Remove(coordinate));

            bool roadClosed = SetRoadClosed(card.Tile.Field5.LandType);
            int landCounts = GetLandCount(card);
            var topCoord = new Coordinate { x = coordinate.x, y = coordinate.y + 1 };
            var botCoord = new Coordinate { x = coordinate.x, y = coordinate.y - 1 };
            var rightCoord = new Coordinate { x = coordinate.x + 1, y = coordinate.y };
            var leftCoord = new Coordinate { x = coordinate.x - 1, y = coordinate.y };

            if (card.Tile.Field5.LandType.Name == "Monastery")
            {
                id++;
                _monasteries.Add(new Monastery(id, coordinate) { CardId = card.Id });
                card.MonasteryId = id;
            }

            var Places = new Dictionary<Side, int>();

            PlaceLands(topCoord, botCoord, leftCoord, rightCoord, card, landCounts);

            PlaceCity(topCoord, botCoord, leftCoord, rightCoord, card);

            PlaceRoad(topCoord, botCoord, leftCoord, rightCoord, card, roadClosed);

            // TODO When place cities and roads add them to the lands

        }

        private void PlaceLands(Coordinate topCoord, Coordinate botCoord, Coordinate leftCoord, Coordinate rightCoord, Card card, int landCounts)
        {

            var landsAround = new Dictionary<Side, HashSet<int>>();

            if (landCounts == 0)
                return;

            if (CardCoordinates.TryGetValue(topCoord, out Card topCard))
            {
                switch (topCard.Bottom.Name)
                {
                    case "Land":
                        landsAround.Add(Side.TOP, new HashSet<int> { topCard.Bottom.PlaceId });
                        if (landsAround.ContainsKey(Side.TOPLEFT))
                            landsAround[Side.TOPLEFT].Add(topCard.Tile.Field7.LandType.PlaceId);
                        else
                            landsAround.Add(Side.TOPLEFT, new HashSet<int> { topCard.Tile.Field7.LandType.PlaceId });

                        if (landsAround.ContainsKey(Side.TOPRIGHT))
                            landsAround[Side.TOPRIGHT].Add(topCard.Tile.Field9.LandType.PlaceId);
                        else
                            landsAround.Add(Side.TOPRIGHT, new HashSet<int> { topCard.Tile.Field9.LandType.PlaceId });
                        break;

                    case "Road":
                        if (landsAround.ContainsKey(Side.TOPLEFT))
                            landsAround[Side.TOPLEFT].Add(topCard.Tile.Field7.LandType.PlaceId);
                        else
                            landsAround.Add(Side.TOPLEFT, new HashSet<int> { topCard.Tile.Field7.LandType.PlaceId });

                        if (landsAround.ContainsKey(Side.TOPRIGHT))
                            landsAround[Side.TOPRIGHT].Add(topCard.Tile.Field9.LandType.PlaceId);
                        else
                            landsAround.Add(Side.TOPLEFT, new HashSet<int> { topCard.Tile.Field9.LandType.PlaceId });
                        break;
                }
            }

            if (CardCoordinates.TryGetValue(leftCoord, out Card leftCard))
            {
                switch (leftCard.Right.Name)
                {
                    case "Land":
                        landsAround.Add(Side.MIDDLELEFT, new HashSet<int> { leftCard.Right.PlaceId });
                        if (landsAround.ContainsKey(Side.TOPLEFT))
                            landsAround[Side.TOPLEFT].Add(leftCard.Tile.Field3.LandType.PlaceId);
                        else
                            landsAround.Add(Side.TOPLEFT, new HashSet<int> { leftCard.Tile.Field3.LandType.PlaceId });

                        if (landsAround.ContainsKey(Side.BOTTOMLEFT))
                            landsAround[Side.BOTTOMLEFT].Add(leftCard.Tile.Field9.LandType.PlaceId);
                        else
                            landsAround.Add(Side.TOPLEFT, new HashSet<int> { leftCard.Tile.Field9.LandType.PlaceId });
                        break;

                    case "Road":
                        if (landsAround.ContainsKey(Side.TOPLEFT))
                            landsAround[Side.TOPLEFT].Add(leftCard.Tile.Field3.LandType.PlaceId);
                        else
                            landsAround.Add(Side.TOPLEFT, new HashSet<int> { leftCard.Tile.Field3.LandType.PlaceId });

                        if (landsAround.ContainsKey(Side.BOTTOMLEFT))
                            landsAround[Side.BOTTOMLEFT].Add(leftCard.Tile.Field9.LandType.PlaceId);
                        else
                            landsAround.Add(Side.TOPLEFT, new HashSet<int> { leftCard.Tile.Field9.LandType.PlaceId });
                        break;
                }
            }

            if (CardCoordinates.TryGetValue(botCoord, out Card botCard))
            {
                switch (botCard.Top.Name)
                {
                    case "Land":
                        landsAround.Add(Side.BOTTOM, new HashSet<int> { botCard.Top.PlaceId });
                        if (landsAround.ContainsKey(Side.BOTTOMLEFT))
                            landsAround[Side.BOTTOMLEFT].Add(botCard.Tile.Field1.LandType.PlaceId);
                        else
                            landsAround.Add(Side.BOTTOMLEFT, new HashSet<int> { botCard.Tile.Field1.LandType.PlaceId });

                        if (landsAround.ContainsKey(Side.BOTTOMRIGHT))
                            landsAround[Side.BOTTOMRIGHT].Add(botCard.Tile.Field3.LandType.PlaceId);
                        else
                            landsAround.Add(Side.BOTTOMRIGHT, new HashSet<int> { botCard.Tile.Field3.LandType.PlaceId });
                        break;

                    case "Road":
                        if (landsAround.ContainsKey(Side.BOTTOMLEFT))
                            landsAround[Side.BOTTOMLEFT].Add(botCard.Tile.Field1.LandType.PlaceId);
                        else
                            landsAround.Add(Side.BOTTOMLEFT, new HashSet<int> { botCard.Tile.Field1.LandType.PlaceId });

                        if (landsAround.ContainsKey(Side.BOTTOMRIGHT))
                            landsAround[Side.BOTTOMRIGHT].Add(botCard.Tile.Field3.LandType.PlaceId);
                        else
                            landsAround.Add(Side.BOTTOMRIGHT, new HashSet<int> { botCard.Tile.Field3.LandType.PlaceId });
                        break;
                }
            }

            if (CardCoordinates.TryGetValue(rightCoord, out Card rightCard))
            {
                switch (rightCard.Left.Name)
                {
                    case "Land":
                        landsAround.Add(Side.MIDDLERIGHT, new HashSet<int> { rightCard.Left.PlaceId });
                        if (landsAround.ContainsKey(Side.TOPRIGHT))
                            landsAround[Side.TOPRIGHT].Add(rightCard.Tile.Field1.LandType.PlaceId);
                        else
                            landsAround.Add(Side.TOPRIGHT, new HashSet<int> { rightCard.Tile.Field1.LandType.PlaceId });

                        if (landsAround.ContainsKey(Side.BOTTOMRIGHT))
                            landsAround[Side.BOTTOMRIGHT].Add(rightCard.Tile.Field7.LandType.PlaceId);
                        else
                            landsAround.Add(Side.BOTTOMRIGHT, new HashSet<int> { rightCard.Tile.Field7.LandType.PlaceId });
                        break;

                    case "Road":
                        if (landsAround.ContainsKey(Side.TOPRIGHT))
                            landsAround[Side.TOPRIGHT].Add(rightCard.Tile.Field1.LandType.PlaceId);
                        else
                            landsAround.Add(Side.TOPRIGHT, new HashSet<int> { rightCard.Tile.Field1.LandType.PlaceId });

                        if (landsAround.ContainsKey(Side.BOTTOMRIGHT))
                            landsAround[Side.BOTTOMRIGHT].Add(rightCard.Tile.Field7.LandType.PlaceId);
                        else
                            landsAround.Add(Side.BOTTOMRIGHT, new HashSet<int> { rightCard.Tile.Field7.LandType.PlaceId });
                        break;
                }
            }

            if (landsAround.Count == 0)
            {
                if (landCounts == 1)
                {
                    id++;
                    _grassLands.Add(new GrassLand(id, card.Id));
                    foreach (var field in card.Tile.Sides)
                    {
                        if (field.Value.LandType.Name == "Land")
                            card.SetField(field.Key, id);
                    }
                    return;
                }

                for (int i = 0; i < 2; i++)
                {
                    id++;
                    _grassLands.Add(new GrassLand(id, card.Id));
                }

                if (card.Sides.Count(s => s.Name == "Road") == 2)
                {
                    var closedSide = GetSidesClosedByRoads(card).FirstOrDefault();
                    if (closedSide != default)
                    {
                        card.SetField(closedSide, id);
                        foreach (var field in card.Tile.Sides)
                        {
                            if (field.Key != closedSide && field.Value.LandType.Name == "Land")
                                card.SetField(field.Key, id - 1);
                        }
                        return;
                    }
                    if (card.Top.Name == "Road")
                    {
                        card.SetField(Side.TOPLEFT, id);
                        if (card.Left.Name == "Land")
                            card.SetField(Side.MIDDLELEFT, id);
                        card.SetField(Side.BOTTOMLEFT, id);
                        card.SetField(Side.TOPRIGHT, id - 1);
                        if (card.Right.Name == "Land")
                            card.SetField(Side.MIDDLERIGHT, id - 1);
                        card.SetField(Side.BOTTOMRIGHT, id - 1);
                        return;
                    }
                    card.SetField(Side.TOPLEFT, id);
                    if (card.Top.Name == "Land")
                        card.SetField(Side.TOP, id);
                    card.SetField(Side.TOPRIGHT, id);
                    card.SetField(Side.BOTTOMLEFT, id - 1);
                    if (card.Bottom.Name == "Land")
                        card.SetField(Side.BOTTOM, id - 1);
                    card.SetField(Side.BOTTOMRIGHT, id - 1);

                }

                if (card.Sides.Count(c => c.Name == "City") == 2)
                {

                    if (card.Top.Name == "Land")
                    {
                        card.SetField(Side.TOPLEFT, id);
                        card.SetField(Side.TOP, id);
                        card.SetField(Side.TOPRIGHT, id);
                        card.SetField(Side.BOTTOMLEFT, id - 1);
                        card.SetField(Side.BOTTOM, id - 1);
                        card.SetField(Side.BOTTOMRIGHT, id - 1);
                    }

                    card.SetField(Side.TOPLEFT, id);
                    card.SetField(Side.MIDDLELEFT, id);
                    card.SetField(Side.BOTTOMLEFT, id);
                    card.SetField(Side.TOPRIGHT, id - 1);
                    card.SetField(Side.MIDDLERIGHT, id - 1);
                    card.SetField(Side.BOTTOMRIGHT, id - 1);
                    return;
                }
                var tempId = id;
                foreach (var side in card.Tile.Sides.Where(f => f.Value.LandType.Name == "Land").Select(s => s.Key))
                {
                    card.SetField(side, tempId);
                    tempId--;
                }
                return;
            }


            if (landsAround.Count == 3)
            {
                var land = _grassLands.FirstOrDefault(l => l.Id == landsAround.First().Value.First());
                land.ExpandLand(card.Id);
                if (landCounts == 1)
                {
                    foreach (var side in card.Tile.Sides.Where(f => f.Value.LandType.Name == "Land").Select(s => s.Key))
                    {
                        card.SetField(side, land.Id);
                    }
                    return;
                }

                foreach (var l in landsAround)
                {
                    card.SetField(l.Key, l.Value.First());
                }

                if (landCounts == 2)
                {
                    id++;
                    _grassLands.Add(new GrassLand(id, card.Id));
                    if (card.Top.Name == "Road" && card.Bottom.Name == "Road"
                        || card.Left.Name == "Road" && card.Right.Name == "Road"
                        || card.Tile.Field5.LandType.Name == "City")
                    {
                        foreach (var side in card.Tile.Sides.Where(f => f.Value.LandType.Name == "Land").Select(s => s.Key))
                        {
                            if (!landsAround.ContainsKey(side))
                                card.SetField(side, id);
                        }
                        return;
                    }

                    var closedSide = GetSidesClosedByRoads(card).First();
                    card.SetField(closedSide, id);
                    foreach (var field in card.Tile.Sides)
                    {
                        if (field.Key != closedSide && field.Value.LandType.Name == "Land")
                            card.SetField(field.Key, land.Id);
                    }
                    return;
                }

                foreach (var side in card.Tile.Sides.Where(f => f.Value.LandType.Name == "Land").Select(s => s.Key))
                {
                    if (!landsAround.ContainsKey(side))
                    {
                        id++;
                        _grassLands.Add(new GrassLand(id, card.Id));
                        card.SetField(side, id);
                    }
                }
                return;
            }

            if (landCounts == 1)
            {
                var surroundingLandIds = LandIdsAround(landsAround);
                if (surroundingLandIds.Count == 1)
                {
                    var land = _grassLands.First(l => l.Id == surroundingLandIds.First());
                    land.ExpandLand(card.Id);
                    foreach (var side in card.Tile.Sides.Where(f => f.Value.LandType.Name == "Land").Select(s => s.Key))
                    {
                        card.SetField(side, land.Id);
                    }
                    return;
                }

                id++;
                var newLand = new GrassLand(id);
                foreach (var id in surroundingLandIds)
                {
                    var land = _grassLands.First(l => l.Id == id);
                    newLand.Meeples.AddRange(land.Meeples);
                    newLand.Peasants.AddRange(land.Peasants);
                    land.Roads.ToList().ForEach(r => newLand.Roads.Add(r));
                    land.SurroundingCities.ToList().ForEach(c => newLand.SurroundingCities.Add(c));
                    foreach (var cardId in land.CardIds)
                    {
                        var landCard = CardCoordinates.Values.First(c => c.Id == cardId);
                        landCard.Tile.Sides.Where(f => f.Value.LandType.PlaceId == id).ToList().ForEach(t => t.Value.LandType.PlaceId = newLand.Id);
                    }
                }

                foreach (var side in card.Tile.Sides.Where(f => f.Value.LandType.Name == "Land").Select(s => s.Key))
                {
                    card.SetField(side, newLand.Id);
                }

                return;
            }

            var cornerClosedLands = GetSidesClosedByRoads(card);

            var newLands = new List<GrassLand>();
            var left = new Side[] { Side.BOTTOMLEFT, Side.MIDDLELEFT, Side.TOPLEFT };
            var right = new Side[] { Side.BOTTOMRIGHT, Side.MIDDLERIGHT, Side.TOPRIGHT };
            var top = new Side[] { Side.TOPLEFT, Side.TOPRIGHT, Side.TOP };
            var bottom = new Side[] { Side.BOTTOMRIGHT, Side.BOTTOM, Side.BOTTOMLEFT };
            var lefts = new HashSet<int>();
            var rights = new HashSet<int>();
            var bottoms = new HashSet<int>();
            var tops = new HashSet<int>();

            foreach (var around in landsAround)
            {
                if (cornerClosedLands.Contains(around.Key))
                {
                    if (around.Value.Count == 1 || around.Value.Count(i => i == around.Value.First()) == 1)
                    {
                        var land = _grassLands.First(l => l.Id == around.Value.First());
                        land.ExpandLand(card.Id);
                        card.SetField(around.Key, land.Id);
                        continue;
                    }
                    var landToAdd = MergeLands(around.Value);
                    newLands.Add(landToAdd);
                    card.SetField(around.Key, landToAdd.Id);
                    continue;
                }
                foreach (var landId in around.Value)
                {
                    if (card.Top.Name == "Land" && card.Bottom.Name == "Land")
                    {
                        if (left.Contains(around.Key))
                            lefts.Add(landId);
                        else
                            rights.Add(landId);
                        continue;
                    }
                    if (top.Contains(around.Key))
                        tops.Add(landId);
                    else
                        bottoms.Add(landId);
                }
            }
            
            if(lefts.Count != 0)
            {
                if (lefts.Count == 1)
                {
                    var land = _grassLands.First(l => l.Id == lefts.First());
                    land.ExpandLand(card.Id);
                    foreach (var side in card.Tile.Sides.Where(f => f.Value.LandType.Name == "Land").Select(s => s.Key))
                    {
                        if (left.Contains(side))
                            card.SetField(side, land.Id);
                    }
                }
                else
                { 
                    var newLand = MergeLands(lefts);
                    foreach (var side in card.Tile.Sides.Where(f => f.Value.LandType.Name == "Land").Select(s => s.Key))
                    {
                        if (left.Contains(side))
                            card.SetField(side, newLand.Id);
                    }
                    newLands.Add(newLand);
                }
            }

            if (rights.Count != 0)
            {
                if (rights.Count == 1)
                {
                    var land = _grassLands.First(l => l.Id == rights.First());
                    land.ExpandLand(card.Id);
                    foreach (var side in card.Tile.Sides.Where(f => f.Value.LandType.Name == "Land").Select(s => s.Key))
                    {
                        if (right.Contains(side))
                            card.SetField(side, land.Id);
                    }
                }
                else
                {
                    var newLand = MergeLands(rights);
                    foreach (var side in card.Tile.Sides.Where(f => f.Value.LandType.Name == "Land").Select(s => s.Key))
                    {
                        if (right.Contains(side))
                            card.SetField(side, newLand.Id);
                    }
                    newLands.Add(newLand);
                }
            }

            if (tops.Count != 0)
            {
                if (tops.Count == 1)
                {
                    var land = _grassLands.First(l => l.Id == tops.First());
                    land.ExpandLand(card.Id);
                    foreach (var side in card.Tile.Sides.Where(f => f.Value.LandType.Name == "Land").Select(s => s.Key))
                    {
                        if (top.Contains(side))
                            card.SetField(side, land.Id);
                    }
                }
                else
                {
                    var newLand = MergeLands(tops);
                    foreach (var side in card.Tile.Sides.Where(f => f.Value.LandType.Name == "Land").Select(s => s.Key))
                    {
                        if (top.Contains(side))
                            card.SetField(side, newLand.Id);
                    }
                    newLands.Add(newLand);
                }
            }

            if (bottoms.Count != 0)
            {
                if (bottoms.Count == 1)
                {
                    var land = _grassLands.First(l => l.Id == bottoms.First());
                    land.ExpandLand(card.Id);
                    foreach (var side in card.Tile.Sides.Where(f => f.Value.LandType.Name == "Land").Select(s => s.Key))
                    {
                        if (bottom.Contains(side))
                            card.SetField(side, land.Id);
                    }
                }
                else
                {
                    var newLand = MergeLands(bottoms);
                    foreach (var side in card.Tile.Sides.Where(f => f.Value.LandType.Name == "Land").Select(s => s.Key))
                    {
                        if (bottom.Contains(side))
                            card.SetField(side, newLand.Id);
                    }
                    newLands.Add(newLand);
                }
            }
            newLands.ForEach(l => _grassLands.Add(l));
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

        private void PlaceRoad(Coordinate topCoord, Coordinate botCoord, Coordinate leftCoord, Coordinate rightCoord, Card card, bool roadClosed)
        {
            var roadsCount = roadClosed ? card.Sides.Count(l => l.Name == "Road") : 1;

            var roadsAround = new Dictionary<Side, int>();

            var visitedSides = new HashSet<Side>();

            if (roadsCount == 0)
                return;

            if (CardCoordinates.TryGetValue(topCoord, out Card topCard) && topCard.Bottom.Name == "Road")
                roadsAround.Add(Side.TOP, topCard.Bottom.PlaceId);
            if (CardCoordinates.TryGetValue(leftCoord, out Card leftCard) && leftCard.Right.Name == "Road")
                roadsAround.Add(Side.MIDDLELEFT, leftCard.Right.PlaceId);
            if (CardCoordinates.TryGetValue(botCoord, out Card botCard) && botCard.Top.Name == "Road")
                roadsAround.Add(Side.BOTTOM, botCard.Top.PlaceId);
            if (CardCoordinates.TryGetValue(rightCoord, out Card rightCard) && rightCard.Left.Name == "Road")
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
                foreach (var side in card.Tile.Sides.Where(f => f.Value.LandType.Name == "Road").Select(s => s.Key))
                {
                    if (side != Side.MIDDLE)
                    {
                        if (roadsCount != 1)
                        {
                            tempId--;
                        }
                        card.SetField(side, tempId);
                        AddRoadToLand(side, tempId, card);
                    }
                }
            }
            else
            {
                id++;
                var newRoad = new Road(id);

                foreach (var around in roadsAround)
                {
                    var road = _roads.First(r => r.Id == around.Value);
                    switch (around.Key)
                    {
                        case Side.TOP:
                            road.SetSides(topCard.Id);
                            visitedSides.Add(Side.TOP);
                            break;
                        case Side.MIDDLELEFT:
                            road.SetSides(leftCard.Id);
                            visitedSides.Add(Side.MIDDLELEFT);
                            break;
                        case Side.BOTTOM:
                            road.SetSides(botCard.Id);
                            visitedSides.Add(Side.BOTTOM);
                            break;
                        case Side.MIDDLERIGHT:
                            road.SetSides(rightCard.Id);
                            visitedSides.Add(Side.MIDDLERIGHT);
                            break;
                    }
                    if (roadClosed)
                    {
                        road.ExpandRoad(new RoadPart(card.Id) { LeftOpen = false, RightOpen = false });
                        card.SetField(around.Key, road.Id);
                        AddRoadToLand(around.Key, road.Id, card);
                    }
                    else
                    {

                        card.SetField(around.Key, newRoad.Id);
                        AddRoadToLand(around.Key, newRoad.Id, card);

                        foreach (var rp in road.RoadParts)
                        {
                            newRoad.ExpandRoad(rp);
                            var roadCard = CardCoordinates.Values.FirstOrDefault(r => r.Id == rp.CardId);
                            foreach (var side in roadCard.Tile.Sides)
                            {
                                if (side.Value.LandType.PlaceId == road.Id)
                                    roadCard.SetField(side.Key, newRoad.Id);
                                if (side.Value.LandType.Name == "Land")
                                {
                                    var land =  _grassLands.First(l => l.Id == side.Value.LandType.PlaceId);
                                    land.Roads.RemoveWhere(r => r == road.Id);
                                    land.Roads.Add(newRoad.Id);
                                }
                            }
                        }
                        _roads.Remove(road);
                    }
                    roadsCount--;
                }

                if (roadClosed)
                {
                    id--;
                    foreach (var side in card.Tile.Sides.Where(f => f.Value.LandType.Name == "Road").Select(s => s.Key))
                    {
                        if (!visitedSides.Contains(side))
                        {
                            id++;
                            var road = new Road(id);
                            var roadPart = new RoadPart(card.Id);
                            roadPart.LeftOpen = false;
                            road.ExpandRoad(roadPart);
                            _roads.Add(road);
                            card.SetField(side, id);
                            AddRoadToLand(side, id, card);
                        }
                    }
                }
                else
                {
                    _roads.Add(newRoad);
                }
            }

            return;
        }

        private void AddRoadToLand(Side side, int tempId, Card card)
        {
            switch (side)
            {
                case Side.TOP:
                    _grassLands.First(l => l.Id == card.Tile.Field1.LandType.PlaceId).Roads.Add(tempId);
                    _grassLands.First(l => l.Id == card.Tile.Field3.LandType.PlaceId).Roads.Add(tempId);
                    break;
                case Side.MIDDLELEFT:
                    _grassLands.First(l => l.Id == card.Tile.Field1.LandType.PlaceId).Roads.Add(tempId);
                    _grassLands.First(l => l.Id == card.Tile.Field7.LandType.PlaceId).Roads.Add(tempId);
                    break;
                case Side.BOTTOM:
                    _grassLands.First(l => l.Id == card.Tile.Field7.LandType.PlaceId).Roads.Add(tempId);
                    _grassLands.First(l => l.Id == card.Tile.Field9.LandType.PlaceId).Roads.Add(tempId);
                    break;
                case Side.MIDDLERIGHT:
                    _grassLands.First(l => l.Id == card.Tile.Field3.LandType.PlaceId).Roads.Add(tempId);
                    _grassLands.First(l => l.Id == card.Tile.Field9.LandType.PlaceId).Roads.Add(tempId);
                    break;
            }
        }

        private void PlaceCity(Coordinate topCoord, Coordinate botCoord, Coordinate leftCoord, Coordinate rightCoord, Card card)
        {
            int cityCounts = GetCityCount(card);
            if (cityCounts == 2)
            {
                PlaceTwoCity(topCoord, botCoord, leftCoord, rightCoord, card);
                return;
            }

            var citiesAround = new Dictionary<Side, int>();

            if (cityCounts == 0)
                return;

            if (CardCoordinates.TryGetValue(topCoord, out Card topCard) && topCard.Bottom.Name == "City")
                citiesAround.Add(Side.TOP, topCard.Bottom.PlaceId);
            if (CardCoordinates.TryGetValue(leftCoord, out Card leftCard) && leftCard.Right.Name == "City")
                citiesAround.Add(Side.MIDDLELEFT, leftCard.Right.PlaceId);
            if (CardCoordinates.TryGetValue(botCoord, out Card botCard) && botCard.Top.Name == "City")
                citiesAround.Add(Side.BOTTOM, botCard.Top.PlaceId);
            if (CardCoordinates.TryGetValue(rightCoord, out Card rightCard) && rightCard.Left.Name == "City")
                citiesAround.Add(Side.MIDDLERIGHT, rightCard.Left.PlaceId);

            var cityPart = new CityPart(card.Id);
            if (card.Top.Name != "City")
                cityPart.TopIsOpen = false;
            if (card.Left.Name != "City")
                cityPart.LeftIsOpen = false;
            if (card.Bottom.Name != "City")
                cityPart.BottomIsOpen = false;
            if (card.Right.Name != "City")
                cityPart.RightIsOpen = false;

            if (citiesAround.Count == 0)
            {
                id++;
                var city = new City(id);
                city.ExpandCity(cityPart);
                _cities.Add(city);
                foreach(var side in card.Tile.Sides)
                {
                    if(side.Value.LandType.Name == "City")
                        card.SetField(side.Key, id);
                    if (side.Value.LandType.Name == "Land")
                        _grassLands.First(l => l.Id == side.Value.LandType.PlaceId).SurroundingCities.Add(city.Id);
                }
            }
            else
            {
                id++;
                var newCity = new City(id);
                foreach (var around in citiesAround)
                {
                    var city = _cities.FirstOrDefault(c => c.Id == around.Value);
                    switch (around.Key)
                    {
                        case Side.TOP:
                            cityPart.TopIsOpen = false;
                            break;
                        case Side.MIDDLELEFT:
                            cityPart.LeftIsOpen = false;
                            break;
                        case Side.BOTTOM:
                            cityPart.BottomIsOpen = false;
                            break;
                        case Side.MIDDLERIGHT:
                            cityPart.LeftIsOpen = false;
                            break;
                    }
                    if (citiesAround.Count == 1)
                    {
                        city.ExpandCity(cityPart);
                        foreach (var side in card.Tile.Sides)
                        {
                            if (side.Value.LandType.Name == "City")
                                card.SetField(side.Key, city.Id);
                            if (side.Value.LandType.Name == "Land")
                                _grassLands.First(l => l.Id == side.Value.LandType.PlaceId).SurroundingCities.Add(city.Id);
                        }
                        id--;
                    }
                    else
                    {
                        foreach (var cp in city.CityParts)
                        {
                            newCity.ExpandCity(cp);
                            var cityCard = CardCoordinates.Values.FirstOrDefault(c => c.Id == cp.CardId);
                            foreach (var side in cityCard.Tile.Sides)
                            {
                                if (side.Value.LandType.PlaceId == city.Id)
                                    cityCard.SetField(side.Key, newCity.Id);
                                if (side.Value.LandType.Name == "Land")
                                {
                                    var land = _grassLands.First(l => l.Id == side.Value.LandType.PlaceId);
                                    land.SurroundingCities.Remove(city.Id);
                                    land.SurroundingCities.Add(newCity.Id);
                                }
                            }
                        }
                        _cities.Remove(city);
                    }
                }
                if (citiesAround.Count > 1)
                {
                    _cities.Add(newCity);
                    foreach (var side in card.Tile.Sides)
                    {
                        if (side.Value.LandType.Name == "City")
                            card.SetField(side.Key, newCity.Id);
                        if (side.Value.LandType.Name == "Land")
                            _grassLands.First(l => l.Id == side.Value.LandType.PlaceId).SurroundingCities.Add(newCity.Id);
                    }
                }
            }
            return;
        }

        private void PlaceTwoCity(Coordinate topCoord, Coordinate botCoord, Coordinate leftCoord, Coordinate rightCoord, Card card)
        {
            if (card.Top.Name == "City")
            {
                if (CardCoordinates.TryGetValue(topCoord, out Card topCard))
                {
                    var city = _cities.FirstOrDefault(c => c.Id == topCard.Bottom.PlaceId);
                    card.SetField(Side.TOP, city.Id);
                    if (city.GetCityPartByCardId(card.Id) == null)
                        city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false });
                    city.SetSides(topCard.Id, Side.BOTTOM);
                    _grassLands.First(l => l.Id == card.Tile.Field5.LandType.PlaceId).SurroundingCities.Add(city.Id);
                }
                else
                {
                    id++;
                    var city = new City(id);
                    city.ExpandCity(new CityPart(card.Id) { TopIsOpen = true, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false });
                    _cities.Add(city);
                    card.SetField(Side.TOP, id);
                    _grassLands.First(l => l.Id == card.Tile.Field5.LandType.PlaceId).SurroundingCities.Add(city.Id);
                }

            }

            if (card.Left.Name == "City")
            {

                if (CardCoordinates.TryGetValue(leftCoord, out Card leftCard))
                {
                    var city = _cities.FirstOrDefault(c => c.Id == leftCard.Right.PlaceId);
                    card.SetField(Side.MIDDLELEFT, city.Id);
                    if (city.GetCityPartByCardId(card.Id) == null)
                        city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false });
                    city.SetSides(leftCard.Id, Side.MIDDLERIGHT);
                    _grassLands.First(l => l.Id == card.Tile.Field5.LandType.PlaceId).SurroundingCities.Add(city.Id);

                }
                else
                {
                    id++;
                    var city = new City(id);
                    city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = true, BottomIsOpen = false, RightIsOpen = false });
                    _cities.Add(city);
                    card.SetField(Side.MIDDLELEFT, id);
                    _grassLands.First(l => l.Id == card.Tile.Field5.LandType.PlaceId).SurroundingCities.Add(city.Id);
                }
            }

            if (card.Bottom.Name == "City")
            {
                if (CardCoordinates.TryGetValue(botCoord, out Card botCard))
                {
                    var city = _cities.FirstOrDefault(c => c.Id == botCard.Top.PlaceId);
                    card.SetField(Side.BOTTOM, city.Id);
                    if (city.GetCityPartByCardId(card.Id) == null)
                        city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false });
                    city.SetSides(botCard.Id, Side.TOP);
                    _grassLands.First(l => l.Id == card.Tile.Field5.LandType.PlaceId).SurroundingCities.Add(city.Id);

                }
                else
                {
                    id++;
                    var city = new City(id);
                    city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = true, RightIsOpen = false });
                    _cities.Add(city);
                    card.SetField(Side.BOTTOM, id);
                    _grassLands.First(l => l.Id == card.Tile.Field5.LandType.PlaceId).SurroundingCities.Add(city.Id);

                }
            }

            if (card.Right.Name == "City")
            {

                if (CardCoordinates.TryGetValue(rightCoord, out Card rightCard))
                {
                    var city = _cities.FirstOrDefault(c => c.Id == rightCard.Right.PlaceId);
                    card.SetField(Side.MIDDLERIGHT, city.Id);
                    if (city.GetCityPartByCardId(card.Id) == null)
                        city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false });
                    city.SetSides(rightCard.Id, Side.MIDDLELEFT);
                    _grassLands.First(l => l.Id == card.Tile.Field5.LandType.PlaceId).SurroundingCities.Add(city.Id);

                }
                else
                {
                    id++;
                    var city = new City(id);
                    city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = true });
                    _cities.Add(city);
                    card.SetField(Side.MIDDLERIGHT, id);
                    _grassLands.First(l => l.Id == card.Tile.Field5.LandType.PlaceId).SurroundingCities.Add(city.Id);
                }
            }

            return;
        }

        private int GetCityCount(Card card)
        {
            if (card.Sides.Count(s => s.Name == "City") == 0)
                return 0;
            if (card.Tile.Field5.LandType.Name == "Land")
            {
                return card.Sides.Count(s => s.Name == "City");
            }
            return 1;
        }

        private int GetLandCount(Card card)
        {
            var roadCount = card.Sides.Count(s => s.Name == "Road");
            if (roadCount != 0)
            {
                if (card.Sides.Count(s => s.Name == "City") == 3)
                    return 2;
                return roadCount;
            }
            if (card.Top.Name == "City" && card.Bottom.Name == "City" && card.Tile.Field5.LandType.Name == "City" ||
                card.Left.Name == "City" && card.Right.Name == "City" && card.Tile.Field5.LandType.Name == "City")
                return card.Sides.Count(s => s.Name == "Land");

            if (card.Sides.Count(l => l.Name == "Land") >= 2)
                return 1;

            return 0;
        }

        private bool SetRoadClosed(LandType landType)
        {
            var names = new string[] { "Monastery", "City", "Other" };
            return names.Contains(landType.Name);
        }
        private GrassLand MergeLands(HashSet<int> around)
        {
            id++;
            var newLand = new GrassLand(id);
            foreach (var landId in around)
            {
                var land = _grassLands.First(l => l.Id == landId);
                newLand.Meeples.AddRange(land.Meeples);
                newLand.Peasants.AddRange(land.Peasants);
                land.Roads.ToList().ForEach(r => newLand.Roads.Add(r));
                land.SurroundingCities.ToList().ForEach(s => newLand.SurroundingCities.Add(s));
                foreach (var cardId in land.CardIds)
                {
                    var landCard = CardCoordinates.Values.First(c => c.Id == cardId);
                    landCard.Tile.Sides.Where(f => f.Value.LandType.PlaceId == id).ToList().ForEach(t => t.Value.LandType.PlaceId = newLand.Id);
                }
                _grassLands.Remove(land);
            }
            return newLand;
        }

        internal void PlaceMeeple(int placeOfMeeple, Card placedCard, Client owner)
        {
            var landtype = placedCard.Tile.Fields[placeOfMeeple - 1].LandType;

            switch (landtype.Name)
            {
                case "City":
                    var city = _cities.FirstOrDefault(c => c.CityParts.Where(part => part.CardId == placedCard.Id) != null);
                    if (city != null && city.CanPlaceMeeple)
                    {
                        city.PlaceMeeple(owner, placeOfMeeple, placedCard);
                    }
                    break;
                case "Road":
                    var road = _roads.FirstOrDefault(r => r.RoadParts.Where(part => part.CardId == placedCard.Id) != null);
                    if (road != null && road.CanPlaceMeeple)
                    {
                        road.PlaceMeeple(owner, placeOfMeeple, placedCard);
                    }
                    break;
                case "Land":
                    var land = _grassLands.FirstOrDefault(g => g.CardIds.Contains(placedCard.Id));
                    if(land != null && land.CanPlaceMeeple)
                    {
                        land.PlaceMeeple(owner, placeOfMeeple, placedCard);
                    }
                    break;
                case "Monastery":
                    var monastery = _monasteries.FirstOrDefault(m => m.CardId == placedCard.Id);
                    if (monastery != null && monastery.CanPlaceMeeple)
                    {
                        monastery.PlaceMeeple(owner, placeOfMeeple, placedCard);
                    }
                    break;
            }            
        }
        internal void CountScores()
        {
            foreach(var city in _cities)
            {
                if (!city.IsOpen && !city.IsCounted)
                {
                    ScoreBoard.CheckOwnerOfCity(city);
                    city.RemoveMeeples();
                    city.IsCounted = true;
                }
            }

            foreach(var road in _roads)
            {
                if (!road.IsOpen && !road.IsCounted)
                {
                    ScoreBoard.CheckOwnerOfRoad(road);
                    road.RemoveMeeples();
                    road.IsCounted = true;
                }
            }

            foreach(var monastery in _monasteries)
            {
                if (monastery.IsFinished && !monastery.IsCounted)
                {
                    ScoreBoard.AddPointsForMonastery(monastery);
                    monastery.RemoveMeeples();
                    monastery.IsCounted = true;
                }
            }
        }

        internal void CountEndScores()
        {
            foreach (var city in _cities)
            {
                if (city.IsOpen && !city.CanPlaceMeeple)
                {
                    ScoreBoard.CheckOwnerOfCity(city);
                }
            }

            foreach (var road in _roads)
            {
                if (road.IsOpen && !road.CanPlaceMeeple)
                {
                    ScoreBoard.CheckOwnerOfRoad(road);
                }
            }

            foreach (var monastery in _monasteries)
            {
                if (!monastery.IsFinished && !monastery.CanPlaceMeeple)
                {
                    ScoreBoard.AddPointsForMonastery(monastery);
                }
            }

            foreach (var grassland in _grassLands)
            {
                //TODO if grassland has finished cities...
            }
        }

        internal void CheckWinner()
        {
            Console.WriteLine("And the winner(s) is(/are): ");
            foreach(var player in ScoreBoard.GetWinner())
            {
                Console.WriteLine(player.Name);
            }

            Console.WriteLine("Points: ");
            foreach(var player in ScoreBoard.Players)
            {
                Console.WriteLine($"{player.Key.Name}: {player.Value}");
            }
        }
    }
}
