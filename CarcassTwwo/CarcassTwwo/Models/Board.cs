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
        public List <Meeple> RemovableMeeples = new List<Meeple>();

        public ScoreBoard ScoreBoard { get; set; }

        private readonly Side[] LEFT = new Side[] { Side.BOTTOMLEFT, Side.MIDDLELEFT, Side.TOPLEFT };
        private readonly Side[] RIGHT = new Side[] { Side.BOTTOMRIGHT, Side.MIDDLERIGHT, Side.TOPRIGHT };
        private readonly Side[] TOP = new Side[] { Side.TOPLEFT, Side.TOPRIGHT, Side.TOP };
        private readonly Side[] BOTTOM = new Side[] { Side.BOTTOMRIGHT, Side.BOTTOM, Side.BOTTOMLEFT };
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

            bool roadClosed = SetRoadClosed(card);
            int landCounts = GetLandCount(card);
            var topCoord = new Coordinate { x = coordinate.x, y = coordinate.y + 1 };
            var botCoord = new Coordinate { x = coordinate.x, y = coordinate.y - 1 };
            var rightCoord = new Coordinate { x = coordinate.x + 1, y = coordinate.y };
            var leftCoord = new Coordinate { x = coordinate.x - 1, y = coordinate.y };

            if (card.Tile.Field5.Name == "Monastery")
            {
                var surroundingCoords = new List<Coordinate> { 
                    new Coordinate { x = coordinate.x - 1, y = coordinate.y + 1 },
                    topCoord,
                    new Coordinate { x = coordinate.x + 1, y = coordinate.y + 1 },
                    leftCoord,
                    rightCoord,
                    new Coordinate { x = coordinate.x - 1, y = coordinate.y - 1 },
                    botCoord,
                    new Coordinate { x = coordinate.x + 1, y = coordinate.y - 1 }
                };
                PlaceMonastery(card, surroundingCoords);
                
            }

            var Places = new Dictionary<Side, int>();

            PlaceLands(topCoord, botCoord, leftCoord, rightCoord, card, landCounts);

            PlaceCity(topCoord, botCoord, leftCoord, rightCoord, card, landCounts);

            PlaceRoad(topCoord, botCoord, leftCoord, rightCoord, card, roadClosed);


        }

        private void PlaceMonastery(Card card, List<Coordinate> surroundingCoords)
        {
            id++;
            var monastery = new Monastery(id, card.Coordinate);
            card.MonasteryId = id;
            surroundingCoords.ForEach(coord => {
                if (CardCoordinates.ContainsKey(coord))
                    monastery.SurroundingCoordinates.Remove(coord);
            });
            _monasteries.Add(monastery);
            card.SetField(Side.MIDDLE, id);
        }

        internal List<int> GetMeeplePlaces(int cardId)
        {
            var places = new List<int>();
            var card = CardCoordinates.First(c => c.Value.Id == cardId).Value;
            var ids = new List<int>();
            foreach (var id in card.PlaceIds)
            {
                switch(card.Tile.Sides.Where(s => s.Value.PlaceId == id).Select(t => t.Value.Name).First())
                {
                    case "City":
                        if (_cities.First(c => c.Id == id).Meeples.Count == 0)
                            ids.Add(id);
                        break;
                    case "Road":
                        if (_roads.First(r => r.Id == id).Meeples.Count == 0)
                            ids.Add(id);
                        break;
                    case "Land":
                        if (_grassLands.First(l => l.Id == id).Meeples.Count == 0)
                            ids.Add(id);
                        break;
                    case "Monastery":
                        ids.Add(id);
                        break;

                }
            }

            for (int i = 0; i < 9; i++)
            {
                if (ids.Contains(card.Tile.FieldsPlaceIds[i]))
                    places.Add(i + 1);
            }
            return places;
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

            if (CardCoordinates.TryGetValue(leftCoord, out Card leftCard))
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

            if (CardCoordinates.TryGetValue(botCoord, out Card botCard))
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

            if (CardCoordinates.TryGetValue(rightCoord, out Card rightCard))
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
                    _grassLands.Add(new GrassLand(id, card.Id));
                    foreach (var field in card.Tile.Sides)
                    {
                        if (field.Value.Name == "Land")
                            card.SetField(field.Key, id);
                    }
                    return;
                }

                var closedSides = GetSidesClosedByRoads(card);

                if(closedSides.Count != 0)
                {
                    id++;
                    var newLand = new GrassLand(id, card.Id);
                    _grassLands.Add(newLand);
                    foreach(var side in card.Tile.Sides.Where(s => s.Value.Name == "Land").Select(t => t.Key))
                    {
                        if (closedSides.Contains(side))
                        {
                            id++;
                            _grassLands.Add(new GrassLand(id, card.Id));
                            card.SetField(side, id);
                        }
                        card.SetField(side, newLand.Id);
                    }
                    return;
                }

                if(card.Sides.Count(s => s.Name == "City") == 3)
                {
                    foreach(var side in card.Tile.Sides.Where(s => s.Value.Name == "Land").Select(t => t.Key))
                    {
                        id++;
                        _grassLands.Add(new GrassLand(id, card.Id));
                        card.SetField(side, id);
                    }
                    return;
                }

                for (int i = 0; i < 2; i++)
                {
                    id++;
                    _grassLands.Add(new GrassLand(id, card.Id));
                }

                if (card.Top.Name == "Road" && card.Bottom.Name == "Road" || card.Top.Name == "City" && card.Bottom.Name == "City")
                {
                    foreach(var side in card.Tile.Sides.Where(s => s.Value.Name == "Land").Select(t => t.Key))
                    {
                        if (LEFT.Contains(side))
                            card.SetField(side, id);
                        else
                            card.SetField(side, id - 1);
                    }
                    return;
                }

                foreach (var side in card.Tile.Sides.Where(s => s.Value.Name == "Land").Select(t => t.Key))
                {
                    if (TOP.Contains(side))
                        card.SetField(side, id);
                    else
                        card.SetField(side, id - 1);
                }
                return;
            }


            if (landsAround.Count == 3)
            {
                var land = _grassLands.FirstOrDefault(l => l.Id == landsAround.First().Value.First());
                land.ExpandLand(card.Id);
                if (landCounts == 1)
                {
                    foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Land").Select(s => s.Key))
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
                        || card.Tile.Field5.Name == "City")
                    {
                        foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Land").Select(s => s.Key))
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
                        if (field.Key != closedSide && field.Value.Name == "Land")
                            card.SetField(field.Key, land.Id);
                    }
                    return;
                }

                foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Land").Select(s => s.Key))
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
                var land = _grassLands.First(l => l.Id == surroundingLandIds.First());
                
                if (surroundingLandIds.Count == 1)
                    land.ExpandLand(card.Id);
                else
                    land = MergeLands(surroundingLandIds, card.Id);

                foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Land").Select(s => s.Key))
                {
                    card.SetField(side, land.Id);
                }

                return;
            }

            var cornerClosedLands = GetSidesClosedByRoads(card);

            if(cornerClosedLands.Count == 0 && card.Tile.Field5.Name == "City")
            {
                foreach(var around in landsAround)
                {
                    var land = _grassLands.First(l => l.Id == around.Value.First());
                    land.ExpandLand(card.Id);
                    card.SetField(around.Key, land.Id);
                }
                return;
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
                            var gland = _grassLands.First(l => l.Id == around.Value.First());
                            visitedSides.Add(around.Key);
                            gland.ExpandLand(card.Id);
                            card.SetField(around.Key, gland.Id);
                            continue;
                        }
                        var landToAdd = MergeLands(around.Value, card.Id);
                        _grassLands.Add(landToAdd);
                        card.SetField(around.Key, landToAdd.Id);
                        visitedSides.Add(around.Key);
                        continue;
                    }
                    foreach (var landId in around.Value)
                        notCorners.Add(landId);
                }

                GrassLand land;

                if(notCorners.Count != 0)
                {
                    land = _grassLands.First(l => l.Id == notCorners.First());
                    if (notCorners.Count == 1)
                        land.ExpandLand(card.Id);
                    else if(notCorners.Count > 1)
                        land = MergeLands(notCorners, card.Id);
                }
                else
                {
                    id++;
                    land = new GrassLand(id, card.Id);
                    _grassLands.Add(land);
                }


                foreach (var side in card.Tile.Sides.Where(s => !visitedSides.Contains(s.Key) && s.Value.Name == "Land").Select(t => t.Key))
                {
                    if (cornerClosedLands.Contains(side))
                    {
                        id++;
                        var newLand = new GrassLand(id, card.Id);
                        card.SetField(side, newLand.Id);
                        _grassLands.Add(newLand);
                        continue;
                    }

                    card.SetField(side, land.Id);
                }

                return;
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
            
            if(lefts.Count != 0)
            {
                var land = _grassLands.First(l => l.Id == lefts.First());
                if (lefts.Count == 1)
                {
                    land.ExpandLand(card.Id);
                }
                else
                    land = MergeLands(lefts, card.Id);

                foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Land" && LEFT.Contains(f.Key)).Select(s => s.Key))
                {
                    card.SetField(side, land.Id);
                }

            }

            if (rights.Count != 0)
            {
                var land = _grassLands.First(l => l.Id == rights.First());
                if (rights.Count == 1)
                {
                    land.ExpandLand(card.Id);
                }
                else
                    land = MergeLands(rights, card.Id);

                foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Land" && RIGHT.Contains(f.Key)).Select(s => s.Key))
                {
                    card.SetField(side, land.Id);
                }
            }

            if (tops.Count != 0)
            {
                var land = _grassLands.First(l => l.Id == tops.First());
                if (tops.Count == 1)
                {
                    land.ExpandLand(card.Id);
                }
                else
                    land = MergeLands(tops, card.Id);

                foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Land" && TOP.Contains(f.Key)).Select(s => s.Key))
                {
                    card.SetField(side, land.Id);
                }
            }

            if (bottoms.Count != 0)
            {
                var land = _grassLands.First(l => l.Id == bottoms.First());
                if (bottoms.Count == 1)
                {
                    land.ExpandLand(card.Id);
                }
                else
                    land = MergeLands(bottoms, card.Id);

                foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Land" && BOTTOM.Contains(f.Key)).Select(s => s.Key))
                {
                    card.SetField(side, land.Id);
                }
            }
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
            var roadsCount = 0;
            var sideRoadsCount = card.Sides.Count(s => s.Name == "Road");
            if (sideRoadsCount != 0)
                roadsCount = roadClosed ? sideRoadsCount : 1;

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
                foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Road").Select(s => s.Key))
                {
                    card.SetField(side, tempId);
                    AddRoadToLand(side, tempId, card);
                    if (roadsCount != 1)
                    {
                        tempId--;
                    }
                }
                return;
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
                if(!roadClosed)
                {
                    card.Tile.Sides.Where(s => s.Value.Name == "Road").Select(t => t.Key).ToList().ForEach(r => card.SetField(r, road.Id));
                    AddRoadToLand(around.Key, road.Id, card);
                    return;
                }
                foreach (var side in card.Tile.Sides.Where(s => s.Value.Name == "Road" && !visitedSides.Contains(s.Key)).Select(t => t.Key))
                {
                    id++;
                    var roadToAdd = new Road(id);
                    roadToAdd.ExpandRoad(new RoadPart(card.Id) { LeftOpen = false, RightOpen = true });
                    card.SetField(side, roadToAdd.Id);
                    AddRoadToLand(side, roadToAdd.Id, card);
                    _roads.Add(roadToAdd);
                }
                return;
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

                if (roadClosed)
                {
                    if(road.RoadParts.Count(rp => rp.CardId == card.Id) == 0)
                        road.ExpandRoad(new RoadPart(card.Id) { LeftOpen = false, RightOpen = false });
                    card.SetField(around.Key, road.Id);
                    AddRoadToLand(around.Key, road.Id, card);
                    continue;
                }

                if(roadsAround.Values.Distinct().Count() == 1)
                {
                    if(road.RoadParts.Count(rp => rp.CardId == card.Id) == 0)
                        road.ExpandRoad(new RoadPart(card.Id) { LeftOpen = false, RightOpen = false });
                    card.SetField(around.Key, road.Id);
                    AddRoadToLand(around.Key, road.Id, card);
                    continue;
                }

                card.SetField(around.Key, newRoad.Id);
                AddRoadToLand(around.Key, newRoad.Id, card);

                foreach (var rp in road.RoadParts)
                {
                    newRoad.ExpandRoad(rp);
                    var roadCard = CardCoordinates.Values.FirstOrDefault(r => r.Id == rp.CardId);
                    foreach (var side in roadCard.Tile.Sides)
                    {
                        if (side.Value.PlaceId == road.Id)
                            roadCard.SetField(side.Key, newRoad.Id);
                        if (side.Value.Name == "Land")
                        {
                            var land =  _grassLands.First(l => l.Id == side.Value.PlaceId);
                            if (land.Roads.Remove(road.Id))
                                land.Roads.Add(newRoad.Id);
                        }
                    }
                }
                _roads.Remove(road);
                roadsCount--;
            }

            if (roadsAround.Values.Distinct().Count() == 1)
                id--;

            if (roadClosed)
            {
                id--;
                foreach (var side in card.Tile.Sides.Where(f => f.Value.Name == "Road" && !visitedSides.Contains(f.Key)).Select(s => s.Key))
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
                return;
            }

            _roads.Add(newRoad);
            
        }

        private void AddRoadToLand(Side side, int tempId, Card card)
        {
            switch (side)
            {
                case Side.TOP:
                    _grassLands.First(l => l.Id == card.Tile.Field1.PlaceId).Roads.Add(tempId);
                    _grassLands.First(l => l.Id == card.Tile.Field3.PlaceId).Roads.Add(tempId);
                    break;
                case Side.MIDDLELEFT:
                    _grassLands.First(l => l.Id == card.Tile.Field1.PlaceId).Roads.Add(tempId);
                    _grassLands.First(l => l.Id == card.Tile.Field7.PlaceId).Roads.Add(tempId);
                    break;
                case Side.BOTTOM:
                    _grassLands.First(l => l.Id == card.Tile.Field7.PlaceId).Roads.Add(tempId);
                    _grassLands.First(l => l.Id == card.Tile.Field9.PlaceId).Roads.Add(tempId);
                    break;
                case Side.MIDDLERIGHT:
                    _grassLands.First(l => l.Id == card.Tile.Field3.PlaceId).Roads.Add(tempId);
                    _grassLands.First(l => l.Id == card.Tile.Field9.PlaceId).Roads.Add(tempId);
                    break;
            }
        }

        private void PlaceCity(Coordinate topCoord, Coordinate botCoord, Coordinate leftCoord, Coordinate rightCoord, Card card, int landCounts)
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
                    if(side.Value.Name == "City")
                        card.SetField(side.Key, id);
                }
                AddCityToLand(card, landCounts, city.Id);
                return;
            }

            if (citiesAround.Values.Distinct().Count() == 1)
            {
                var city = _cities.First(c => c.Id == citiesAround.First().Value);
                foreach (var around in citiesAround)
                {
                    switch (around.Key)
                    {
                        case Side.TOP:
                            cityPart.TopIsOpen = false;
                            city.SetSides(topCard.Id, Side.BOTTOM);
                            break;
                        case Side.MIDDLELEFT:
                            cityPart.LeftIsOpen = false;
                            city.SetSides(leftCard.Id, Side.MIDDLERIGHT);
                            break;
                        case Side.BOTTOM:
                            cityPart.BottomIsOpen = false;
                            city.SetSides(botCard.Id, Side.TOP);
                            break;
                        case Side.MIDDLERIGHT:
                            cityPart.RightIsOpen = false;
                            city.SetSides(rightCard.Id, Side.MIDDLELEFT);
                            break;
                    }
                }
                city.ExpandCity(cityPart);
                card.Tile.Sides.Where(s => s.Value.Name == "City").Select(t => t.Key).ToList().ForEach(side => card.SetField(side, city.Id));
                AddCityToLand(card, landCounts, city.Id);
                return;
            }

            id++;
            var newCity = new City(id);
            foreach (var around in citiesAround)
            {
                var city = _cities.First(c => c.Id == around.Value);
                switch (around.Key)
                {
                    case Side.TOP:
                        cityPart.TopIsOpen = false;
                        city.SetSides(topCard.Id, Side.BOTTOM);
                        break;
                    case Side.MIDDLELEFT:
                        cityPart.LeftIsOpen = false;
                        city.SetSides(leftCard.Id, Side.MIDDLERIGHT);
                        break;
                    case Side.BOTTOM:
                        cityPart.BottomIsOpen = false;
                        city.SetSides(botCard.Id, Side.TOP);
                        break;
                    case Side.MIDDLERIGHT:
                        cityPart.RightIsOpen = false;
                        city.SetSides(rightCard.Id, Side.MIDDLELEFT);
                        break;
                }
                    
                foreach (var cp in city.CityParts)
                {
                    newCity.ExpandCity(cp);
                    var cityCard = CardCoordinates.Values.First(c => c.Id == cp.CardId);
                    foreach (var side in cityCard.Tile.Sides)
                    {
                        if (side.Value.PlaceId == city.Id)
                            cityCard.SetField(side.Key, newCity.Id);
                        if (side.Value.Name == "Land")
                        {
                            var land = _grassLands.First(l => l.Id == side.Value.PlaceId);
                            if(land.SurroundingCities.Remove(city.Id))
                                land.SurroundingCities.Add(newCity.Id);
                        }
                    }
                }
                _cities.Remove(city);
            }

            newCity.ExpandCity(cityPart);    
            _cities.Add(newCity);
            foreach (var side in card.Tile.Sides)
            {
                if (side.Value.Name == "City")
                    card.SetField(side.Key, newCity.Id);
            }
            AddCityToLand(card, landCounts, newCity.Id);        
            return;
        }

        private void AddCityToLand(Card card, int landCounts, int cityId)
        {
            if (landCounts == 0)
                return;
            if (landCounts == 1)
            {
                _grassLands.First(l => l.Id == card.Tile.Sides.First(t => t.Value.Name == "Land").Value.PlaceId).SurroundingCities.Add(cityId);
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
                        _grassLands.First(l => l.Id == side.Value.PlaceId).SurroundingCities.Add(cityId);
                    }
                }
                return;
            }
            var closedLands = GetSidesClosedByRoads(card);
            if (closedLands.Count != 0)
            {
                _grassLands.First(l => l.Id == card.Tile.Sides.First(s => s.Value.Name == "Land" && !closedLands.Contains(s.Key)).Value.PlaceId).SurroundingCities.Add(cityId);
                return;
            }
            if (card.Top.Name == "City" || card.Left.Name == "City")
                _grassLands.First(l => l.Id == card.Tile.Field1.PlaceId).SurroundingCities.Add(cityId);
            else
                _grassLands.First(l => l.Id == card.Tile.Field9.PlaceId).SurroundingCities.Add(cityId);
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
                    _grassLands.First(l => l.Id == card.Tile.Field5.PlaceId).SurroundingCities.Add(city.Id);
                }
                else
                {
                    id++;
                    var city = new City(id);
                    city.ExpandCity(new CityPart(card.Id) { TopIsOpen = true, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false });
                    _cities.Add(city);
                    card.SetField(Side.TOP, id);
                    _grassLands.First(l => l.Id == card.Tile.Field5.PlaceId).SurroundingCities.Add(city.Id);
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
                    _grassLands.First(l => l.Id == card.Tile.Field5.PlaceId).SurroundingCities.Add(city.Id);

                }
                else
                {
                    id++;
                    var city = new City(id);
                    city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = true, BottomIsOpen = false, RightIsOpen = false });
                    _cities.Add(city);
                    card.SetField(Side.MIDDLELEFT, id);
                    _grassLands.First(l => l.Id == card.Tile.Field5.PlaceId).SurroundingCities.Add(city.Id);
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
                    _grassLands.First(l => l.Id == card.Tile.Field5.PlaceId).SurroundingCities.Add(city.Id);

                }
                else
                {
                    id++;
                    var city = new City(id);
                    city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = true, RightIsOpen = false });
                    _cities.Add(city);
                    card.SetField(Side.BOTTOM, id);
                    _grassLands.First(l => l.Id == card.Tile.Field5.PlaceId).SurroundingCities.Add(city.Id);

                }
            }

            if (card.Right.Name == "City")
            {

                if (CardCoordinates.TryGetValue(rightCoord, out Card rightCard))
                {
                    var city = _cities.FirstOrDefault(c => c.Id == rightCard.Left.PlaceId);
                    card.SetField(Side.MIDDLERIGHT, city.Id);
                    if (city.GetCityPartByCardId(card.Id) == null)
                        city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = false });
                    city.SetSides(rightCard.Id, Side.MIDDLELEFT);
                    _grassLands.First(l => l.Id == card.Tile.Field5.PlaceId).SurroundingCities.Add(city.Id);

                }
                else
                {
                    id++;
                    var city = new City(id);
                    city.ExpandCity(new CityPart(card.Id) { TopIsOpen = false, LeftIsOpen = false, BottomIsOpen = false, RightIsOpen = true });
                    _cities.Add(city);
                    card.SetField(Side.MIDDLERIGHT, id);
                    _grassLands.First(l => l.Id == card.Tile.Field5.PlaceId).SurroundingCities.Add(city.Id);
                }
            }

            return;
        }

        private int GetCityCount(Card card)
        {
            if (card.Sides.Count(s => s.Name == "City") == 0)
                return 0;
            if (card.Tile.Field5.Name == "Land")
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
            if (card.Top.Name == "City" && card.Bottom.Name == "City" && card.Tile.Field5.Name == "City" ||
                card.Left.Name == "City" && card.Right.Name == "City" && card.Tile.Field5.Name == "City")
                return card.Sides.Count(s => s.Name == "Land");

            if (card.Sides.Count(l => l.Name == "Land") >= 2)
                return 1;

            return 0;
        }

        private bool SetRoadClosed(Card card)
        {
            var names = new string[] { "Monastery", "Other" };
            return names.Contains(card.Tile.Field5.Name) || card.Sides.Count(s => s.Name == "City") == 3;
        }
        private GrassLand MergeLands(HashSet<int> around, int cardId)
        {
            id++;
            var newLand = new GrassLand(id);
            newLand.ExpandLand(cardId);
            foreach (var landId in around)
            {
                var land = _grassLands.First(l => l.Id == landId);
                newLand.Meeples.AddRange(land.Meeples);
                land.Roads.ToList().ForEach(r => newLand.Roads.Add(r));
                land.SurroundingCities.ToList().ForEach(s => newLand.SurroundingCities.Add(s));
                foreach (var landCardId in land.CardIds)
                {
                    newLand.ExpandLand(landCardId);
                    var landCard = CardCoordinates.Values.First(c => c.Id == landCardId);
                    landCard.Tile.Sides.Where(f => f.Value.PlaceId == land.Id).ToList().ForEach(t => landCard.SetField(t.Key, newLand.Id));
                }
                _grassLands.Remove(land);
            }
            _grassLands.Add(newLand);
            return newLand;
        }

        internal void PlaceMeeple(int placeOfMeeple, int cardId, Client owner)
        {
            var placedCard = CardCoordinates.First(c => c.Value.Id == cardId).Value;
            var landtype = placedCard.Tile.Fields[placeOfMeeple - 1];

            switch (landtype.Name)
            {
                case "City":
                    var city = _cities.First(c => c.Id == landtype.PlaceId);
                    if (city.CanPlaceMeeple)
                    {
                        city.PlaceMeeple(owner, placeOfMeeple, placedCard);
                    }
                    break;
                case "Road":
                    var road = _roads.First(r => r.Id == landtype.PlaceId);
                    if (road.CanPlaceMeeple)
                    {
                        road.PlaceMeeple(owner, placeOfMeeple, placedCard);
                    }
                    break;
                case "Land":
                    var land = _grassLands.First(l => l.Id == landtype.PlaceId);
                    if(land.CanPlaceMeeple)
                    {
                        land.PlaceMeeple(owner, placeOfMeeple, placedCard);
                    }
                    break;
                case "Monastery":
                    var monastery = _monasteries.First(m => m.Id == landtype.PlaceId);
                    if (monastery.CanPlaceMeeple)
                    {
                        monastery.PlaceMeeple(owner, placeOfMeeple, placedCard);
                    }
                    break;
            }            
        }
        internal void CountScores()
        {
            RemovableMeeples.Clear();

            foreach(var city in _cities)
            {
                if (!city.IsOpen && !city.IsCounted)
                {
                    ScoreBoard.CheckOwnerOfCity(city);
                    RemovableMeeples.AddRange(city.RemoveMeeples());
                    city.IsCounted = true;
                }
            }

            foreach(var road in _roads)
            {
                if (!road.IsOpen && !road.IsCounted)
                {
                    ScoreBoard.CheckOwnerOfRoad(road);
                    RemovableMeeples.AddRange(road.RemoveMeeples());
                    road.IsCounted = true;
                }
            }

            foreach(var monastery in _monasteries)
            {
                if (monastery.IsFinished && !monastery.IsCounted)
                {
                    ScoreBoard.AddPointsForMonastery(monastery);
                    RemovableMeeples.AddRange(monastery.RemoveMeeples());
                    monastery.IsCounted = true;
                }
            }
        }

        internal Dictionary<Client,int> CountEndScores()
        {
            foreach (var city in _cities)
            {
                if (!city.CanPlaceMeeple && !city.IsCounted)
                {
                    ScoreBoard.CheckOwnerOfCity(city);
                }
            }

            foreach (var road in _roads)
            {
                if (!road.IsCounted && !road.CanPlaceMeeple)
                {
                    ScoreBoard.CheckOwnerOfRoad(road);
                }
            }

            foreach (var monastery in _monasteries)
            {
                if (!monastery.IsCounted && !monastery.CanPlaceMeeple)
                {
                    ScoreBoard.AddPointsForMonastery(monastery);
                }
            }

            foreach (var grassland in _grassLands)
            {
                int finishedCities = GetFinishedCitiesOfLand(grassland);
                ScoreBoard.CheckOwnerOfLand(grassland, finishedCities);
            }

            return ScoreBoard.Players;
        }

        internal Client CheckWinner()
        {
            return ScoreBoard.GetWinner();
        }

        private int GetFinishedCitiesOfLand(GrassLand land)
        {
            int cityCount = 0;
            foreach(var city in land.SurroundingCities)
            {
                if(!_cities.First(c => c.Id == city).IsOpen)
                    cityCount++;
            }

            return cityCount;
        }
    }
}
