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
                var top = new RequiredCard(null,  null, card.Top, null);
                top.Coordinate = new Coordinate { x = card.Coordinate.x, y = card.Coordinate.y + 1 };
                AvailableCoordinates.Add(top, top.Coordinate);
                top.UpdateRequiredCard(CardCoordinates);
            }

            if (card.LeftIsFree)
            {
                var left = new RequiredCard(null, null, null, card.Left);
                left.Coordinate = new Coordinate { x = card.Coordinate.x - 1, y = card.Coordinate.y };
                AvailableCoordinates.Add(left, left.Coordinate);
                left.UpdateRequiredCard(CardCoordinates);
            }

            if (card.BottomIsFree)
            {
                var bottom = new RequiredCard(card.Bottom, null, null, null);
                bottom.Coordinate = new Coordinate { x = card.Coordinate.x, y = card.Coordinate.y - 1 };
                AvailableCoordinates.Add(bottom, bottom.Coordinate);
                bottom.UpdateRequiredCard(CardCoordinates);
            }


            if (card.RightIsFree)
            {
                var right = new RequiredCard(null, card.Right, null, null);
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

        public void SetSideOccupation(Coordinate coord)
        {
            var card = new Card();
            if(!CardCoordinates.TryGetValue(coord, out card))
            {
                throw new ArgumentException();
            }

            var vertical = new Coordinate { x = card.Coordinate.x, y = card.Coordinate.y + 1 };

            card.TopIsFree = CardCoordinates.ContainsKey(vertical) ? false : true;
            if (!card.TopIsFree)
                CardCoordinates[vertical].BottomIsFree = false;

            vertical.y = card.Coordinate.y - 1;
            card.BottomIsFree = CardCoordinates.ContainsKey(vertical) ? false : true;
            if (!card.BottomIsFree)
                CardCoordinates[vertical].TopIsFree = false;

            var horizontal = new Coordinate { x = card.Coordinate.x + 1, y = card.Coordinate.y };
            card.RightIsFree = CardCoordinates.ContainsKey(horizontal) ? false : true;
            if (!card.RightIsFree)
                CardCoordinates[horizontal].LeftIsFree = false;

            horizontal.x = card.Coordinate.x - 1;
            card.LeftIsFree = CardCoordinates.ContainsKey(horizontal) ? false : true;
            if (!card.LeftIsFree)
                CardCoordinates[horizontal].RightIsFree = false;
        }
    }
}
