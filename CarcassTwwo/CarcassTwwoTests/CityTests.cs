using CarcassTwwo.Models;
using CarcassTwwo.Models.Places;
using NUnit.Framework;
using System.Collections.Generic;

namespace CarcassTwwoTests
{
    [TestFixture]
    public class CityTests
    {
        private  City _city;

        [SetUp]
        public void SetUp()
        {
            _city = new City(0);
        }

        [TearDown]
        public void TearDown()
        {
            _city = null;
        }

        [Test]
        public void ExpandCityWorks()
        {
            _city.ExpandCity(new CityPart(0));
            Assert.AreEqual(1, _city.Size);
        }


        [Test]
        public void GetCityPartByCardIdWorks()
        {
            var cityPart = new CityPart(0);
            _city.ExpandCity(cityPart);
            Assert.AreEqual(cityPart, _city.GetCityPartByCardId(cityPart.CardId));
        }

        [Test]
        public void SetSidesWorks()
        {
            var cityPart = new CityPart(1);
            cityPart.TopIsOpen = true;
            _city.ExpandCity(cityPart);
            _city.SetSides( 1, Side.TOP);
            Assert.IsFalse(_city.GetCityPartByCardId(1).TopIsOpen);
        }

        [Test]
        public void PlaceMeepleWorks()
        {
            Client owner = new Client();
            var card = new Card(new Tile
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

            int before = _city.Meeples.Count;
            _city.PlaceMeeple(owner, 3, card);
            int after = _city.Meeples.Count;
            Assert.AreEqual(0, before);
            Assert.AreEqual(1, after);
            Assert.AreEqual(MeepleType.KNIGHT, _city.Meeples[0].Type);
            Assert.IsTrue(card.HasMeeple);
            Assert.AreEqual(6, owner.MeepleCount);
        }
    }
}
