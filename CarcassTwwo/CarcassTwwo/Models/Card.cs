using System.Collections.Generic;

namespace CarcassTwwo.Models
{
    public class Card
    {
        public int Id { get; private set; }
        public Tile Tile { get; set; }
        public Meeple meeple { get; set; }
        public bool HasMeeple { get; set; }
        public Coordinate Coordinate { get; set; }

        public LandType Top { get; set; }
        public LandType Bottom { get; set; }
        public LandType Left { get; set; }
        public LandType Right { get; set; }

        public Dictionary<string,List<LandType>> Rotations { get; set; }

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
            Rotations = new Dictionary<string, List<LandType>>();
            SetRotations();
        }

        public Card()
        {

        }
        

        public void SetRotations()
        {
            Rotations["0"] = new List<LandType> { Top, Left, Bottom, Right };
            Rotations["90"] = new List<LandType> { Right, Top, Left, Bottom };
            Rotations["180"] = new List<LandType> { Bottom, Right, Top, Left };
            Rotations["270"] = new List<LandType> { Left, Bottom, Right, Top };
        }

        public void PlaceMeeple(Field field)
        {
            meeple = new Meeple(field.Coordinate, field.LandType.Meeple);
            HasMeeple = true;
        }

        public void RemoveMeeple()
        {
            meeple = null;
            HasMeeple = false;
        }
    }
}