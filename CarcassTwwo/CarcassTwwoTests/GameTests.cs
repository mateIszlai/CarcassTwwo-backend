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

        //[Test]
        //public void GetFirstCard_ReturnsFirstCard()
        //{
        //    var land = new LandType { Name = "Land", Meeple = MeepleType.PEASANT };
        //    var monastery = new LandType { Name = "Monastery", Meeple = MeepleType.MONK };

        //    var expected = new Card(new Tile
        //    {
        //        Field1 = new LandType {  = land },
        //        Field2 = new Field { LandType = land },
        //        Field3 = new Field { LandType = land },
        //        Field4 = new Field { LandType = land },
        //        Field5 = new Field { LandType = monastery },
        //        Field6 = new Field { LandType = land },
        //        Field7 = new Field { LandType = land },
        //        Field8 = new Field { LandType = land },
        //        Field9 = new Field { LandType = land },
        //        Amount = 4,
        //        Remaining = 4,
        //        Image = "../wwwroot/image/1_4.png",
        //        HasCrest = false
        //    }, 1);
        //    var actual = _game.GetFirstCard();

        //    Assert.AreEqual(expected.Id, actual.Id);
        //}


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
