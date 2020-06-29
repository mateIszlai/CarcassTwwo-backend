using System;
using System.Collections.Generic;
using System.Text;
using CarcassTwwo;
using CarcassTwwo.Models;
using NUnit.Framework;

namespace CarcassTwwoTests
{
   /* [TestFixture]
    class DataSeederTests
    {
        [SetUp]
        public void SetUp()
        {
            DataSeeder.SeedLandTypes();
            DataSeeder.SeedTiles();
        }

        [TearDown]
        public void TearDown()
        {
            DataSeeder.landTypes = null;
            DataSeeder.tiles = null;
        }

        [Test]
        public void SeedLandTypesGiveBackData()
        {
            Assert.IsNotNull(DataSeeder.landTypes);
        }

        [Test]
        public void LandTypesCountAre5()
        {
            int count = DataSeeder.landTypes.Count;
            Assert.AreEqual(count, 5);
        }

        [Test]
        public void SecondLandtypeInlandTypesIsCity()
        {
            string wanted = "City";
            string actual = DataSeeder.landTypes[1].Name;
            Assert.AreEqual(wanted, actual);
        }

        [Test]
        public void SeedTilesGiveBackData()
        {
            Assert.IsNotNull(DataSeeder.tiles);
        }

        [Test]
        public void SeventhTileIsCorrectlySeeded()
        {
            Assert.AreEqual(DataSeeder.tiles[6].Amount, 2);
            Assert.AreEqual(DataSeeder.tiles[6].HasCrest, true);
            Assert.AreEqual(DataSeeder.tiles[6].Id, 7);
            Assert.AreEqual(DataSeeder.tiles[6].Image, "../wwwroot/image/7_2.png");
        }
        
    }*/
}
