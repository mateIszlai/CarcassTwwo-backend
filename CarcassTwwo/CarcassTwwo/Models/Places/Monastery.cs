using System.Collections.Generic;

namespace CarcassTwwo.Models.Places
{
    public class Monastery : Place
    {
        public List<Coordinate> SurroundingCoordinates { get; private set; }
        public int CardId { get; set; }
        public bool IsFinished { get { return SurroundingCoordinates.Count == 0; } }
        public Monastery(int id, Coordinate cardCoordinate) : base(id)
        {
            SurroundingCoordinates = new List<Coordinate>
            {
                // topleft
                new Coordinate{x = cardCoordinate.x - 1, y = cardCoordinate.y + 1},
                //topmid
                new Coordinate{x = cardCoordinate.x, y = cardCoordinate.y + 1},
                // topright
                new Coordinate{x = cardCoordinate.x + 1, y = cardCoordinate.y + 1},
                // midleft
                new Coordinate{x = cardCoordinate.x - 1, y = cardCoordinate.y},
                //midright
                new Coordinate{x = cardCoordinate.x + 1, y = cardCoordinate.y},
                // botleft
                new Coordinate{x = cardCoordinate.x - 1, y = cardCoordinate.y - 1},
                //botmid
                new Coordinate{x = cardCoordinate.x, y = cardCoordinate.y - 1},
                //botright
                new Coordinate{x = cardCoordinate.x + 1, y = cardCoordinate.y - 1},
            };
        }

        public void ExpandMonastery(Coordinate coordinate)
        {
            SurroundingCoordinates.Remove(coordinate);
        }

        public override void PlaceMeeple(Client owner, int field, Card card)
        {
            if (!card.HasMeeple)
            {
                var monk = new Meeple(MeepleType.MONK, owner, field, card.Coordinate, Id);
                Meeples.Add(monk);
                card.HasMeeple = true;
                owner.MeepleCount--;
            }
        }
    }
}
