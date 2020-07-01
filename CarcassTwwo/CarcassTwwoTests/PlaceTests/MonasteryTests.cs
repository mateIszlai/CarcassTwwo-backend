using CarcassTwwo.Models;
using CarcassTwwo.Models.Places;
using NUnit.Framework;
using System.Linq;

namespace CarcassTwwoTests.PlaceTests
{
    [TestFixture]
    class MonasteryTests
    {
        private Monastery _monastery;
        private Client _owner;
        private Card _card;
        private const int MONASTERY_ID = 0;
        private const int CARD_ID = 1;
        private const int FIELD = 5;
        private readonly Coordinate MONASTERY_COORDINATE = new Coordinate { x = 0, y = 0 };
        private readonly Coordinate TOP_COORDINATE = new Coordinate { x = 0, y = 1 };
        private readonly Coordinate TOPLEFT_COORDINATE = new Coordinate { x = - 1, y = 1};
        private readonly Coordinate BOTTOMRIGHT_COORDINATE = new Coordinate { x = 1, y = -1 };


        [SetUp]
        public void SetUp()
        {
            _monastery = new Monastery(MONASTERY_ID, MONASTERY_COORDINATE);
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
            _monastery = null;
            _owner = null;
            _card = null;
        }

        [Test]
        public void ExpandMonasteryWorksWhenRemoveTopCoord()
        {
            _monastery.ExpandMonastery(TOP_COORDINATE);
            Assert.IsFalse(_monastery.SurroundingCoordinates.Contains(TOP_COORDINATE));
        }

        [Test]
        public void ExpandMonasteryWorksWhenRemoveTopLeftCoord()
        {
            _monastery.ExpandMonastery(TOPLEFT_COORDINATE);
            Assert.IsFalse(_monastery.SurroundingCoordinates.Contains(TOPLEFT_COORDINATE));
        }

        [Test]
        public void ExpandMonasteryWorksWhenRemoveBottomRightCoord()
        {
            _monastery.ExpandMonastery(BOTTOMRIGHT_COORDINATE);
            Assert.IsFalse(_monastery.SurroundingCoordinates.Contains(BOTTOMRIGHT_COORDINATE));
        }

        [Test]
        public void PlaceMeepleAddMeeple()
        {
            int before = _monastery.Meeples.Count;
            _monastery.PlaceMeeple(_owner, FIELD, _card);
            int after = _monastery.Meeples.Count;
            Assert.AreEqual(before + 1, after);
        }

        [Test]
        public void PlaceMeepleAddCorrectType()
        {
            var expected = MeepleType.MONK;
            _monastery.PlaceMeeple(_owner, FIELD, _card);
            var actual = _monastery.Meeples.First().Type;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PlaceMeepleRemoveOwnersMeepleCount()
        {
            var expected = _owner.MeepleCount - 1;
            _monastery.PlaceMeeple(_owner, FIELD, _card);
            var actual = _owner.MeepleCount;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PlaceMeepleSetCardsHasMeepleProperty()
        {
            _monastery.PlaceMeeple(_owner, FIELD, _card);
            Assert.IsTrue(_card.HasMeeple);
        }
    }
}
