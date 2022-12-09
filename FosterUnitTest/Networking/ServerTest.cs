using FosterServer.Core.DataModels;
using FosterServer.Core.Networking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FosterUnitTest.Networking
{
    [TestClass]
    public class ServerTest
    {
        Mock<PacketHandler<ClientPackets>> m_packetHandler;
        Mock<Client> m_client;
        public ServerTest()
        {
            m_packetHandler = new Mock<PacketHandler<ClientPackets>>();
            m_client = new Mock<Client>();
        }
        #region Tests
        [TestMethod("Server Initialized and started")]
        public void Initialize()
        {
            StartServer();

            bool expectedResult = true;
            //Act
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] udpEndPoints = ipProperties.GetActiveUdpListeners();
            IPEndPoint[] tcpEndPoints = ipProperties.GetActiveTcpListeners();
            bool isUdpPort = false, isTcpPort = false;
            foreach (IPEndPoint udpEndPoint in udpEndPoints)
            {
                if (udpEndPoint.Port == Constants.LISTENING_PORT)
                {
                    isUdpPort = true;
                }
            }
            foreach (IPEndPoint tcpEndpoint in tcpEndPoints)
            {
                if (tcpEndpoint.Port == Constants.LISTENING_PORT)
                {
                    isTcpPort = true;
                }
            }

            //Assert
            Assert.AreEqual(expectedResult, isTcpPort);
            Assert.AreEqual(expectedResult, isUdpPort);

            //Shut down server
            Server.DisconnectServer();
        }

        [TestMethod("Server Login Packet - Single Login")]
        public void ServerLoginPacketSingleLogin()
        {
            bool waiting = true;
            int totalConnected = 0;
            int expectedConnected = 1;
            //Start the Server
            StartServer();

            PacketHandler<ClientPackets>.AddPacketHandleEvent(ClientPackets.loginReceived, (id, packet) => {
                Console.WriteLine(packet.Id);
                totalConnected = Server.m_clients.Sum(x => x.Value.IsConnected ? 1 : 0);
                waiting = false;
                return Result.Valid();
            });

            Client client = CreateUdpClient(1);

            client.udp.SendData(new Packet(client.id, ClientPackets.loginReceived));

            while (waiting) { }
            Assert.AreEqual(expectedConnected, totalConnected);
            client.Disconnect();
            Server.DisconnectServer();
        }

        [TestMethod("Server Login Packet - Two Clients Login")]
        public void ServerLoginPacketTwoClients()
        {
            bool waiting = true;
            int totalConnected = 0;
            int expectedConnected = 2;
            
            Client client = CreateUdpClient(1);

            Client client2 = CreateUdpClient(2);

            //Start the Server
            StartServer();

            PacketHandler<ClientPackets>.AddPacketHandleEvent(ClientPackets.loginReceived, (id, packet) => {
                Console.WriteLine(packet.Id);
                totalConnected = Server.m_clients.Sum(x => x.Value.IsConnected ? 1 : 0);
                if (totalConnected == expectedConnected)
                {
                    waiting = false;
                }
                return Result.Valid();
            });

            client.udp.SendData(new Packet(client.id, ClientPackets.loginReceived));
            client2.udp.SendData(new Packet(client2.id, ClientPackets.loginReceived));

            while (waiting) { }
            Assert.AreEqual(expectedConnected, totalConnected);
            client.Disconnect();
            client2.Disconnect();
            Server.DisconnectServer();
        }
        #endregion

        #region Common
        public Client CreateUdpClient(int id)
        {
            var newClient = new Client(id, true);
            newClient.udp.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.69"), Constants.LISTENING_PORT));
            return newClient;
        }

        public void StartServer()
        {
            Server.Start(4, Constants.LISTENING_PORT);
        }
        #endregion
    }
}
