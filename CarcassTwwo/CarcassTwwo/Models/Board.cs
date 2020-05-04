using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models
{
    public class Board
    {
        public Dictionary<Coordinate, Card> CardCoordinates { get; set; }
        public HashSet<Coordinate> AvailableCoordinates { get; set; }
        public Dictionary<Coordinate, RequiredCard> AvailableCoordinatesR { get; set; }
        public void AddAvailableCoordinates(Card card)
        {
            /*
             * 1. We need to check all the available coordinates around the card.
             * 2. Create a Required card to that coordinate
             * 3. If there are existing coordinates around the required, we need to update it
             * e.g. we have one on the left and one on the bottom, then we have data for the 
             * required card's left and bottom (by the other cards' right and top)
             * 4. update the required card sides
             * This needs to be finished!!! It's not enought to create a required card with one side! 
             * We need to check every side!
             * e.g. this method calls the Update method everytime a new card is created. 
             * So to make this easier, I need to create the coordinate, and give it the method as a parameter
             * so making "new sg" wont be enough. 
             */
            if (card.TopIsFree)
            {
                AvailableCoordinatesR.Add(new Coordinate { x = card.Coordinate.x, y = card.Coordinate.y + 1 }, 
                                                            new RequiredCard(null, card.Top, null, null));

                AvailableCoordinates.Add(new Coordinate { x = card.Coordinate.x, y = card.Coordinate.y + 1 });
            }

            if (card.BottomIsFree)
            {
                AvailableCoordinatesR.Add(new Coordinate { x = card.Coordinate.x, y = card.Coordinate.y - 1 },
                                                            new RequiredCard(card.Bottom, null, null, null));

                AvailableCoordinates.Add(new Coordinate { x = card.Coordinate.x, y = card.Coordinate.y - 1 });
            }

            if (card.LeftIsFree)
            {
                AvailableCoordinatesR.Add(new Coordinate { x = card.Coordinate.x - 1, y = card.Coordinate.y },
                                                            new RequiredCard(null, null, null, card.Left));

                AvailableCoordinates.Add(new Coordinate { x = card.Coordinate.x - 1, y = card.Coordinate.y });
            }

            if (card.RightIsFree)
            {
                AvailableCoordinatesR.Add(new Coordinate { x = card.Coordinate.x + 1, y = card.Coordinate.y },
                                                               new RequiredCard(null, null, card.Right, null));
                AvailableCoordinates.Add(new Coordinate { x = card.Coordinate.x + 1, y = card.Coordinate.y });
            }
        }

        public void UpdateRequiredCard()
        {
            //TODO
            /*e.g.: this method gets a a required card, and checks every side of it. then refreshes.
             * this may be very time and resource consuming
             */
        }

        public void RemoveFromAvailableCoordinates(Coordinate coordinate)
        {
            AvailableCoordinates.Remove(coordinate);
        }

    }


    public struct Coordinate
    {
        public int x { get; set; }
        public int y { get; set; }
    }
}
