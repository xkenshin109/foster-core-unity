using FosterServer.Core.DataModels;
using FosterServer.Core.Utilities;
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
    public class Server
    {
        private static bool m_initialized = false;
        public bool IsRunning => m_initialized;

        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public static Dictionary<int, Client> m_clients = new Dictionary<int, Client>();
        public static int NextClientId { 
            get
            {
                return m_clients.Keys.First(x => !m_clients[x].udp.IsConnected && !m_clients[x].tcp.IsConnected);
            } 
        }
        public static PacketHandler<ClientPackets> m_clientPacket = new PacketHandler<ClientPackets>();
        private static TcpListener m_tcpListener;
        private static UdpClient m_udpListener;

        public static void Start(int a_maxPlayers, int a_port)
        {
            MaxPlayers = a_maxPlayers;
            Port = a_port;

            Console.WriteLine("Starting server...");
            InitializeServerData();

            m_tcpListener = new TcpListener(IPAddress.Any, Port);
            m_tcpListener.ExclusiveAddressUse = false;
            m_tcpListener.Start();
            //m_tcpListener.BeginAcceptTcpClient(TCPConnectCallback, m_tcpListener.Server);
            m_tcpListener.Server.NoDelay = true;
            m_tcpListener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, true);
            m_tcpListener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, true);

            m_udpListener = new UdpClient() { ExclusiveAddressUse = true };
            m_udpListener.Client.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.69"),Port));
            m_udpListener.BeginReceive(UDPReceiveCallback, null);

            Console.WriteLine($"Server started on {Port}.");
            m_initialized = true;

        }

        public static void SendUDPData(IPEndPoint a_clientEndPoint, Packet a_packet)
        {
            try
            {
                if (a_clientEndPoint != null)
                {
                    m_udpListener.BeginSend(a_packet.ToArray(), a_packet.Length(), a_clientEndPoint, null, null);
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error sending data to {a_clientEndPoint} via UDP: {_ex}");
            }
        }

        public static void SendTcpData(TcpClient a_client,Packet a_packet)
        {
            try
            {
                NetworkStream stream = a_client.GetStream();
                stream.Write(a_packet.ToArray(), 0, a_packet.Length());
                stream.Flush();
            }
            catch (Exception _ex)
            {
                Console.WriteLine("Server.SendTcpData - " + _ex?.Message);
            }
        }
        public static void DisconnectServer()
        {
            Console.WriteLine("Closing connections to clients...");
            foreach(var client in m_clients)
            {
                client.Value.Disconnect();
            }
            Console.WriteLine("Server shutting down...");
            m_tcpListener.Stop();
            m_udpListener.Close();
            m_initialized = false;
        }
        //private static Task<bool> TCPConnectCallback(IAsyncResult a_result)
        //{
        //    //try
        //    //{
        //    //    if (!m_initialized) return Task.false;
        //    //    TcpClient tcpClient = m_tcpListener.EndAcceptTcpClient(a_result);
        //    //    m_tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
        //    //    Console.WriteLine($"Incoming connection from {tcpClient.Client.RemoteEndPoint}...");

        //    //    for (int i = 1; i <= MaxPlayers; i++)
        //    //    {
        //    //        if (m_clients[i].tcp.socket == null)
        //    //        {
        //    //            m_clients[i].tcp.Connect(tcpClient, false);
        //    //            var loginPacket = new Packet(i, ServerPackets.login);
        //    //            var test = new Packet(loginPacket.ToArray());
        //    //            SendTcpData(tcpClient, loginPacket);
        //    //            return;
        //    //        }
        //    //        else
        //    //        {
        //    //            //Process and handle packet
        //    //        }
        //    //    }

        //    //    Console.WriteLine($"{tcpClient.Client.RemoteEndPoint} failed to connect: Server Full!");
        //    //}catch(Exception ie)
        //    //{
        //    //    Console.WriteLine($"Connection closed:{ie.Message}");
        //    //}

        //}

        private static void UDPReceiveCallback(IAsyncResult a_result)
        {
            try
            {
                if (!m_initialized) return;
                IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] _data = m_udpListener.EndReceive(a_result, ref _clientEndPoint);
                m_udpListener.BeginReceive(UDPReceiveCallback, null);

                if(_data.Length < 4)
                {
                    return;
                }
                using(Packet _pack = new Packet(_data))
                {
                    var _clientId = _pack.Id;
                    if(_pack.Id == 0 || m_clients[_pack.Id].udp.endPoint == null)
                    {
                        _clientId = NextClientId;
                        m_clients[NextClientId].udp.Connect(_clientEndPoint, true);
                        var loginPacket = new Packet(_clientId, ServerPackets.login);

                        SendUDPData(_clientEndPoint, loginPacket);
                        PacketHandler<ClientPackets>.PacketHandlerDict[_pack.PacketValue].ForEach(x=>x(_clientId, _pack));
                        Console.WriteLine($"New client has connected and been assigned {_clientId} at {_clientEndPoint.ToString()}");
                        return;
                    }
                    if (m_clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                    {
                        PacketHandler<ClientPackets>.PacketHandlerDict[_pack.PacketValue].ForEach(x => x(_clientId, _pack));
                    }
                }
            }catch(Exception _e)
            {
                Console.WriteLine($"Error receiving UDP data: {_e}");
            }
        }

        private static void InitializeServerData()
        {
            for(int i = 1; i <= MaxPlayers; i++)
            {
                if (!m_clients.ContainsKey(i))
                {
                    m_clients.Add(i, new Client(i));
                }
                else
                {
                    m_clients[i] = new Client(i);
                }
                
            }
                        
            Console.WriteLine($"Initialized packets..{PacketHandler<ClientPackets>.PacketHandlerDict.Count} Total packet types");
        }

    }
}
