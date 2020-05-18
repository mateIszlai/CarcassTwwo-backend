using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models
{
    public class RequiredCard
    {
        public LandType Top { get; private set; }
        public LandType Bottom { get; private set; }
        public LandType Left { get; private set; }
        public LandType Right { get; private set; }
        public Coordinate Coordinate { get; private set; }

        public RequiredCard(Coordinate coordinate, Dictionary<Coordinate, Card> cardCoordinates)
        {
            Coordinate = coordinate;

            var left = new Coordinate { x = Coordinate.x - 1, y = Coordinate.y };
            var right = new Coordinate { x = Coordinate.x + 1, y = Coordinate.y };
            var top = new Coordinate { x = Coordinate.x, y = Coordinate.y + 1 };
            var bottom = new Coordinate { x = Coordinate.x, y = Coordinate.y - 1 };

            Left = cardCoordinates.ContainsKey(left) ? cardCoordinates[left].Right : null;
            Right = cardCoordinates.ContainsKey(right) ? cardCoordinates[right].Left : null;
            Top = cardCoordinates.ContainsKey(top) ? cardCoordinates[top].Bottom : null;
            Bottom = cardCoordinates.ContainsKey(bottom) ? cardCoordinates[bottom].Top : null;
        }
    }
}
