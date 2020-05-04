using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models
{
    public class RequiredCard
    {
        public LandType Top { get; set; }
        public LandType Bottom { get; set; }
        public LandType Left { get; set; }
        public LandType Right { get; set; }
        public Coordinate Coordinate { get; set; }

        public RequiredCard(LandType top, LandType bottom, LandType left, LandType right)
        {
            Top = top;
            Bottom = bottom;
            Left = left;
            Right = right;
        }

        public void UpdateRequiredCard(Dictionary<Coordinate, Card> CardCoordinates)
        {
            var left = new Coordinate { x = Coordinate.x - 1, y = Coordinate.y };
            var right = new Coordinate { x = Coordinate.x + 1, y = Coordinate.y };
            var top = new Coordinate { x = Coordinate.x, y = Coordinate.y + 1 };
            var bottom = new Coordinate { x = Coordinate.x, y = Coordinate.y - 1 };

            if (CardCoordinates.ContainsKey(left))
            {
                Left = CardCoordinates[left].Right;
            }
            if (CardCoordinates.ContainsKey(right))
            {
                Right = CardCoordinates[right].Left;
            }
            if (CardCoordinates.ContainsKey(top))
            {
                Top = CardCoordinates[top].Bottom;
            }
            if (CardCoordinates.ContainsKey(bottom))
            {
                Bottom = CardCoordinates[bottom].Top;
            }
        }
    }
}
