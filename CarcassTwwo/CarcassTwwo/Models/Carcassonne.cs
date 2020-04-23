using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models
{
    public class Carcassonne
    {
        public bool IsOver { get; set; }
        public bool IsStarted { get; set; }
        public int RoundsLeft { get; set; }
        public List<Client> Players { get; set; }
        public string WinnerName { get; set; }

        public Carcassonne(LocalGroup group)
        {
            Players = group.Members;
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
    }
}
