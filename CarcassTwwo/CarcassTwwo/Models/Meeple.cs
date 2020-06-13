using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models
{
    public class Meeple
    {
        public MeepleType Type { get; set; }
        public Client Owner { get; set; }
        public int FieldId { get; set; }
        public Card Card { get; set; }
        public int PlaceId { get; set; }
  
        public Meeple( MeepleType type, Client owner, int fieldId, Card card, int placeId)
        {
            Type = type;
            Owner = owner;
            FieldId = fieldId;
            Card = card;
            PlaceId = placeId;
        }
    }
}
