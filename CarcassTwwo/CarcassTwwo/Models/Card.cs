using System.Collections.Generic;

namespace CarcassTwwo.Models
{
    public class Card
    {
        public int Id { get; private set; }
        public Tile Tile { get; set; }
        public Coordinate MeepleField { get; set; }
        public string MeepleType { get; set; }
        public bool HasMeeple { get; set; }
        public Coordinate Coordinate { get; set; }

        public LandType Top { get; set; }
        public LandType Bottom { get; set; }
        public LandType Left { get; set; }
        public LandType Right { get; set; }

        public Dictionary<int,List<LandType>> Rotations { get; set; }

        public bool TopIsFree { get; set; }
        public bool BottomIsFree { get; set; }
        public bool LeftIsFree { get; set; }
        public bool RightIsFree { get; set; }
        public bool HasCrest { get; set; }

        public Card(Tile tile, int id)
        {
            Id = id;
            Tile = tile;
            Top = Tile.Field2.LandType;
            Bottom = Tile.Field8.LandType;
            Left = Tile.Field4.LandType;
            Right = Tile.Field6.LandType;
            HasCrest = Tile.HasCrest;
            Rotations = new Dictionary<int, List<LandType>>();
            SetRotations();
        }

        public void SetSideOccupation(Board board)
        {
            var vertical = new Coordinate { x = Coordinate.x, y = Coordinate.y + 1 };

            TopIsFree = board.CardCoordinates.ContainsKey(vertical) ? false : true;
            if (!TopIsFree)
                board.CardCoordinates[vertical].BottomIsFree = false;
                        
            vertical.y = Coordinate.y - 1;
            BottomIsFree = board.CardCoordinates.ContainsKey(vertical) ? false : true;
            if(!BottomIsFree)
                board.CardCoordinates[vertical].TopIsFree = false;

            var horizontal = new Coordinate { x = Coordinate.x + 1, y = Coordinate.y };
            RightIsFree = board.CardCoordinates.ContainsKey(horizontal) ? false : true;
            if(!RightIsFree)
                board.CardCoordinates[horizontal].LeftIsFree = false;
            
            horizontal.x = Coordinate.x - 1;
            LeftIsFree = board.CardCoordinates.ContainsKey(horizontal) ? false : true;
            if(!LeftIsFree)
                board.CardCoordinates[horizontal].RightIsFree = false;
        }

        public void SetRotations()
        {
            Rotations[0] = new List<LandType> { Top, Left, Bottom, Right };
            Rotations[90] = new List<LandType> { Right, Top, Left, Bottom };
            Rotations[180] = new List<LandType> { Bottom, Right, Top, Left };
            Rotations[270] = new List<LandType> { Left, Bottom, Right, Top };
        }

        public void PlaceMeeple(Field field)
        {
            MeepleField = field.Coordinate;
            MeepleType = field.LandType.Meeple;
            HasMeeple = true;
        }

        public void RemoveMeeple()
        {
            HasMeeple = false;
        }
    }
}