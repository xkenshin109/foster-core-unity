using FosterServer.Core.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FosterServer.Core.Utilities
{
    public static class ClientSend
    {

        public static void SendTCPData(this TCP a_tcp,int a_toClient, Packet a_packet)
        {
            a_packet.WriteLength();
            a_tcp.SendData(a_packet);
        }

        public static void SendUDPData(this UDP a_udp,int a_toClient, Packet a_packet)
        {
            a_packet.WriteLength();
            a_udp.SendData(a_packet);
        }
    }
}
