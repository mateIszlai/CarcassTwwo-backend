using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using NUnit.Framework;
using CarcassTwwo.Models;
using CarcassTwwo.Models.Places;
using System.Linq;

namespace CarcassTwwoTests
{
    [TestFixture]
    class ScoreBoardTests
    {
        ScoreBoard scoreBoard;
        Client first;
        Client second;
        HashSet<Client> players;
        City city;
        Road road;
        Monastery monastery;
        [SetUp]
        public void SetUp()
        {
            players = new HashSet<Client>();
            first = new Client();
            second = new Client();
            players.Add(first);
            players.Add(second);
            scoreBoard = new ScoreBoard(players);
            city = new City(2);
            city.CityParts.Add(new CityPart(1) { HasCrest = false });
            city.CityParts.Add(new CityPart(2) { HasCrest = true });
            city.CityParts.Add(new CityPart(3) { HasCrest = false });
            city.Meeples.Add(new Meeple(MeepleType.KNIGHT, 
                first, 4, new Coordinate(), 3));
            
            road = new Road(4);
            road.RoadParts.Add(new RoadPart(4) { LeftOpen = false, RightOpen = false }); ;
            road.RoadParts.Add(new RoadPart(5) { LeftOpen = true, RightOpen = false });
            road.RoadParts.Add(new RoadPart(6) { LeftOpen = false, RightOpen = true });

            monastery = new Monastery(7, new Coordinate());
            monastery.Meeples.Add(new Meeple(MeepleType.MONK, first, 5, new Coordinate(), 7));
            monastery.SurroundingCoordinates.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            scoreBoard = null;
        }

        [Test]
        public void AddPointsForCityWorks()
        {
            var pointbefore = scoreBoard.Players[first];
            scoreBoard.AddPointsForCity(players.ToList(), city);
            var pointafter = scoreBoard.Players[first];
            Assert.AreNotEqual(pointafter, pointbefore);
        }

        [Test]
        public void AddPointsForRoadWorks()
        {
            var pointbefore = scoreBoard.Players[first];
            scoreBoard.AddPointsForRoad(players.ToList(), road);
            var pointafter = scoreBoard.Players[first];
            Assert.AreNotEqual(pointafter, pointbefore);
        }

        [Test]
        public void AddPointsForLandWorks()
        {
            var pointbefore = scoreBoard.Players[first];
            scoreBoard.AddPointsForLand(players.ToList(), 3);
            var pointafter = scoreBoard.Players[first];
            Assert.AreNotEqual(pointafter, pointbefore);
        }

        [Test]
        public void AddPointsForMonasteryWorks()
        {
            var pointbefore = scoreBoard.Players[first];
            scoreBoard.AddPointsForMonastery(monastery);
            var pointafter = scoreBoard.Players[first];
            Assert.AreNotEqual(pointafter, pointbefore);
        }

        [Test]
        public void GetWinnerWorks()
        {
            scoreBoard.Players[first] = 12;
            scoreBoard.Players[second] = 3;
            var winner = scoreBoard.GetWinner();
            Assert.AreEqual(first, winner);
        }
    }
}
