using FosterServer.Core.Networking;
using FosterServer.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FosterServer.Core.DataModels
{
    public class UDP
    {
        public IPEndPoint endPoint;

        private int id;

        public UdpClient m_udpClient;
        public bool IsConnected = false;

        public UDP(int _id)
        {
            id = _id;

        }

        public void Connect(IPEndPoint _endPoint, bool _isServer = false)
        {
            endPoint = _endPoint;
            m_udpClient = new UdpClient() { ExclusiveAddressUse = true };
            try
            {

                m_udpClient.Connect(endPoint);
                if (!_isServer)
                {
                    m_udpClient.BeginReceive(ReceiveCallback, null);
                }

                m_udpClient.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
                m_udpClient.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);

                IsConnected = true;
            } catch (Exception e)
            {
                IsConnected = false;
                Console.WriteLine(e.ToString());
            }
        }

        public void SendData(Packet _packet)
        {
            m_udpClient.Send(_packet.ToArray(), _packet.Length());
        }
        public void ReceiveCallback(IAsyncResult a_result)
        {
            try
            {
                IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] _data = m_udpClient.EndReceive(a_result, ref _clientEndPoint);
                m_udpClient.BeginReceive(ReceiveCallback, null);

                if (_data.Length < 4)
                {
                    return;
                }
                using (Packet _pack = new Packet(_data))
                {
                    HandleData(_pack);
                }
            }
            catch (Exception _e)
            {
                Console.WriteLine($"Error receiving UDP data: {_e}");
            }
        }
        public void HandleData(Packet _packetData)
        {
            //ThreadManager.ExecuteOnMainThread(() =>
            //{
            PacketHandler<ServerPackets>.PacketHandlerDict[_packetData.PacketValue].ForEach(x => x.Invoke(_packetData.Id, _packetData));
            //});
        }
        public void Disconnect()
        {
            m_udpClient.Close();
            IsConnected = false;
        }
    }
}
