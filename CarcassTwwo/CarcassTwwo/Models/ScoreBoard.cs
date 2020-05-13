using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models
{
    public class ScoreBoard
    {
        HashSet<Client> players;

        public ScoreBoard(HashSet<Client> players)
        {
            this.players = players;
        }

        //This class could be used to keep track of every players' score
        //like in the real game

        public void CountPointsAfterRound()
        {
            //this should get data from the game/board, and use it to update the points e.g.:
            foreach(Client player in players)
            {
                player.Points += 42;
            }
        }
    }
}
