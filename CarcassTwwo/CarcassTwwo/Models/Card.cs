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

        public void SetSideOccupation(Card card, Board board)
        {
            Coordinate newCoord = new Coordinate { x = card.Coordinate.x, y = card.Coordinate.y + 1 };
            if (!board.CardCoordinates.ContainsKey(newCoord))
            {
                card.TopIsFree = true;
            } else
            {
                card.TopIsFree = false;
                board.CardCoordinates[newCoord].BottomIsFree = false;
            }
                        
            newCoord.y = card.Coordinate.y - 1;
            if (!board.CardCoordinates.ContainsKey(newCoord))
            {
                card.BottomIsFree = true;
            }
            else
            {
                card.BottomIsFree = false;
                board.CardCoordinates[newCoord].TopIsFree = false;
            }
            
            newCoord.x = card.Coordinate.x + 1;
            newCoord.y = card.Coordinate.y;
            if (!board.CardCoordinates.ContainsKey(newCoord))
            {
                card.RightIsFree = true;
            }
            else
            {
                card.RightIsFree = false;
                board.CardCoordinates[newCoord].LeftIsFree = false;
            }
            
            newCoord.x = card.Coordinate.x - 1;
            if (!board.CardCoordinates.ContainsKey(newCoord))
            {
                card.LeftIsFree = true;
            }
            else
            {
                card.LeftIsFree = false;
                board.CardCoordinates[newCoord].RightIsFree = false;
            }
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