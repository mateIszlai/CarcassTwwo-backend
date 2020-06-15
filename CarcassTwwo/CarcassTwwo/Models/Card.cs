using CarcassTwwo.Models.Places;
using System;
using System.Collections.Generic;

namespace CarcassTwwo.Models
{
    public class Card
    {
        public int Id { get; private set; }
        public Tile Tile { get; set; }
        public Meeple Meeple { get; set; }
        public bool HasMeeple { get; set; }
        public LandType MeepleField { get; set; }
        public Coordinate Coordinate { get; set; }
        public LandType Top
        {
            get
            {
                return Tile.Field2;
            }
            set
            {
                Tile.Field2 = value;
            }
        }
        public LandType Bottom
        {
            get
            {
                return Tile.Field8;
            }
            set
            {
                Tile.Field8 = value;
            }
        }

        public LandType Left
        {
            get
            {
                return Tile.Field4;
            }
            set
            {
                Tile.Field4 = value;
            }
        }
        public LandType Right
        {
            get
            {
                return Tile.Field6;
            }
            set
            {
                Tile.Field6 = value;
            }
        }

        public HashSet<int> PlaceIds
        { 
            get
            {
                var ids = new HashSet<int>();
                foreach (var landType in Tile.Sides.Values)
                    ids.Add(landType.PlaceId);
                return ids;
            } 
        }

        public int MonasteryId { get; set; }



        public List<LandType> Sides { get { return new List<LandType> { Top, Bottom, Left, Right }; } }

        public Dictionary<string, LandType[]> Rotations { get; private set; }

        public bool TopIsFree { get; set; }
        public bool BottomIsFree { get; set; }
        public bool LeftIsFree { get; set; }
        public bool RightIsFree { get; set; }
        public bool HasCrest { get; set; }

        public Card(Tile tile, int id)
        {
            Id = id;
            Tile = tile;
            HasCrest = Tile.HasCrest;
            Rotations = new Dictionary<string, LandType[]>();
            SetRotations();
            MonasteryId = -1;
        }

        public Card()
        {

        }
        

        public void SetRotations()
        {
            Rotations["0"] = new LandType[] { Tile.Field1, Tile.Field2, Tile.Field3, Tile.Field4, Tile.Field5, Tile.Field6, Tile.Field7, Tile.Field8, Tile.Field9 };
            Rotations["90"] = new LandType[] { Tile.Field3, Tile.Field6, Tile.Field9, Tile.Field2, Tile.Field5, Tile.Field8, Tile.Field1, Tile.Field4, Tile.Field7 };
            Rotations["180"] = new LandType[] { Tile.Field9, Tile.Field8, Tile.Field7, Tile.Field6, Tile.Field5, Tile.Field4, Tile.Field3, Tile.Field2, Tile.Field1 };
            Rotations["270"] = new LandType[] { Tile.Field7, Tile.Field4, Tile.Field1, Tile.Field8, Tile.Field5, Tile.Field2, Tile.Field9, Tile.Field6, Tile.Field3 };
        }

        public void AddMeeple(Meeple meeple, int field)
        {
            Meeple = meeple;
            HasMeeple = true;
            SetMeepleField(field);
        }

        public void RemoveMeeple()
        {
            Meeple = null;
            HasMeeple = false;
            MeepleField = null;
        }

        public void SetMeepleField(int field)
        {
            MeepleField = Tile.Fields[field - 1];
        }

        public void Rotate(string rotation)
        {
            Tile.Field1 = Rotations[rotation][0];
            Tile.Field2 = Rotations[rotation][1];
            Tile.Field3 = Rotations[rotation][2];
            Tile.Field4 = Rotations[rotation][3];
            Tile.Field5 = Rotations[rotation][4];
            Tile.Field6 = Rotations[rotation][5];
            Tile.Field7 = Rotations[rotation][6];
            Tile.Field8 = Rotations[rotation][7];
            Tile.Field9 = Rotations[rotation][8];
        }

        internal void SetField(Side side, int placeId)
        {
            switch (side)
            {
                case Side.TOPLEFT:
                    Tile.Field1.PlaceId = placeId;
                    break;
                case Side.TOP:
                    Tile.Field2.PlaceId = placeId;
                    break;
                case Side.TOPRIGHT:
                    Tile.Field3.PlaceId = placeId;
                    break;
                case Side.MIDDLELEFT:
                    Tile.Field4.PlaceId = placeId;
                    break;
                case Side.MIDDLE:
                    Tile.Field5.PlaceId = placeId;
                    break;
                case Side.MIDDLERIGHT:
                    Tile.Field6.PlaceId = placeId;
                    break;
                case Side.BOTTOMLEFT:
                    Tile.Field7.PlaceId = placeId;
                    break;
                case Side.BOTTOM:
                    Tile.Field8.PlaceId = placeId;
                    break;
                case Side.BOTTOMRIGHT:
                    Tile.Field9.PlaceId = placeId;
                    break;
            }
        }
    }
}
