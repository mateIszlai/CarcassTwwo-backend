﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models.Places
{
    public class GrassLand : Place
    {

        //Grasslands are counted only at the end of the game.
        public HashSet<int> SurroundingCities { get; private set; }

        public HashSet<int> Roads { get; private set; }
        public int Size { get { return CardIds.Count; }}

        public HashSet<int> CardIds { get; private set; }
        //only the finished cities count
        public GrassLand(int id, int cardId) : base(id)
        {
            Roads = new HashSet<int>();
            SurroundingCities = new HashSet<int>();
            CardIds = new HashSet<int> { cardId };
        }

        public GrassLand(int id) : base(id)
        {
            Roads = new HashSet<int>();
            SurroundingCities = new HashSet<int>();
            CardIds = new HashSet<int>();
        }

        public void ExpandLand (int cardId)
        {
            CardIds.Add(cardId);
        }

        public override void PlaceMeeple(Client owner)
        {
            Meeples.Add(new Meeple(MeepleType.PEASANT, owner));
        }
    }
}
