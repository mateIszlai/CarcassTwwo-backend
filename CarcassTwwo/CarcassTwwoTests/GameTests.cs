using System;
using System.Collections.Generic;
using System.Text;
using CarcassTwwo.Models;
using NUnit.Framework;

namespace CarcassTwwoTests
{
    [TestFixture]
    public class GameTests
    {
        Game _game = null;

        [SetUp]
        public void SetUp()
        {
            _game = new Game(new HashSet<Client> { new Client { ConnectionId = "0" }, new Client { ConnectionId = "1" }, new Client { ConnectionId = "2" } });
        }

        [TearDown]
        public void TearDown()
        {
            _game = null;
        }

        [Test]
        public void GetFirstCard_ReturnsFirstCard()
        {
            var land = new LandType { Name = "Land", Meeple = MeepleType.PEASANT };
            var monastery = new LandType { Name = "Monastery", Meeple = MeepleType.MONK };

            var expected = new Card(new Tile
            {
                Id = 1,
                Field1 = land,
                Field2 = land,
                Field3 = land,
                Field4 = land,
                Field5 = monastery,
                Field6 = land,
                Field7 = land,
                Field8 = land,
                Field9 = land,
                Image = "../wwwroot/image/1_4.png",
                HasCrest = false
            }, 1);

            var actual = _game.GetFirstCard();

            Assert.AreEqual(expected.Id, actual.Id);

            Assert.AreEqual(expected.Tile.Field1.Name, actual.Tile.Field1.Name);
            Assert.AreEqual(expected.Tile.Field2.Name, actual.Tile.Field2.Name);
            Assert.AreEqual(expected.Tile.Field3.Name, actual.Tile.Field3.Name);
            Assert.AreEqual(expected.Tile.Field4.Name, actual.Tile.Field4.Name);
            Assert.AreEqual(expected.Tile.Field5.Name, actual.Tile.Field5.Name);
            Assert.AreEqual(expected.Tile.Field6.Name, actual.Tile.Field6.Name);
            Assert.AreEqual(expected.Tile.Field7.Name, actual.Tile.Field7.Name);
            Assert.AreEqual(expected.Tile.Field8.Name, actual.Tile.Field8.Name);
            Assert.AreEqual(expected.Tile.Field9.Name, actual.Tile.Field9.Name);

            Assert.AreEqual(expected.Tile.Id, actual.Tile.Id);
            Assert.AreEqual(expected.Tile.Image, actual.Tile.Image);
            Assert.AreEqual(expected.Tile.HasCrest, actual.Tile.HasCrest);
        }


        [Test]
        public void ShuffleCard_ShuffleTheCards()
        {
            var notShuffledFirstCard = _game.GetFirstCard();
            _game.ShuffleCards(5);
            var shuffledFirstCard = _game.GetFirstCard();

            Assert.AreNotEqual(notShuffledFirstCard.Id, shuffledFirstCard.Id);
        }


        [Test]
        public void GetStarterCard_ReturnsStarterCard_WithoutShuffle()
        {
            var expectedId = 20;
            var actualId = _game.GetStarterCard().Tile.Id;

            Assert.AreEqual(expectedId, actualId);
        }

        [Test]
        public void GetStarterCard_ReturnsStarterCard_AfterShuffle()
        {
            var expectedId = 20;
            var actualId = _game.GetStarterCard().Tile.Id;

            Assert.AreEqual(expectedId, actualId);
        }
    }
}
