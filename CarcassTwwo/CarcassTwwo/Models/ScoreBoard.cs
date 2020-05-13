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
    }
}
