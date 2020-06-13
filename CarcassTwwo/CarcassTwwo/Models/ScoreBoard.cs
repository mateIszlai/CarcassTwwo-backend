using CarcassTwwo.Models.Places;
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

        internal void CheckOwnerOfCity(City city)
        {
            Dictionary<Client, int> meepleCountByPlayers = new Dictionary<Client, int>();
            foreach(var meeple in city.Meeples)
            {
                _ = meepleCountByPlayers[meeple.Owner] != 0 ? meepleCountByPlayers[meeple.Owner]++ : meepleCountByPlayers[meeple.Owner] = 1;
            }
            int maxCount = meepleCountByPlayers.Values.Max();
            var cityOwners = meepleCountByPlayers.Keys.Where(k => meepleCountByPlayers[k] == maxCount).ToList();
            AddPointsForCity(cityOwners, city);
        }

        private void AddPointsForCity(List<Client> players, City city)
        {
            int numOfCards = city.Size;
            int numOfMeeples = city.Meeples.Count;
            foreach(Client player in players)
            {
                if (!city.IsOpen){
                    Players[player] += numOfCards * (int)Points.CITYTILE;
                    Players[player] += numOfMeeples * (int)Points.CITYMEEPLE;
                } else
                {
                    Players[player] += numOfCards * (int)Points.OPENCITYTILE;
                    Players[player] += numOfMeeples * (int)Points.OPENCITYMEEPLE;
                }
            }
        }

        internal void CheckOwnerOfRoad(Road road)
        {
            Dictionary<Client, int> meepleCountByPlayers = new Dictionary<Client, int>();
            foreach (var meeple in road.Meeples)
            {
                _ = meepleCountByPlayers[meeple.Owner] != 0 ? meepleCountByPlayers[meeple.Owner]++ : meepleCountByPlayers[meeple.Owner] = 1;
            }
            int maxCount = meepleCountByPlayers.Values.Max();
            var roadOwners = meepleCountByPlayers.Keys.Where(k => meepleCountByPlayers[k] == maxCount).ToList();
            AddPointsForRoad(roadOwners, road);
        }

        private void AddPointsForRoad(List<Client> players, Road road)
        {
            int numOfCards = road.RoadParts.Count;
            foreach (Client player in players)
            {
                Players[player] += numOfCards * (int)Points.ROADTILE;
            }
        }
    }
}
