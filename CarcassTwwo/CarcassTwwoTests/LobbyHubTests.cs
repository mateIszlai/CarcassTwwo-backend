using NUnit.Framework;
using NSubstitute;
using System;
using CarcassTwwo.Hubs;
using System.Collections.Generic;

namespace CarcassTwwoTests
{
    public class LobbyHubTests
    {
        LobbyHub _lobbyHub;

        [SetUp]
        public void Setup()
        {
            _lobbyHub = new LobbyHub();
        }

        [TearDown]
        public void TearDown()
        {
            _lobbyHub = null;
        }

        [Test]
        public void GenerateRoomStringReturnRandomString()
        {
            var randomNames = new List<string>();
            for(int i = 0; i < 100; i++)
            {
                randomNames.Add(_lobbyHub.GenerateRoomString());
            }
            var actual = false;
            foreach(var name in randomNames)
            {
                if(randomNames.FindAll(s => s.Equals(name)).Count != 0)
                {
                    actual = true;
                    break;
                }
            }
            Assert.IsFalse(actual);
        }
    }
}