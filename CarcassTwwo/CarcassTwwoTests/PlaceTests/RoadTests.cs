using CarcassTwwo.Models;
using CarcassTwwo.Models.Places;
using NUnit.Framework;
using System.Linq;

namespace CarcassTwwoTests.PlaceTests
{
    class RoadTests
    {
        private Road _road;
        private const int CARD_ID = 0;
        private const int FIELD = 8;
        private Client _owner;
        private Card _card;

        [SetUp]
        public void SetUp()
        {
            _road = new Road(0);
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
            _road = null;
            _owner = null;
            _card = null;
        }

        [Test]
        public void ExpandCityWorks()
        {
            _road.ExpandRoad(new RoadPart(CARD_ID));
            Assert.AreEqual(1, _road.RoadParts.Count);
        }


        [Test]
        public void SetSidesWorksWhenOnlyLeftIsOpen()
        {
            var roadPart = new RoadPart(CARD_ID);
            roadPart.RightOpen = false;
            _road.ExpandRoad(roadPart);
            _road.SetSides(CARD_ID);
            Assert.IsFalse(_road.IsOpen);
        }

        [Test]
        public void SetSidesWorksWhenOnlyRightIsOpen()
        {
            var roadPart = new RoadPart(CARD_ID);
            roadPart.LeftOpen = false;
            _road.ExpandRoad(roadPart);
            _road.SetSides(CARD_ID);
            Assert.IsFalse(_road.IsOpen);
        }

        [Test]
        public void SetSidesWorksWhenBothSideOpen()
        {
            _road.ExpandRoad(new RoadPart(CARD_ID));
            _road.SetSides(CARD_ID);
            Assert.IsTrue(_road.IsOpen);
            Assert.IsFalse(_road.RoadParts.First(rp => rp.CardId == CARD_ID).LeftOpen);
        }

        [Test]
        public void PlaceMeepleAddMeeple()
        {
            int before = _road.Meeples.Count;
            _road.PlaceMeeple(_owner, FIELD, _card);
            int after = _road.Meeples.Count;
            Assert.AreEqual(before + 1, after);
        }

        [Test]
        public void PlaceMeepleAddCorrectType()
        {
            var expected = MeepleType.HIGHWAYMAN;
            _road.PlaceMeeple(_owner, FIELD, _card);
            var actual = _road.Meeples.First().Type;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PlaceMeepleRemoveOwnersMeepleCount()
        {
            var expected = _owner.MeepleCount - 1;
            _road.PlaceMeeple(_owner, FIELD, _card);
            var actual = _owner.MeepleCount;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PlaceMeepleSetCardsHasMeepleProperty()
        {
            _road.PlaceMeeple(_owner, FIELD, _card);
            Assert.IsTrue(_card.HasMeeple);
        }
    }
}
