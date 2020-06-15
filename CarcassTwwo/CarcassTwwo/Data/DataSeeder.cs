﻿using CarcassTwwo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo
{
    public class DataSeeder
    {
        public static List<LandType> landTypes;
        public static List<Tile> tiles;
        public static void SeedLandTypes()
        {
            if (landTypes == null)
            {
                landTypes = new List<LandType>
                {
                    new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                    new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                    new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    new LandType{ Name = "Monastery", Meeple = MeepleType.MONK },
                    new LandType{Name = "Other"}
                };
            }
        }

        public static void SeedTiles()
        {
            LandType land = landTypes[0];
            LandType city = landTypes[1];
            LandType road = landTypes[2];
            LandType monastery = landTypes[3];
            LandType other = landTypes[4];

            if (tiles == null)
            {
                tiles = new List<Tile>
                {
                //1
                new Tile { Field1 =  new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field2 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field3 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field4 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field5 = new LandType{ Name = "Monastery", Meeple = MeepleType.MONK },
                            Field6 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                           /* Top1 = land,
                            Top2 = land,
                            Top3 = land,
                            Left1 = land,
                            Left2 = land,
                            Left3 = land,
                            Bottom1 = land,
                            Bottom2 = land,
                            Bottom3 = land,
                            Right1 = land,
                            Right2 = land,
                            Right3 = land,
                           */
                            Amount = 4,
                            Remaining = 4,
                            Image = "../wwwroot/image/1_4.png",
                            HasCrest = false},
                //2
                new Tile { Field1 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field2 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field3 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field4 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field5 = new LandType{ Name = "Monastery", Meeple = MeepleType.MONK },
                            Field6 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                           /* Top1 = land,
                            Top2 = land,
                            Top3 = land,
                            Left1 = land,
                            Left2 = land,
                            Left3 = land,
                            Bottom1 = land,
                            Bottom2 = road,
                            Bottom3 = land,
                            Right1 = land,
                            Right2 = land,
                            Right3 = land,*/
                            Amount = 2,
                            Remaining = 2,
                            Image = "../wwwroot/image/2_2.png",
                            HasCrest = false},
                //3
                new Tile { Field1 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field2 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field3 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field4 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field5 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field6 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field7 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field8 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field9 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            
                            Amount = 1,
                            Remaining = 1,
                            Image = "../wwwroot/image/3_1.png",
                            HasCrest = true},
                 //4
                new Tile { Field1 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field2 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field3 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field4 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field5 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field6 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            
                            Amount = 3,
                            Remaining = 3,
                            Image = "../wwwroot/image/4_3.png",
                            HasCrest = false},
                //5
                new Tile { Field1 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field2 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field3 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field4 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field5 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field6 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            
                            Amount = 1,
                            Remaining = 1,
                            Image = "../wwwroot/image/5_1.png",
                            HasCrest = true},
                //6
                new Tile { Field1 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field2 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field3 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field4 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field5 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field6 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            
                            Amount = 1,
                            Remaining = 1,
                            Image = "../wwwroot/image/6_1.png",
                            HasCrest = false},
                //7
                new Tile { Field1 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field2 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field3 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field4 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field5 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field6 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            
                            Amount = 2,
                            Remaining = 2,
                            Image = "../wwwroot/image/7_2.png",
                            HasCrest = true},
                //8
                new Tile { Field1 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field2 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field3 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field4 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field5 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field6 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            
                            Amount = 3,
                            Remaining = 3,
                            Image = "../wwwroot/image/8_3.png",
                            HasCrest = false},
                //9
                new Tile { Field1 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field2 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field3 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field4 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field5 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field6 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            
                            Amount = 2,
                            Remaining = 2,
                            Image = "../wwwroot/image/9_2.png",
                            HasCrest = true},
                //10
                new Tile { Field1 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field2 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field3 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field4 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field5 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field6 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            
                            Amount = 3,
                            Remaining = 3,
                            Image = "../wwwroot/image/10_3.png",
                            HasCrest = false},
                //11
                new Tile { Field1 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field2 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field3 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field4 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field5 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field6 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                           
                            Amount = 2,
                            Remaining = 2,
                            Image = "../wwwroot/image/11_2.png",
                            HasCrest = true},

                //12
                new Tile { Field1 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field2 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field3 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field4 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field5 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field6 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            
                            Amount = 1,
                            Remaining = 1,
                            Image = "../wwwroot/image/12_1.png",
                            HasCrest = false},
                //13
                new Tile { Field1 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field2 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field3 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field4 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field5 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field6 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            
                            Amount = 2,
                            Remaining = 2,
                            Image = "../wwwroot/image/13_2.png",
                            HasCrest = true},
                //14
                new Tile { Field1 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field2 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field3 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field4 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field5 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field6 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                           
                            Amount = 2,
                            Remaining = 2,
                            Image = "../wwwroot/image/14_2.png",
                            HasCrest = false},
                //15
                new Tile { Field1 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field2 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field3 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field4 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field5 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field6 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            
                            Amount = 3,
                            Remaining = 3,
                            Image = "../wwwroot/image/15_3.png",
                            HasCrest = false},
                //16
                new Tile { Field1 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field2 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field3 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field4 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field5 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field6 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            
                            Amount = 5,
                            Remaining = 5,
                            Image = "../wwwroot/image/16_5.png",
                            HasCrest = false},
                
                //17
                new Tile { Field1 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field2 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field3 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field4 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field5 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field6 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            
                            Amount = 3,
                            Remaining = 3,
                            Image = "../wwwroot/image/17_3.png",
                            HasCrest = false},
                //18
                new Tile { Field1 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field2 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field3 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field4 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field5 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field6 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            
                            Amount = 3,
                            Remaining = 3,
                            Image = "../wwwroot/image/18_3.png",
                            HasCrest = false},
                //19
                new Tile { Field1 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field2 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field3 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field4 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field5 = new LandType{Name = "Other"},
                            Field6 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            
                            Amount = 3,
                            Remaining = 3,
                            Image = "../wwwroot/image/19_3.png",
                            HasCrest = false},
                //20 - first  card
                new Tile { Field1 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field2 = new LandType{ Name = "City", Meeple = MeepleType.KNIGHT },
                            Field3 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field4 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field5 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field6 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            
                            Amount = 4,
                            Remaining = 4,
                            Image = "../wwwroot/image/20_4.png",
                            HasCrest = false},
                //21
                new Tile { Field1 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field2 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field3 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field4 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field5 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field6 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            
                            Amount = 8,
                            Remaining = 8,
                            Image = "../wwwroot/image/21_8.png",
                            HasCrest = false},
                //22
                new Tile { Field1 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field2 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field3 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field4 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field5 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field6 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            
                            Amount = 9,
                            Remaining = 9,
                            Image = "../wwwroot/image/22_9.png",
                            HasCrest = false},
                //23
                new Tile { Field1 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field2 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field3 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field4 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field5 = new LandType{Name = "Other"},
                            Field6 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            
                            Amount = 4,
                            Remaining = 4,
                            Image = "../wwwroot/image/23_4.png",
                            HasCrest = false},
                //24
                new Tile { Field1 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field2 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field3 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field4 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field5 = new LandType{Name = "Other"},
                            Field6 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            Field8 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                            Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                            
                            Amount = 1,
                            Remaining = 1,
                            Image = "../wwwroot/image/24_1.png",
                            HasCrest = false},
                };

                for(int i = 1; i <= tiles.Count; i++)
                {
                    tiles[i - 1].Id = i;
                }
            }
        }

    }
}
