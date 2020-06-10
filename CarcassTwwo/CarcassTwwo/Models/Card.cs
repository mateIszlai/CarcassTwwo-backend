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
        public Field MeepleField { get; set; }
        public Coordinate Coordinate { get; set; }
        public LandType Top
        {
            get
            {
                return Tile.Field2.LandType;
            }
            set
            {
                Tile.Field2.LandType = value;
            }
        }
        public LandType Bottom
        {
            get
            {
                return Tile.Field8.LandType;
            }
            set
            {
                Tile.Field8.LandType = value;
            }
        }

        public LandType Left
        {
            get
            {
                return Tile.Field4.LandType;
            }
            set
            {
                Tile.Field4.LandType = value;
            }
        }
        public LandType Right
        {
            get
            {
                return Tile.Field6.LandType;
            }
            set
            {
                Tile.Field6.LandType = value;
            }
        }

        public HashSet<int> PlaceIds
        { 
            get
            {
                var ids = new HashSet<int>();
                Sides.ForEach(s => ids.Add(s.PlaceId));
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
            Rotations["0"] = new LandType[] { Tile.Field1.LandType, Tile.Field2.LandType, Tile.Field3.LandType, Tile.Field4.LandType, Tile.Field6.LandType, Tile.Field7.LandType, Tile.Field8.LandType, Tile.Field9.LandType };
            Rotations["90"] = new LandType[] { Tile.Field3.LandType, Tile.Field6.LandType, Tile.Field9.LandType, Tile.Field2.LandType, Tile.Field8.LandType, Tile.Field1.LandType, Tile.Field4.LandType, Tile.Field7.LandType };
            Rotations["180"] = new LandType[] { Tile.Field9.LandType, Tile.Field8.LandType, Tile.Field7.LandType, Tile.Field6.LandType, Tile.Field4.LandType, Tile.Field3.LandType, Tile.Field2.LandType, Tile.Field1.LandType };
            Rotations["270"] = new LandType[] { Tile.Field7.LandType, Tile.Field4.LandType, Tile.Field1.LandType, Tile.Field8.LandType, Tile.Field2.LandType, Tile.Field9.LandType, Tile.Field6.LandType, Tile.Field3.LandType };
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
            Tile.Field1.LandType = Rotations[rotation][0];
            Tile.Field2.LandType = Rotations[rotation][1];
            Tile.Field3.LandType = Rotations[rotation][2];
            Tile.Field4.LandType = Rotations[rotation][3];
            Tile.Field6.LandType = Rotations[rotation][4];
            Tile.Field7.LandType = Rotations[rotation][5];
            Tile.Field8.LandType = Rotations[rotation][6];
            Tile.Field9.LandType = Rotations[rotation][7];
        }

        internal void SetField(Side side, int placeId)
        {
            switch (side)
            {
                case Side.TOPLEFT:
                    Tile.Field1.LandType.PlaceId = placeId;
                    break;
                case Side.TOP:
                    Tile.Field2.LandType.PlaceId = placeId;
                    break;
                case Side.TOPRIGHT:
                    Tile.Field3.LandType.PlaceId = placeId;
                    break;
                case Side.MIDDLELEFT:
                    Tile.Field4.LandType.PlaceId = placeId;
                    break;
                case Side.MIDDLE:
                    Tile.Field5.LandType.PlaceId = placeId;
                    break;
                case Side.MIDDLERIGHT:
                    Tile.Field6.LandType.PlaceId = placeId;
                    break;
                case Side.BOTTOMLEFT:
                    Tile.Field7.LandType.PlaceId = placeId;
                    break;
                case Side.BOTTOM:
                    Tile.Field8.LandType.PlaceId = placeId;
                    break;
                case Side.BOTTOMRIGHT:
                    Tile.Field9.LandType.PlaceId = placeId;
                    break;
            }
        }
    }
}
