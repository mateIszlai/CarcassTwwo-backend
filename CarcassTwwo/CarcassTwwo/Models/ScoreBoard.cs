using CarcassTwwo.Models.Places;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarcassTwwo.Models
{
    public class ScoreBoard : IRoadScoreCounter, ICityScoreCounter, IMonasteryScoreCounter, ILandScoreCounter
    {
        public Dictionary<Client, int> Players { get; private set; }

        public ScoreBoard(HashSet<Client> players)
        {
            Players = new Dictionary<Client, int>();
            players.ToList().ForEach(pl => Players[pl] = 0);
        }

        public void CheckOwnerOfCity(City city)
        {
            Dictionary<Client, int> meepleCountByPlayers = new Dictionary<Client, int>();
            if(city.Meeples.Count > 0)
            {
                foreach(var meeple in city.Meeples)
                {
                    _ = meepleCountByPlayers.Keys.Contains(meeple.Owner) ? meepleCountByPlayers[meeple.Owner]++ : meepleCountByPlayers[meeple.Owner] = 1;
                }
                int maxCount = meepleCountByPlayers.Values.Max();
                var cityOwners = meepleCountByPlayers.Keys.Where(k => meepleCountByPlayers[k] == maxCount).ToList();
                AddPointsForCity(cityOwners, city);
            }
        }

        private void AddPointsForCity(List<Client> players, City city)
        {
            int numOfCards = city.Size;
            int numOfMeeples = city.Meeples.Count;
            int numOfCrests = 0;

            city.CityParts.ToList().ForEach(cp => { if (cp.HasCrest) numOfCrests++; });

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
                Players[player] += numOfCrests * (int)Points.CREST;
            }
        }

        public void CheckOwnerOfRoad(Road road)
        {
            Dictionary<Client, int> meepleCountByPlayers = new Dictionary<Client, int>();
            if(road.Meeples.Count > 0)
            {
                foreach (var meeple in road.Meeples)
                {
                    _ = meepleCountByPlayers.Keys.Contains(meeple.Owner) ? meepleCountByPlayers[meeple.Owner]++ : meepleCountByPlayers[meeple.Owner] = 1;
                }
                int maxCount = meepleCountByPlayers.Values.Max();
                var roadOwners = meepleCountByPlayers.Keys.Where(k => meepleCountByPlayers[k] == maxCount).ToList();
                AddPointsForRoad(roadOwners, road);
            }
        }

        private void AddPointsForRoad(List<Client> players, Road road)
        {
            int numOfCards = road.RoadParts.Count;
            foreach (Client player in players)
            {
                Players[player] += numOfCards * (int)Points.ROADTILE;
            }
        }

        public void CheckOwnerOfLand(GrassLand land, int cities)
        {
            Dictionary<Client, int> meepleCountByPlayers = new Dictionary<Client, int>();
            if(land.Meeples.Count > 0)
            {
                foreach (var meeple in land.Meeples)
                {
                    _ = meepleCountByPlayers.Keys.Contains(meeple.Owner) ? meepleCountByPlayers[meeple.Owner]++ : meepleCountByPlayers[meeple.Owner] = 1;
                }
                int maxCount = meepleCountByPlayers.Values.Max();
                var landOwners = meepleCountByPlayers.Keys.Where(k => meepleCountByPlayers[k] == maxCount).ToList();
                AddPointsForLand(landOwners, cities);
            }
        }

        private void AddPointsForLand(List<Client> players, int finishedCities)
        {
            foreach (Client player in players)
            {
                Players[player] += finishedCities * (int)Points.FINISHEDCITYONLAND;
            }
        }

        public void AddPointsForMonastery(Monastery monastery)
        {
            if(monastery.Meeples.Count > 0)
            {
                if(monastery.IsFinished)
                    Players[monastery.Meeples[0].Owner] += (int)Points.FINISHEDMONASTERY;
                else
                    Players[monastery.Meeples[0].Owner] += (9 - monastery.SurroundingCoordinates.Count) * (int)Points.MONASTERYTILE;
            }

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
        CREST = 2,
        MONASTERYTILE = 1,
        FINISHEDMONASTERY = 9,
        FINISHEDCITYONLAND = 3
    }


}
