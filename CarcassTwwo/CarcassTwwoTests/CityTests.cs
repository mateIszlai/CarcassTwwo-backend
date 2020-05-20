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
            _city = new City();
        }

        [TearDown]
        public void TearDown()
        {
            _city = null;
        }

        [Test]
        public void ExpandCityWorks()
        {
            _city.ExpandCity(new CityPart());
            Assert.AreEqual(1, _city.Size);
        }


        [Test]
        public void GetCityPartByCardIdWorks()
        {
            var cityPart = new CityPart();
            _city.ExpandCity(cityPart);
            Assert.AreEqual(cityPart, _city.GetCityPartByCardId(cityPart.CardId));
        }

        [Test]
        public void SetSidesWorks()
        {
            _city.ExpandCity(new CityPart { CardId = 1, TopIsOpen = true });
            _city.SetSides(new List<CityPart> { new CityPart { CardId = 1, TopIsOpen = false } });
            Assert.IsFalse(_city.GetCityPartByCardId(1).TopIsOpen);
        }
    }
}
