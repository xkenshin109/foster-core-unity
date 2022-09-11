using FosterServer.Core.Networking;
using FosterServer.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FosterServer.Core.DataModels
{
    public class TCP
    {
        public TcpClient socket;

        private int id;
        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;
        public bool IsConnected = false;
        public TCP(int _id)
        {
            id = _id;
        }

        public void Connect(TcpClient _socket, bool isClient = true)
        {
            socket = _socket;
            socket.NoDelay = true;
            ///socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.PacketInformation, true);
            socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, true);
            socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, true);

            stream = socket.GetStream();

            socket.ReceiveBufferSize = Client.dataBufferSize;
            socket.SendBufferSize = Client.dataBufferSize;

            receivedData = new Packet();
            receiveBuffer = new byte[Client.dataBufferSize];
            if (isClient)
            {
                stream.BeginRead(receiveBuffer, 0, Client.dataBufferSize, ReceiveCallback, stream);
                //Thread readThread = new Thread(ReadThread);
                //readThread.Start();
            }
            IsConnected = true;
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.Write(_packet.ToArray(), 0, _packet.Length());
                    stream.Flush();
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error sending data to player {id} via TCP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                stream.BeginRead(receiveBuffer.ToArray(), 0, Client.dataBufferSize, ReceiveCallback, null);
                byte[] data = new byte[_byteLength];
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    var res = reader.ReadBytes(_byteLength);
                    res.CopyTo(data, 0);
                }

                receivedData = new Packet(data);

            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error receiving TCP data: {_ex}");
                // TODO: disconnect
            }
        }

        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0;

            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 4)
            {
                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }

            while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
            {
                byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        PacketHandler<ServerPackets>.PacketHandlerDict[id].ForEach(x => x(id, _packet));
                    }
                });

                _packetLength = 0;
                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            if (_packetLength <= 1)
            {
                return true;
            }

            return false;
        }

        public void Disconnect()
        {
            stream.Close();
            socket.Close();
            IsConnected = false;
        }

        public void ReadThread()
        {
            while (true)
            {
                if (stream.DataAvailable)
                {
                    
                    using(BinaryReader reader = new BinaryReader(stream))
                    {
                        

                    }
                }
            }
        }
    }


}
