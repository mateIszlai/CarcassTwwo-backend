using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace CarcassTwwo.Models
{
    public class ScoreBoard
    {
        public Dictionary<Client, int> Players { get; private set; }

        public ScoreBoard(HashSet<Client> players)
        {
            players.ToList().ForEach(pl => Players[pl] = 0);
        }


        {
            //this should get data from the game/board, and use it to update the points e.g.:
            foreach(Client player in players)
            {
                player.Points += 42;
            }
        }
    }
}
