using System.Collections.Generic;
using System.Linq;

namespace CarcassTwwo.Models.Places
{
    public class City : Place
    {
        private HashSet<CityPart> _cityParts;
        public int Size { get { return _cityParts.Count; } }
        

        public bool IsOpen {
            get 
            { 
                return _cityParts.Any(c => c.BottomIsOpen || c.LeftIsOpen || c.RightIsOpen || c.TopIsOpen ); 
            } 
        }

        public City(int id, int cardId) : base(id)
        {
            _cityParts = new HashSet<CityPart> { new CityPart(cardId)};
        }

        public void ExpandCity(CityPart newPart)
        {
            _cityParts.Add(newPart);
        }

        public CityPart GetCityPartByCardId(int cardId)
        {
            return _cityParts.FirstOrDefault(c => c.CardId == cardId);
        }

        public void SetSides(int cardId, Side side)
        {
            var part = _cityParts.FirstOrDefault(c => c.CardId == cardId);
            if (part != null)
            {
                switch (side)
                {
                    case Side.TOP:
                        part.TopIsOpen = false;
                        break;
                    case Side.BOTTOM:
                        part.BottomIsOpen = false;
                        break;
                    case Side.LEFT:
                        part.LeftIsOpen = false;
                        break;
                    case Side.RIGHT:
                        part.RightIsOpen = false;
                        break;
                }
            }
            
        }

        public override void PlaceMeeple(Client owner)
        {
            Meeples.Add(new Meeple(MeepleType.KNIGHT, owner));
        }
    }

    public enum Side
    {
        TOP,
        LEFT,
        BOTTOM,
        RIGHT
    }
}
