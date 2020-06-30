using CarcassTwwo.Models;
using CarcassTwwo.Models.Places;
using NUnit.Framework;
using System.Collections.Generic;

namespace CarcassTwwoTests
{
    [TestFixture]
    class CardTests
    {
        Card card;
        [SetUp]
        public void SetUp()
        {
            Client owner = new Client();
            card = new Card(new Tile
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
        public void Teardown()
        {
            card = null;
        }

        [Test]
        public void SetRotationsWorks()
        {
            card.SetRotations();

            var lands = card.Rotations["0"][0].Name == "Land" &&
                card.Rotations["0"][1].Name == "Land" &&
                card.Rotations["0"][2].Name == "Land" &&
                card.Rotations["0"][3].Name == "Land" &&
                card.Rotations["0"][5].Name == "Land" &&
                card.Rotations["0"][6].Name == "Land" &&
                card.Rotations["0"][8].Name == "Land";
            var monastery = card.Rotations["0"][4].Name == "Monastery";
            var road = card.Rotations["0"][7].Name == "Road";
            Assert.IsTrue(lands);
            Assert.IsTrue(monastery);
            Assert.IsTrue(road);

            lands = card.Rotations["180"][0].Name == "Land" &&
                card.Rotations["180"][2].Name == "Land" &&
                card.Rotations["180"][3].Name == "Land" &&
                card.Rotations["180"][5].Name == "Land" &&
                card.Rotations["180"][6].Name == "Land" &&
                card.Rotations["180"][7].Name == "Land" &&
                card.Rotations["180"][8].Name == "Land";
            monastery = card.Rotations["180"][4].Name == "Monastery";
            road = card.Rotations["180"][1].Name == "Road";
            Assert.IsTrue(lands);
            Assert.IsTrue(monastery);
            Assert.IsTrue(road);
        }

        [Test]
        public void RotateWorks()
        {
            var beforeField1 = card.Tile.Field1;
            var beforeField2 = card.Tile.Field2;
            card.Rotate("180");
            var afterField1 = card.Tile.Field1;
            var afterField2 = card.Tile.Field2;
            Assert.AreEqual(beforeField1.Name, afterField1.Name);
            Assert.AreNotEqual(beforeField2.Name, afterField2.Name);
        }

        [Test]
        public void SetFieldWorks()
        {
            var placeId = 2;
            var placeId2 = 7;
            card.SetField(Side.BOTTOM, placeId);
            Assert.AreEqual(placeId, card.Tile.Field8.PlaceId);
            card.SetField(Side.MIDDLERIGHT, placeId2);
            Assert.AreEqual(placeId2, card.Tile.Field6.PlaceId);

        }

    }
}
