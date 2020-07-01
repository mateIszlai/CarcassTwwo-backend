using CarcassTwwo.Models;
using CarcassTwwo.Models.Places;
using NUnit.Framework;
using System.Linq;

namespace CarcassTwwoTests.PlaceTests
{
    [TestFixture]
    class GrassLandTests
    {
        private GrassLand _land;
        private const int LAND_ID = 0;
        private const int CARD_ID = 1;
        private const int FIELD = 2;
        private Client _owner;
        private Card _card;

        [SetUp]
        public void SetUp()
        {
            _land = new GrassLand(LAND_ID);
            _owner = new Client();
            _card = new Card(new Tile
            {
                Id = 2,
                Field1 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                Field2 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                Field3 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                Field4 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                Field5 = new LandType { Name = "Monastery", Meeple = MeepleType.MONK },
                Field6 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                Field8 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                Image = "../wwwroot/image/2_2.png",
                HasCrest = false
            }, 1);
        }

        [TearDown]
        public void TearDown()
        {
            _land = null;
            _owner = null;
            _card = null;
        }

        [Test]
        public void ExpandLandWorks()
        {
            _land.ExpandLand(CARD_ID);
            var expected = 1;
            var actual = _land.CardIds.Count;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PlaceMeepleAddMeeple()
        {
            int before = _land.Meeples.Count;
            _land.PlaceMeeple(_owner, FIELD, _card);
            int after = _land.Meeples.Count;
            Assert.AreEqual(before + 1, after);
        }

        [Test]
        public void PlaceMeepleAddCorrectType()
        {
            var expected = MeepleType.PEASANT;
            _land.PlaceMeeple(_owner, FIELD, _card);
            var actual = _land.Meeples.First().Type;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PlaceMeepleRemoveOwnersMeepleCount()
        {
            var expected = _owner.MeepleCount - 1;
            _land.PlaceMeeple(_owner, FIELD, _card);
            var actual = _owner.MeepleCount;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PlaceMeepleSetCardsHasMeepleProperty()
        {
            _land.PlaceMeeple(_owner, FIELD, _card);
            Assert.IsTrue(_card.HasMeeple);
        }
    }
}
