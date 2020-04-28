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

        public Client LastPlayer { get; set; }
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

        public Client PickPlayer()
        {
            //TODO: NextPlayer = ?
            var lastPlayerIndex = Players.FindIndex(c => c.Equals(LastPlayer));

            if (lastPlayerIndex == Players.Count - 1)
                LastPlayer = Players[0];
            else
                LastPlayer = Players[lastPlayerIndex + 1];

            return LastPlayer;
        }

        public Card PickRandomCard()
        {
            //TODO This will use the remaining tiles to create a new NextCard

            return NextCard;
        }

        public void PlaceCard(Coordinate coordinate, Card card)
        {
            GameBoard.CardCoordinates.Add(coordinate, card);
            card.Coordinate = coordinate;
            //card.SetSideOccupation(sideA, sideB, sideC, sideD); or something
        }
    }
}
