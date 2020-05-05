using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models
{
    public class Board
    {
        public Dictionary<Coordinate, Card> CardCoordinates { get; set; }
        public Dictionary<RequiredCard, Coordinate> AvailableCoordinates { get; set; }

        public Board()
        {
            CardCoordinates = new Dictionary<Coordinate, Card>();
            AvailableCoordinates = new Dictionary<RequiredCard, Coordinate>();
        }

        public void AddAvailableCoordinates(Card card)
        {
            if (card.TopIsFree)
            {
                var top = new RequiredCard(null, card.Top, null, null);
                top.Coordinate = new Coordinate { x = card.Coordinate.x, y = card.Coordinate.y + 1 };
                AvailableCoordinates.Add(top, top.Coordinate);
                top.UpdateRequiredCard(CardCoordinates);
            }

            if (card.BottomIsFree)
            {
                var bottom = new RequiredCard(card.Bottom, null, null, null);
                bottom.Coordinate = new Coordinate { x = card.Coordinate.x, y = card.Coordinate.y - 1 };
                AvailableCoordinates.Add(bottom, bottom.Coordinate);
                bottom.UpdateRequiredCard(CardCoordinates);
            }

            if (card.LeftIsFree)
            {
                var left = new RequiredCard(null, null, null, card.Left);
                left.Coordinate = new Coordinate { x = card.Coordinate.x - 1, y = card.Coordinate.y };
                AvailableCoordinates.Add(left, left.Coordinate);
                left.UpdateRequiredCard(CardCoordinates);
            }

            if (card.RightIsFree)
            {
                var right = new RequiredCard(null, null, card.Right, null);
                right.Coordinate = new Coordinate { x = card.Coordinate.x + 1, y = card.Coordinate.y };
                AvailableCoordinates.Add(right, right.Coordinate);
                right.UpdateRequiredCard(CardCoordinates);
            }
        }

        public void RemoveFromAvailableCoordinates(Coordinate coordinate)
        {
            var item = AvailableCoordinates.FirstOrDefault(kvp => kvp.Value.Equals(coordinate));
            if(!item.Equals(default(KeyValuePair<RequiredCard, Coordinate>)))
                AvailableCoordinates.Remove(item.Key);
        }

    }


    public struct Coordinate
    {
        public int x { get; set; }
        public int y { get; set; }
    }
}
