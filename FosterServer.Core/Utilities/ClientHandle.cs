using FosterServer.Core.DataModels;
using FosterServer.Core.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FosterServer.Core.Utilities
{
    public class ClientHandle
    {
        public static Result ServerWelcome(int a_fromClient, Packet a_packet)
        {
            
            string _username = a_packet.ReadString();
            Console.WriteLine($"{Server.m_clients[a_packet.Id].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {a_fromClient}");
            if (a_fromClient != a_packet.Id)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {a_fromClient} has assumed the wrong client ID({a_packet.Id})!");
            }
            return Result.Valid();
        }
        public static Result ServerLoginResponse(int a_fromClient, Packet a_packet)
        {
            return Result.Valid();
        }
        public static Result DisconnectFromServer(int a_fromClient, Packet a_packet)
        {
            try
            {
                Console.WriteLine($"Disconnected from server successful");
                return Result.Valid();
            }
            catch(Exception ie)
            {
                Console.WriteLine("ClientHandle.DisconnectFromServer() - " + ie?.Message);
                return Result.Error("ClientHandle.DisconnectFromServer() - " + ie?.Message);
            }
            
        }
    }
}
