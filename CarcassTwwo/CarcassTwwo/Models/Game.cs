using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models
{
    public class Game
    {
        public int GameId { get; set; }
        public bool IsOver { get; set; }
        public bool IsStarted { get; set; }
        public int RoundsLeft { get; set; }
        public List<Client> Players { get; set; }
        public string WinnerName { get; set; }

        public Client NextPlayer { get; set; }
        public Card NextCard { get; set; }
        public Board GameBoard { get; set; }

        public Game(HashSet<Client> players)
        {
            GameBoard = new Board();
            Players = players.ToList();
            IsOver = false;
            IsStarted = true;
        }

        public void Play()
        {
            //TODO
            //params:
            //eg. card position, player, board size...
        }

        public void CheckWinner(List<Client> players)
        {
            int maxPoint = 0;
            foreach (var player in players)
            {
                if (player.Points > maxPoint)
                {
                    maxPoint = player.Points;
                    WinnerName = player.Name;
                }
            }
        }

        public Client PickFirstPlayer()
        {
            //TODO: NextPlayer = ?

            return NextPlayer;
        }

        public Card PickRandomCard()
        {
            //TODO This will use the remaining tiles to create a new NextCard

            return NextCard;
        }
        public void PlaceFirstCard()
        {
            //card: 20th card
            //coordinate: 0,0
        }

        public void PlaceCard(Coordinate coordinate, Card card)
        {
            GameBoard.CardCoordinates.Add(coordinate, card);
            card.Coordinate = coordinate;

            GameBoard.RemoveFromAvailableCoordinates(coordinate);
            card.SetSideOccupation(card, GameBoard);
            GameBoard.AddAvailableCoordinates(card); 
        }


        public List<Coordinate> GetPossiblePlacements()
        {
            return GameBoard.AvailableCoordinates;
            //this will check if the card can be placed. later.
        }
    }
}
