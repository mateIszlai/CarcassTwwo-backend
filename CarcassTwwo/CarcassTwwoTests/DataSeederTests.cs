using System;
using System.Collections.Generic;
using System.Text;
using CarcassTwwo;
using CarcassTwwo.Models;
using NUnit.Framework;

namespace CarcassTwwoTests
{
    [TestFixture]
    class DataSeederTests
    {
        [SetUp]
        public void SetUp()
        {
            DataSeeder.SeedCards();
        }

        [Test]
        public void SeedCardsGiveBackData()
        {
            Assert.IsNotNull(DataSeeder.Cards);
        }

        [Test]
        public void CardCountIs72()
        {
            int count = DataSeeder.Cards.Count;
            Assert.AreEqual(count, 72);
        }

        [Test]
        public void SeventhCardIsCorrectlySeeded()
        {
            Assert.AreEqual(DataSeeder.Cards[6].Tile.Field5.Name, "City");
            Assert.AreEqual(DataSeeder.Cards[6].HasCrest, true);
            Assert.AreEqual(DataSeeder.Cards[6].Tile.Id, 3);
            Assert.AreEqual(DataSeeder.Cards[6].Id, 7);
            Assert.AreEqual(DataSeeder.Cards[6].Tile.Image, "../wwwroot/image/3_1.png");
        }

    }
}
