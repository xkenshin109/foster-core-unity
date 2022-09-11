using FosterServer.Core.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FosterServer.Core.Networking
{
    public class Client
    {
        public static int dataBufferSize = 4096;

        public int id;
        public TCP tcp;
        public UDP udp;
        public bool IsConnected => tcp.IsConnected || udp.IsConnected;
        public Client(int _clientId, bool isClient = false)
        {
            id = _clientId;
            tcp = new TCP(id);
            udp = new UDP(id);
            if (isClient)
            {
                PacketHandler<ServerPackets>.AddPacketHandleEvent(ServerPackets.login, LoginListen);
            }
        }
        public void LoginListen(int a_fromClient, Packet a_packet)
        {
            if(id == 0)
            {
                id = a_packet.Id;
            }            
            Console.WriteLine($"Login Response from Server. Assigned Client {id}");
        }
        public void Disconnect()
        {
            if (tcp.IsConnected)
            {
                tcp.Disconnect();
            }
            if (udp.IsConnected)
            {
                udp.Disconnect();
            }
        }
    }
}
