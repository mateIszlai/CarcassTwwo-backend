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
                var top = new RequiredCard();
                top.Coordinate = new Coordinate { x = card.Coordinate.x, y = card.Coordinate.y + 1 };
                top.UpdateRequiredCard(CardCoordinates);
                RemoveFromAvailableCoordinates(top.Coordinate);
                    
                AvailableCoordinates.Add(top, top.Coordinate);
            }

            if (card.LeftIsFree)
            {
                var left = new RequiredCard();
                left.Coordinate = new Coordinate { x = card.Coordinate.x - 1, y = card.Coordinate.y };
                left.UpdateRequiredCard(CardCoordinates);
                RemoveFromAvailableCoordinates(left.Coordinate);
                AvailableCoordinates.Add(left, left.Coordinate);
            }

            if (card.BottomIsFree)
            {
                var bottom = new RequiredCard();
                bottom.Coordinate = new Coordinate { x = card.Coordinate.x, y = card.Coordinate.y - 1 };
                bottom.UpdateRequiredCard(CardCoordinates);
                RemoveFromAvailableCoordinates(bottom.Coordinate);
                AvailableCoordinates.Add(bottom, bottom.Coordinate);
            }


            if (card.RightIsFree)
            {
                var right = new RequiredCard();
                right.Coordinate = new Coordinate { x = card.Coordinate.x + 1, y = card.Coordinate.y };
                right.UpdateRequiredCard(CardCoordinates);
                RemoveFromAvailableCoordinates(right.Coordinate);
                AvailableCoordinates.Add(right, right.Coordinate);
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
            
            vertical.y = card.Coordinate.y - 1;
            card.BottomIsFree = CardCoordinates.ContainsKey(vertical) ? false : true;

            var horizontal = new Coordinate { x = card.Coordinate.x + 1, y = card.Coordinate.y };
            card.RightIsFree = CardCoordinates.ContainsKey(horizontal) ? false : true;
           
            horizontal.x = card.Coordinate.x - 1;
            card.LeftIsFree = CardCoordinates.ContainsKey(horizontal) ? false : true;
           
        }
    }
}
