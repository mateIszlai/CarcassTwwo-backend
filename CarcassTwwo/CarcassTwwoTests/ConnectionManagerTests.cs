using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CarcassTwwo.Hubs;
using CarcassTwwo.Models;
using NUnit.Framework;

namespace CarcassTwwoTests
{
    [TestFixture]
    class ConnectionManagerTests
    {
        ConnectionManager connectionManager;
        HashSet<Client> connections;
        [SetUp]
        public void SetUp()
        {
            connectionManager = new ConnectionManager();
            connectionManager.AddConnection("group", "user", true, "connectionId");
            connectionManager.AddConnection("group", "user2", false, "connectionId2");
            connections = connectionManager.GetConnections("group");
        }

        [TearDown]
        public void TearDown()
        {
            connectionManager = null;
            connections = null;
        }
        [Test]
        public void GroupIsCreated()
        {
            var groups = connectionManager.GetGroupNames();
            Assert.IsTrue(groups.Contains("group"));
        }

        [Test]
        public void ConnectionsAreReturned()
        {
            Assert.IsNotNull(connectionManager.GetConnections("group"));
        }
        [Test]
        public void UserIsAddedToGroup()
        {
            Assert.IsNotNull(connections.FirstOrDefault(client => client.Name == "user2"));
        }

        [Test]
        public void GroupNamesAreReturned()
        {
            Assert.IsNotNull(connectionManager.GetGroupNames());
        }

        [Test]
        public void RemovedConnectionIsNoLongerInGroup()
        {
            connectionManager.RemoveConnection("connectionId2");
            Assert.IsNull(connections.FirstOrDefault(client => client.Name == "user2"));
        }

        [Test]
        public void GroupIsReturned()
        {
            Group group = connectionManager.GetGroup("group");
            Assert.IsNotNull(group);
        }

        [Test]
        public void GroupIsRemoved()
        {
            connectionManager.RemoveGroup("group");
            Assert.IsNull(connectionManager.GetGroup("group"));
        }

    }
}
