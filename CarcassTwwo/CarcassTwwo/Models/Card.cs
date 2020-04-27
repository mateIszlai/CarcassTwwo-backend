namespace CarcassTwwo.Models
{
    public class Card
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public Carcassonne Game { get; set; }
        public Tile Tile { get; set; }
        public int MeepleField { get; set; }
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
            Top = Tile.Field2;
            Bottom = Tile.Field8;
            Left = Tile.Field4;
            Right = Tile.Field6;
        }

        public void SetSideOccupation()
        {
            //TODO: this will change the occupation of a side from free to taken
        }
    }
}