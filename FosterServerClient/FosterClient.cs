using FosterServer.Core.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using FosterServer.Core.DataModels;
namespace FosterServerClient
{
    public class FosterClient
    {
        public Client Client { get; set; }
        public FosterClient()
        {
            string iPAddress = "192.168.1.69";
            Client = new Client(0, true);
            Client.tcp.Connect(new TcpClient(iPAddress, Constants.LISTENING_PORT));
            Client.tcp.SendData(new Packet(Client.id, ClientPackets.loginReceived));
            //Client.udp.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.69"), Constants.LISTENING_PORT));
            //Client.udp.SendData(new Packet(0, ClientPackets.loginReceived));
            //Client.udp.SendData(new Packet(Client.id, ClientPackets.disconnect));
        }
    }
}
