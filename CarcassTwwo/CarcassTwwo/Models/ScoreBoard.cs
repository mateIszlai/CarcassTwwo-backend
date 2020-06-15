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

        internal void CheckOwnerOfLand(GrassLand land)
        {
            Dictionary<Client, int> meepleCountByPlayers = new Dictionary<Client, int>();
            foreach (var meeple in land.Meeples)
            {
                _ = meepleCountByPlayers[meeple.Owner] != 0 ? meepleCountByPlayers[meeple.Owner]++ : meepleCountByPlayers[meeple.Owner] = 1;
            }
            int maxCount = meepleCountByPlayers.Values.Max();
            var landOwners = meepleCountByPlayers.Keys.Where(k => meepleCountByPlayers[k] == maxCount).ToList();
            AddPointsForLand(landOwners, land);
        }

        private void AddPointsForLand(List<Client> players, GrassLand land)
        {
            //TODO HOW TO GET THE FINISHED CITIES??

            int finishedCities = 1;
            foreach (Client player in players)
            {
                Players[player] += finishedCities * (int)Points.FINISHEDCITYONLAND;
            }
        }

        internal void AddPointsForMonastery(Monastery monastery)
        {
            if(monastery.IsFinished)
                Players[monastery.Meeples[0].Owner] += (int)Points.FINISHEDMONASTERY;
            else
                Players[monastery.Meeples[0].Owner] += (9 - monastery.SurroundingCoordinates.Count) * (int)Points.MONASTERYTILE;

        }

        internal Client GetWinner()
        {
            int maxPoint = Players.Values.Max();
            var winners = Players.Keys.First(k => Players[k] == maxPoint);
            return winners;
        }
    }

    public enum Points
    {
        ROADTILE = 1,
        CITYTILE = 2,
        CITYMEEPLE = 2,
        OPENCITYTILE = 1,
        OPENCITYMEEPLE = 1,
        MONASTERYTILE = 1,
        FINISHEDMONASTERY = 9,
        FINISHEDCITYONLAND = 3
    }


}
