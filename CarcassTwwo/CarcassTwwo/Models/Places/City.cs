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

        public void SetSides(List<CityPart> cityParts)
        {
            foreach (var cityPart in cityParts)
            {
                var part = _cityParts.FirstOrDefault(c => c.CardId == cityPart.CardId);
                if ( part != null)
                {
                    part.BottomIsOpen = cityPart.BottomIsOpen;
                    part.LeftIsOpen = cityPart.LeftIsOpen;
                    part.RightIsOpen = cityPart.RightIsOpen;
                    part.TopIsOpen = cityPart.TopIsOpen;
                }
            }
        }

        public override void PlaceMeeple(Client owner)
        {
            Meeples.Add(new Meeple(MeepleType.KNIGHT, owner));
        }
    }
}
