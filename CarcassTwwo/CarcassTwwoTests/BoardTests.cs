using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarcassTwwo;
using CarcassTwwo.Models;
using NUnit.Framework;

namespace CarcassTwwoTests
{
    /*[TestFixture]
    class BoardTests
    {
        Board board;
        Card testCard;
        Card testCard2;

        [SetUp]
        public void SetUp()
        {
            DataSeeder.SeedLandTypes();
            DataSeeder.SeedTiles();
            board = new Board(null);

            testCard = new Card(DataSeeder.tiles[7], 0);
            testCard.BottomIsFree = false;
            testCard.LeftIsFree = testCard.TopIsFree = testCard.RightIsFree = true;
            testCard.Coordinate = new Coordinate() { x = 0, y = 0 };
            board.CardCoordinates.Add(testCard.Coordinate, testCard);

            testCard2 = new Card(DataSeeder.tiles[6], 1);
            testCard2.TopIsFree = testCard2.BottomIsFree = 
                testCard2.LeftIsFree = testCard2.RightIsFree = true;
            testCard2.Coordinate = new Coordinate() { x = -1, y = 0 };
            board.CardCoordinates.Add(testCard2.Coordinate, testCard2);
        }

        [TearDown]
        public void TearDown()
        {
            board = null;
            testCard = null;
            DataSeeder.tiles = null;
            DataSeeder.landTypes = null;
        }

        [Test]
        public void AvailableCoordinatesAreAdded()
        {
            var top = new Coordinate() {
                x = testCard.Coordinate.x,
                y = testCard.Coordinate.y + 1
            };

            var right = new Coordinate()
            {
                x = testCard.Coordinate.x + 1,
                y = testCard.Coordinate.y
            };
            var left = new Coordinate()
            {
                x = testCard.Coordinate.x - 1,
                y = testCard.Coordinate.y
            };

            board.AddAvailableCoordinates(testCard);
            Assert.IsTrue(board.AvailableCoordinates.Values.Contains(top));
            Assert.IsTrue(board.AvailableCoordinates.Values.Contains(left));
            Assert.IsTrue(board.AvailableCoordinates.Values.Contains(right));
        }

        [Test]
        public void UnavailableCoordinatesAreNotAdded()
        {
            var bottom = new Coordinate()
            {
                x = testCard.Coordinate.x,
                y = testCard.Coordinate.y - 1
            };

            board.AddAvailableCoordinates(testCard);
            Assert.IsFalse(board.AvailableCoordinates.Values.Contains(bottom));
            }

        [Test]
        public void CoordinateIsRemovedFromAvailables()
        {
            var right = new Coordinate()
            {
                x = testCard.Coordinate.x + 1,
                y = testCard.Coordinate.y
            };
            board.AddAvailableCoordinates(testCard);
            board.RemoveFromAvailableCoordinates(right);
            Assert.IsFalse(board.AvailableCoordinates.Values.Contains(right));
        }

        [Test]
        public void SideOccupationIsSet()
        {
            var coord = testCard2.Coordinate;
            Assert.IsTrue(testCard2.RightIsFree);
            board.SetSideOccupation(coord);
            Assert.IsFalse(testCard2.RightIsFree);

            var coord2 = testCard.Coordinate;
            Assert.IsTrue(testCard.LeftIsFree);
            board.SetSideOccupation(coord2);
            Assert.IsFalse(testCard.LeftIsFree);

        }

    }*/
}
