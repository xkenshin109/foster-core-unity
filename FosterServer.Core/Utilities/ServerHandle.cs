using FosterServer.Core.DataModels;
using FosterServer.Core.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FosterServer.Core.Utilities
{
    public class ServerHandle
    {
        public static Result WelcomeReceived(int a_fromClient, Packet a_packet)
        {
            int _clientIdCheck = a_packet.ReadInt();
            string _username = a_packet.ReadString();
            Console.WriteLine($"{Server.m_clients[_clientIdCheck].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {a_fromClient}");
            if(a_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {a_fromClient} has assumed the wrong client ID({_clientIdCheck})!");
            }
            return Result.Valid();
        }

        public static Result UDPTestReceived(int a_fromClient, Packet a_packet)
        {
            string _msg = a_packet.ReadString();

            Console.WriteLine($"Received packet via UDP, Contains message: {_msg}");
            return Result.Valid();
        }

        public static Result LoginRequested(int a_fromClient, Packet a_packet)
        {
            //string _username = a_packet.ReadString();
            //string _password = a_packet.ReadString();

            Console.WriteLine($"Received packet LoginRequest, User connecting: {a_fromClient}");
            return Result.Valid();
        }

        public static Result DisconnectUser(int a_fromClient, Packet a_packet)
        {
            if (Server.m_clients.ContainsKey(a_fromClient))
            {
                try
                {
                    var packet = new Packet(a_fromClient, ClientPackets.disconnect);
                    if (Server.m_clients[a_fromClient].udp.IsConnected)
                    {
                        Server.m_clients[a_fromClient].udp.m_udpClient.Close();
                        Server.SendUDPData(Server.m_clients[a_fromClient].udp.endPoint, packet);
                    }
                    if (Server.m_clients[a_fromClient].tcp.IsConnected)
                    {
                        Server.m_clients[a_fromClient].tcp.socket.Close();
                    }
                    Console.WriteLine($"Player has disconnected from server");
                    return Result.Valid();
                }
                catch(Exception ie)
                {
                    Console.WriteLine($"ServerHandle.DisconnectUser() - Error in disconnecting {ie.Message}");
                    return Result.Error($"ServerHandle.DisconnectUser() - Error in disconnecting {ie.Message}");
                }
            }
            return Result.Valid();
            
        }
    }
}
