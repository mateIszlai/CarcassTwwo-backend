using CarcassTwwo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo
{
    public class DataSeeder
    {
        private static List<Card> _cards;
        private static  int id = 0;

        public static List<Card> SeedCards()
        {
            if (_cards != null)
                return new List<Card>(_cards);

            _cards = new List<Card>();


            //1
            for (int i = 0; i < 4; i++)
            {
                id++;
                _cards.Add(new Card(
                    new Tile
                    {
                        Id = 1,
                        Field1 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field2 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field3 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field4 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field5 = new LandType { Name = "Monastery", Meeple = MeepleType.MONK },
                        Field6 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field8 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Image = "../wwwroot/image/1_4.png",
                        HasCrest = false
                    }, id)) ;
            }

            //2
            for (int i = 0; i < 2; i++)
            {
                id++;
                _cards.Add(new Card(
                    new Tile
                    {
                        Id = 2,
                        Field1 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field2 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field3 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field4 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field5 = new LandType { Name = "Monastery", Meeple = MeepleType.MONK },
                        Field6 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field8 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                        Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Image = "../wwwroot/image/2_2.png",
                        HasCrest = false
                    }, id));
            }

            //3
            id++;
            _cards.Add(new Card(
                new Tile
                {
                    Id = 3,
                    Field1 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field2 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field3 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field4 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field5 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field6 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field7 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field8 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field9 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Image = "../wwwroot/image/3_1.png",
                    HasCrest = true
                }, id));


            //4
            for (int i = 0; i < 3; i++)
            {
                id++;
                _cards.Add(new Card(
                    new Tile
                    {
                        Id = 4,
                        Field1 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                        Field2 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                        Field3 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                        Field4 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                        Field5 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                        Field6 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                        Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field8 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Image = "../wwwroot/image/4_3.png",
                        HasCrest = false
                    }, id));
            }

            //5
            id++;
            _cards.Add(new Card(
                 new Tile
                 {
                     Id = 5,
                     Field1 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                     Field2 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                     Field3 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                     Field4 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                     Field5 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                     Field6 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                     Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                     Field8 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                     Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                     Image = "../wwwroot/image/5_1.png",
                     HasCrest = true
                 }, id));

            //6
            id++;
            _cards.Add(new Card(
                 new Tile
                 { 
                     Id = 6,
                     Field1 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                     Field2 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                     Field3 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                     Field4 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                     Field5 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                     Field6 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                     Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                     Field8 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                     Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                     Image = "../wwwroot/image/6_1.png",
                     HasCrest = false
                 }, id));

            //7
            for (int i = 0; i < 2; i++)
            {
                id++;
                _cards.Add(new Card(
                    new Tile
                    {
                        Id = 7,
                        Field1 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                        Field2 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                        Field3 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                        Field4 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                        Field5 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                        Field6 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                        Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field8 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                        Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Image = "../wwwroot/image/7_2.png",
                        HasCrest = true
                    }, id));
            }

            //8
            for (int i = 0; i < 3; i++)
            {
                id++;
                _cards.Add(new Card(
                     new Tile
                     {
                         Id = 8,
                         Field1 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                         Field2 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                         Field3 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                         Field4 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                         Field5 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                         Field6 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                         Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                         Field8 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                         Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                         Image = "../wwwroot/image/8_3.png",
                         HasCrest = false
                     }, id));
            }

            //9
            for (int i = 0; i < 2; i++)
            {
                id++;
                _cards.Add(new Card(
                    new Tile
                    {
                        Id = 9,
                        Field1 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                        Field2 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                        Field3 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field4 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                        Field5 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                        Field6 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field8 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Image = "../wwwroot/image/9_2.png",
                        HasCrest = true
                    }, id));
            }

            //10
            for (int i = 0; i < 3; i++)
            {
                id++;
                _cards.Add(new Card(
                     new Tile
                     {
                         Id = 10,
                         Field1 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                         Field2 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                         Field3 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                         Field4 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                         Field5 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                         Field6 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                         Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                         Field8 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                         Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                         Image = "../wwwroot/image/10_3.png",
                         HasCrest = false
                     }, id));
            }

            //11
            for (int i = 0; i < 2; i++)
            {
                id++;
                _cards.Add(new Card(
                new Tile
                {
                    Id = 11,
                    Field1 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field2 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field3 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field4 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field5 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field6 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field8 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Image = "../wwwroot/image/11_2.png",
                    HasCrest = true
                }, id));
            }

            //12
            id++;
            _cards.Add(new Card(
                new Tile 
                {
                    Id = 12,
                    Field1 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field2 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field3 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field4 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field5 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field6 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field8 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Image = "../wwwroot/image/12_1.png",
                    HasCrest = false
                }, id));


            //13
            for (int i = 0; i < 2; i++)
            {
                id++;
                _cards.Add(new Card(
                new Tile
                {
                    Id = 13,
                    Field1 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field2 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field3 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field4 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field5 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field6 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field8 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Image = "../wwwroot/image/13_2.png",
                    HasCrest = true
                }, id));
            }

            //14
            for (int i = 0; i < 2; i++)
            {
                id++;
                _cards.Add(new Card(
                new Tile
                {
                    Id = 14,
                    Field1 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field2 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field3 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field4 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field5 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field6 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field8 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Image = "../wwwroot/image/14_2.png",
                    HasCrest = false
                }, id));
            }

            //15
            for (int i = 0; i < 3; i++)
            {
                id++;
                _cards.Add(new Card(
                new Tile
                {
                    Id = 15,
                    Field1 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field2 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field3 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field4 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field5 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field6 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field8 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Image = "../wwwroot/image/15_3.png",
                    HasCrest = false
                }, id));
            }

            //16
            for (int i = 0; i < 5; i++)
            {
                id++;
                _cards.Add(new Card(
                new Tile
                {
                    Id = 16,
                    Field1 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field2 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field3 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field4 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field5 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field6 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field8 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Image = "../wwwroot/image/16_5.png",
                    HasCrest = false
                }, id));
            }

            //17
            for (int i = 0; i < 3; i++)
            {
                id++;
                _cards.Add(new Card(
                new Tile
                {
                    Id = 17,
                    Field1 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field2 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field3 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field4 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field5 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field6 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field8 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Image = "../wwwroot/image/17_3.png",
                    HasCrest = false
                }, id));

            }

            //18
            for (int i = 0; i < 3; i++)
            {
                id++;
                _cards.Add(new Card(
                    new Tile
                    {
                        Id = 18,
                        Field1 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field2 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                        Field3 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field4 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field5 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field6 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                        Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Field8 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                        Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                        Image = "../wwwroot/image/18_3.png",
                        HasCrest = false
                    }, id));

            }

            //19
            for (int i = 0; i < 3; i++)
            {
                id++;
                _cards.Add(new Card(
                new Tile
                {
                    Id = 19,
                    Field1 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field2 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field3 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field4 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field5 = new LandType { Name = "Other" },
                    Field6 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field8 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Image = "../wwwroot/image/19_3.png",
                    HasCrest = false
                }, id));
            }

            //20 - first  card
            for (int i = 0; i < 4; i++)
            {
                id++;
                _cards.Add(new Card(
                new Tile
                {
                    Id = 20,
                    Field1 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field2 = new LandType { Name = "City", Meeple = MeepleType.KNIGHT },
                    Field3 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field4 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field5 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field6 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field8 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Image = "../wwwroot/image/20_4.png",
                    HasCrest = false
                }, id));
            }

            //21
            for (int i = 0; i < 8; i++)
            {
                id++;
                _cards.Add(new Card(
                new Tile
                {
                    Id = 21,
                    Field1 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field2 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field3 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field4 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field5 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field6 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field8 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Image = "../wwwroot/image/21_8.png",
                    HasCrest = false
                }, id));
            }

            //22
            for (int i = 0; i < 9; i++)
            {
                id++;
                _cards.Add(new Card(
                new Tile
                {
                    Id = 22,
                    Field1 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field2 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field3 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field4 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field5 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field6 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field8 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Image = "../wwwroot/image/22_9.png",
                    HasCrest = false
                }, id));
            }

            //23
            for (int i = 0; i < 4; i++)
            {
                id++;
                _cards.Add(new Card(
                new Tile
                {
                    Id = 23,
                    Field1 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field2 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field3 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field4 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field5 = new LandType { Name = "Other" },
                    Field6 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field7 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Field8 = new LandType { Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field9 = new LandType { Name = "Land", Meeple = MeepleType.PEASANT },
                    Image = "../wwwroot/image/23_4.png",
                    HasCrest = false
                }, id));
            }

            //24
            id++;
            _cards.Add(new Card(
                new Tile
                { 
                    Id = 24,
                    Field1 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                    Field2 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field3 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                    Field4 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field5 = new LandType{Name = "Other"},
                    Field6 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field7 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                    Field8 = new LandType{ Name = "Road", Meeple = MeepleType.HIGHWAYMAN },
                    Field9 = new LandType{ Name = "Land", Meeple = MeepleType.PEASANT },
                    Image = "../wwwroot/image/24_1.png",
                    HasCrest = false
                }, id));

            return new List<Card>(_cards);
        }
    }

}

