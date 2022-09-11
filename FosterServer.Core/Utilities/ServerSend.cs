using FosterServer.Core.DataModels;
using FosterServer.Core.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FosterServer.Core.Utilities
{
    public class ServerSend
    {
        public static void SendTCPData(int a_toClient, Packet a_packet)
        {
            a_packet.WriteLength();
            Server.m_clients[a_toClient].tcp.SendData(a_packet);
        }

       public static void SendUDPData(int a_toClient, Packet a_packet)
        {
            a_packet.WriteLength();
            Server.m_clients[a_toClient].udp.SendData(a_packet);
        }

        public static void SendTCPDataToAll(Packet a_packet)
        {
            a_packet.WriteLength();
            for(int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.m_clients[i].tcp.SendData(a_packet);
            }
        }

        private static void SendTCPDataToAll(int a_exceptClient, Packet a_packet)
        {
            a_packet.WriteLength();
            for(int i = 1; i <= Server.MaxPlayers; i++)
            {
                if(i != a_exceptClient)
                {
                    Server.m_clients[i].tcp.SendData(a_packet);
                }

            }
        }

        public static void SendUDPDataToAll(Packet a_packet)
        {
            a_packet.WriteLength();
            for(int i = 1; i < Server.MaxPlayers; i++)
            {
                Server.m_clients[i].udp.SendData(a_packet);
            }
        }

        public static void SendUDPDataToAll(int a_exceptClient, Packet a_packet)
        {
            a_packet.WriteLength();
            for (int i = 1; i < Server.MaxPlayers; i++)
            {
                if(i != a_exceptClient)
                {
                    Server.m_clients[i].udp.SendData(a_packet);
                }                
            }
        }

        public static void Welcome(int a_toClient, string a_msg)
        {
            using (Packet _packet = new Packet(a_toClient, ServerPackets.welcome))
            {
                _packet.Write(a_msg);
                _packet.Write(a_toClient);

                SendTCPData(a_toClient, _packet);
            }
        }

        public static void UDPTest(int a_toClient)
        {
            using(Packet _packet = new Packet(a_toClient, ServerPackets.udpTest))
            {
                _packet.Write("A test packet for UDP");
                SendUDPData(a_toClient, _packet);
            }
        }
    }
}
