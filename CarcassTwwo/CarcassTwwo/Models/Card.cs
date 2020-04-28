namespace CarcassTwwo.Models
{
    public class Card
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public Tile Tile { get; set; }
        public Coordinate MeepleField { get; set; }
        public string MeepleType { get; set; }
        public bool HasMeeple { get; set; }
        public Coordinate Coordinate { get; set; }

        public LandType Top { get; set; }
        public LandType Bottom { get; set; }
        public LandType Left { get; set; }
        public LandType Right { get; set; }

        public bool TopIsFree { get; set; }
        public bool BottomIsFree { get; set; }
        public bool LeftIsFree { get; set; }
        public bool RightIsFree { get; set; }
        public bool HasCrest { get; set; }

        public Card(Tile tile)
        {
            Tile = tile;
            Top = Tile.Field2.LandType;
            Bottom = Tile.Field8.LandType;
            Left = Tile.Field4.LandType;
            Right = Tile.Field6.LandType;
            HasCrest = Tile.HasCrest;
        }

        public void SetSideOccupation(bool top, bool bottom, bool left, bool right)
        {
            TopIsFree = top;
            BottomIsFree = bottom;
            LeftIsFree = left;
            RightIsFree = right;

            //TODO: this will change the occupation of a side from free to taken
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