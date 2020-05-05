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
        }

        public void SetSideOccupation(Board board)
        {
            var vertical = new Coordinate { x = Coordinate.x, y = Coordinate.y + 1 };

            TopIsFree = board.CardCoordinates.ContainsKey(vertical) ? false : true;
            board.CardCoordinates[vertical].BottomIsFree = TopIsFree;
                        
            vertical.y = Coordinate.y - 1;
            BottomIsFree = board.CardCoordinates.ContainsKey(vertical) ? false : true;
            board.CardCoordinates[vertical].TopIsFree = BottomIsFree;

            var horizontal = new Coordinate { x = Coordinate.x + 1, y = Coordinate.y };
            RightIsFree = board.CardCoordinates.ContainsKey(horizontal) ? false : true;
            board.CardCoordinates[horizontal].LeftIsFree = RightIsFree;
            
            horizontal.x = Coordinate.x - 1;
            LeftIsFree = board.CardCoordinates.ContainsKey(horizontal) ? false : true;
            board.CardCoordinates[horizontal].RightIsFree = LeftIsFree;
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