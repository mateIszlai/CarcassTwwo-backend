namespace CarcassTwwo.Models
{
    public class Meeple
    {
        public MeepleType Type { get; private set; }
        public Client Owner { get; private set; }
        public int FieldId { get; private set; }
        public Coordinate Coordinate { get; private set; }
        public int PlaceId { get; private set; }
  
        public Meeple( MeepleType type, Client owner, int fieldId, Coordinate coordinate, int placeId)
        {
            Type = type;
            Owner = owner;
            FieldId = fieldId;
            PlaceId = placeId;
            Coordinate = coordinate;
        }
    }
}
