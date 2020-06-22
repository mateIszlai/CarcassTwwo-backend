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
    }
}
