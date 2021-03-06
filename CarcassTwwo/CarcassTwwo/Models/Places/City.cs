﻿using System.Collections.Generic;
using System.Linq;

namespace CarcassTwwo.Models.Places
{
    public class City : Place
    {
        public HashSet<CityPart> CityParts { get; private set; }
        public int Size { get { return CityParts.Count; } }

        public bool IsOpen {
            get 
            { 
                return CityParts.Any(c => c.BottomIsOpen || c.LeftIsOpen || c.RightIsOpen || c.TopIsOpen ); 
            } 
        }

        public City(int id) : base(id)
        {
            CityParts = new HashSet<CityPart>();
        }

        public void ExpandCity(CityPart newPart)
        {
            CityParts.Add(newPart);
        }

        public CityPart GetCityPartByCardId(int cardId)
        {
            return CityParts.FirstOrDefault(c => c.CardId == cardId);
        }

        public void SetSides(int cardId, Side side)
        {
            var part = CityParts.FirstOrDefault(c => c.CardId == cardId);
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
                    case Side.MIDDLELEFT:
                        part.LeftIsOpen = false;
                        break;
                    case Side.MIDDLERIGHT:
                        part.RightIsOpen = false;
                        break;
                }
            }     
        }

        public override void PlaceMeeple(Client owner, int field, Card card)
        {
            if (!card.HasMeeple)
            {
                var knight = new Meeple(MeepleType.KNIGHT, owner, field, card.Coordinate, Id);
                Meeples.Add(knight);
                card.HasMeeple = true;
                owner.MeepleCount--;
            }
        }
    }
}
