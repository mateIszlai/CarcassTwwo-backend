using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models
{
    public class Board
    {
        public Dictionary<Coordinate, Card> CardCoordinates { get; set; }
        public List<Coordinate> AvailableCoordinates { get; set; }
        public void AddAvailableCoordinates(Card card)
        {
            if (card.TopIsFree)
            {
                AvailableCoordinates.Add(new Coordinate { x = card.Coordinate.x, y = card.Coordinate.y + 1 });
            }

            if (card.BottomIsFree)
            {
                AvailableCoordinates.Add(new Coordinate { x = card.Coordinate.x, y = card.Coordinate.y - 1 });
            }

            if (card.LeftIsFree)
            {
                AvailableCoordinates.Add(new Coordinate { x = card.Coordinate.x - 1, y = card.Coordinate.y });
            }

            if (card.RightIsFree)
            {
                AvailableCoordinates.Add(new Coordinate { x = card.Coordinate.x + 1, y = card.Coordinate.y });
            }
        }

        public void RemoveFromAvailableCoordinates(Coordinate coordinate)
        {
            AvailableCoordinates.Remove(coordinate);
        }

    }


    public struct Coordinate
    {
        public int x { get; set; }
        public int y { get; set; }
    }
}
