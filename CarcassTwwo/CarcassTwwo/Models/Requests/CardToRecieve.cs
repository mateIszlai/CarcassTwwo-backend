namespace CarcassTwwo.Models.Requests
{
    public class CardToRecieve
    {
        public int CardId { get; set; }
        public string Rotation { get; set; }
        public Coordinate Coordinate { get; set; }
        public int TileId { get; set; }
    }
}
