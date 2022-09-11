using FosterServer.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FosterServer.Core.DataModels
{
    /// <summary>Sent from server to client.</summary>
    public enum ServerPackets : int
    {
        none = 0,
        welcome = 1,
        udpTest = 2,
        login = 3,
        disconnect = 4,
    }

    /// <summary>Sent from client to server.</summary>
    public enum ClientPackets
    {
        none = 0,
        welcomeReceived = 1,
        updTestReceived = 2,
        loginReceived = 3,
        disconnect = 4
    }
    [DebuggerDisplay("Packet Type: {(m_serverPacket != ServerPackets.none? m_serverPacket.ToString(): m_clientPacket.ToString())} From: {IsServerPacket?\"Server\":\"Client\"}")]
    public class Packet : IDisposable
    {
        private List<byte> buffer;
        private byte[] readableBuffer;
        private int headerBytes;
        private int readPos;
        private ServerPackets m_serverPacket;
        private ClientPackets m_clientPacket;
        private int m_packetValue;

        /// <summary>
        /// Client Id associated to the connected Client
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Value of Packet to lookup
        /// </summary>
        public int PacketValue { get { return m_packetValue; } }

        /// <summary>
        /// Is the Packet sent from Server
        /// </summary>
        public bool IsServerPacket { get { return ServerPackets.none != m_serverPacket; } }

        /// <summary>
        /// Is the Packet sent from Client
        /// </summary>
        public bool IsClientPacket { get { return ClientPackets.none != m_clientPacket; } }

        /// <summary>Creates a new empty packet (without an ID).</summary>
        public Packet()
        {
            buffer = new List<byte>(); // Intitialize buffer
            readPos = 0; // Set readPos to 0
            m_clientPacket = ClientPackets.none;
            m_serverPacket = ServerPackets.none;
            Id = -1;
        }

        /// <summary>Creates a new packet with a given ID. Used for sending.</summary>
        /// <param name="_id">The packet ID.</param>
        public Packet(int a_id, ServerPackets a_serverPacket = ServerPackets.none)
        {
            Id = a_id;

            m_serverPacket = a_serverPacket;
            m_clientPacket = ClientPackets.none;

            buffer = new List<byte>(); // Intitialize buffer
            readPos = 0; // Set readPos to 0
            
            WriteHeader();
        }
        public Packet(int a_id, ClientPackets a_clientPacket = ClientPackets.none)
        {
            Id = a_id;

            m_clientPacket = a_clientPacket;
            m_serverPacket = ServerPackets.none;

            buffer = new List<byte>(); // Intitialize buffer
            readPos = 0; // Set readPos to 0

            WriteHeader();
        }

        /// <summary>Creates a packet from which data can be read. Used for receiving.</summary>
        /// <param name="_data">The bytes to add to the packet.</param>
        public Packet(byte[] a_data)
        {

            buffer = new List<byte>(); // Intitialize buffer
            readPos = 0; // Set readPos to 0

            SetBytes(a_data);
            Id = ReadHeader();
        }

        #region Functions
        /// <summary>Sets the packet's content and prepares it to be read.</summary>
        /// <param name="_data">The bytes to add to the packet.</param>
        public void SetBytes(byte[] _data)
        {
            Write(_data);
            readableBuffer = buffer.ToArray();
        }

        /// <summary>Inserts the length of the packet's content at the start of the buffer.</summary>
        public void WriteLength()
        {
            if(headerBytes != 0)
            {
                return; 
                //Header already constructed
            }
            int _totalMessageBytes = BitConverter.GetBytes(buffer.Count).Length;
            
            int _totalHeaderBytes = 
                BitConverter.GetBytes(_totalMessageBytes).Length + 
                BitConverter.GetBytes(m_clientPacket != ClientPackets.none ? (int)m_clientPacket : (int)m_serverPacket).Length +
                BitConverter.GetBytes(Id).Length +
                Encoding.ASCII.GetBytes(m_clientPacket != ClientPackets.none ? nameof(ClientPackets) : nameof(ServerPackets)).Length;

            _totalHeaderBytes += BitConverter.GetBytes(_totalHeaderBytes).Length;
            var packetType = m_clientPacket != ClientPackets.none ? nameof(ClientPackets) : nameof(ServerPackets);
            WriteFirst(m_clientPacket != ClientPackets.none ? (int)m_clientPacket : (int)m_serverPacket);
            WriteFirst(packetType);
            WriteFirst(Id); //Client ID
            WriteFirst(buffer.Count); // Insert the byte length of the packet at the very beginning
            WriteFirst(_totalMessageBytes);//Header Packet size            

            headerBytes = _totalHeaderBytes;
        }

        public void WriteHeader()
        {
            WriteLength();
        }

        /// <summary>Inserts the given int at the start of the buffer.</summary>
        /// <param name="_value">The int to insert.</param>
        public void InsertInt(int _value)
        {
            buffer.InsertRange(0, BitConverter.GetBytes(_value)); // Insert the int at the start of the buffer
        }

        /// <summary>Gets the packet's content in array form.</summary>
        public byte[] ToArray()
        {
            readableBuffer = buffer.ToArray();
            return readableBuffer;
        }

        /// <summary>Gets the length of the packet's content.</summary>
        public int Length()
        {
            return buffer.Count; // Return the length of buffer
        }

        /// <summary>Gets the length of the unread data contained in the packet.</summary>
        public int UnreadLength()
        {
            return Length() - readPos; // Return the remaining length (unread)
        }

        /// <summary>Resets the packet instance to allow it to be reused.</summary>
        /// <param name="_shouldReset">Whether or not to reset the packet.</param>
        public void Reset(bool _shouldReset = true)
        {
            if (_shouldReset)
            {
                buffer.Clear(); // Clear buffer
                readableBuffer = null;
                readPos = 0; // Reset readPos
            }
            else
            {
                readPos -= 4; // "Unread" the last read int
            }
        }
        #endregion

        #region Write Data
        /// <summary>Adds a byte to the packet.</summary>
        /// <param name="_value">The byte to add.</param>
        public void Write(byte _value)
        {
            buffer.Add(_value);
        }

        /// <summary>
        /// Adds a byte to the packet at beginning
        /// </summary>
        /// <param name="_value"></param>
        public void WriteFirst(byte _value)
        {
            buffer.Insert(0, _value);
        }

        /// <summary>Adds an array of bytes to the packet.</summary>
        /// <param name="_value">The byte array to add.</param>
        public void Write(byte[] _value)
        {
            buffer.AddRange(_value);
        }

        /// <summary>
        /// Adds a byte array to the packet at beginning
        /// </summary>
        /// <param name="_value"></param>
        public void WriteFirst(byte[] _value)
        {
            buffer.InsertRange(0, _value);
        }

        /// <summary>Adds a short to the packet.</summary>
        /// <param name="_value">The short to add.</param>
        public void Write(short _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }

        /// <summary>
        /// Adds a short to the packet at beginning
        /// </summary>
        /// <param name="_value"></param>
        public void WriteFirst(short _value)
        {
            buffer.InsertRange(0, BitConverter.GetBytes(_value));
        }

        /// <summary>Adds an int to the packet.</summary>
        /// <param name="_value">The int to add.</param>
        public void Write(int _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }

        /// <summary>
        /// Adds an int to the packet at beginning
        /// </summary>
        /// <param name="_value"></param>
        public void WriteFirst(int _value)
        {
            buffer.InsertRange(0, BitConverter.GetBytes(_value));
        }

        /// <summary>Adds a long to the packet.</summary>
        /// <param name="_value">The long to add.</param>
        public void Write(long _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }

        /// <summary>Adds a long to the packet at beginning</summary>
        /// <param name="_value">The long to add.</param>
        public void WriteFirst(long _value)
        {
            buffer.InsertRange(0, BitConverter.GetBytes(_value));
        }

        /// <summary>Adds a float to the packet.</summary>
        /// <param name="_value">The float to add.</param>
        public void Write(float _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }

        /// <summary>
        /// Adds a float to the packet at beginning
        /// </summary>
        /// <param name="_value"></param>
        public void WriteFirst(float _value)
        {
            buffer.InsertRange(0, BitConverter.GetBytes(_value));
        }
        /// <summary>Adds a bool to the packet.</summary>
        /// <param name="_value">The bool to add.</param>
        public void Write(bool _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }

        /// <summary>
        /// Adds a bool to the packet at beginning
        /// </summary>
        /// <param name="_value"></param>
        public void WriteFirst(bool _value)
        {
            buffer.InsertRange(0, BitConverter.GetBytes(_value));
        }

        /// <summary>Adds a string to the packet.</summary>
        /// <param name="_value">The string to add.</param>
        public void Write(string _value)
        {
            Write(_value.Length); // Add the length of the string to the packet
            buffer.AddRange(Encoding.ASCII.GetBytes(_value)); // Add the string itself
        }

        public void WriteFirst(string _value)
        {
            var data = Encoding.ASCII.GetBytes(_value);

            WriteFirst(data);
            WriteFirst(data.Length);
        }
        #endregion

        #region Read Data
        /// <summary>Reads a byte from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public byte ReadByte(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                byte _value = readableBuffer[readPos]; // Get the byte at readPos' position
                if (_moveReadPos)
                {
                    // If _moveReadPos is true
                    readPos += 1; // Increase readPos by 1
                }
                return _value; // Return the byte
            }
            else
            {
                throw new Exception("Could not read value of type 'byte'!");
            }
        }

        /// <summary>Reads an array of bytes from the packet.</summary>
        /// <param name="_length">The length of the byte array.</param>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public byte[] ReadBytes(int _length, bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                byte[] _value = buffer.GetRange(readPos, _length).ToArray(); // Get the bytes at readPos' position with a range of _length
                if (_moveReadPos)
                {
                    // If _moveReadPos is true
                    readPos += _length; // Increase readPos by _length
                }
                return _value; // Return the bytes
            }
            else
            {
                throw new Exception("Could not read value of type 'byte[]'!");
            }
        }

        /// <summary>Reads a short from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public short ReadShort(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                short _value = BitConverter.ToInt16(readableBuffer, readPos); // Convert the bytes to a short
                if (_moveReadPos)
                {
                    // If _moveReadPos is true and there are unread bytes
                    readPos += 2; // Increase readPos by 2
                }
                return _value; // Return the short
            }
            else
            {
                throw new Exception("Could not read value of type 'short'!");
            }
        }

        /// <summary>Reads an int from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public int ReadInt(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                int _value = BitConverter.ToInt32(readableBuffer, readPos); // Convert the bytes to an int
                if (_moveReadPos)
                {
                    // If _moveReadPos is true
                    readPos += 4; // Increase readPos by 4
                }
                return _value; // Return the int
            }
            else
            {
                throw new Exception("Could not read value of type 'int'!");
            }
        }

        /// <summary>Reads a long from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public long ReadLong(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                long _value = BitConverter.ToInt64(readableBuffer, readPos); // Convert the bytes to a long
                if (_moveReadPos)
                {
                    // If _moveReadPos is true
                    readPos += 8; // Increase readPos by 8
                }
                return _value; // Return the long
            }
            else
            {
                throw new Exception("Could not read value of type 'long'!");
            }
        }

        /// <summary>Reads a float from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public float ReadFloat(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                float _value = BitConverter.ToSingle(readableBuffer, readPos); // Convert the bytes to a float
                if (_moveReadPos)
                {
                    // If _moveReadPos is true
                    readPos += 4; // Increase readPos by 4
                }
                return _value; // Return the float
            }
            else
            {
                throw new Exception("Could not read value of type 'float'!");
            }
        }

        /// <summary>Reads a bool from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public bool ReadBool(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                bool _value = BitConverter.ToBoolean(readableBuffer, readPos); // Convert the bytes to a bool
                if (_moveReadPos)
                {
                    // If _moveReadPos is true
                    readPos += 1; // Increase readPos by 1
                }
                return _value; // Return the bool
            }
            else
            {
                throw new Exception("Could not read value of type 'bool'!");
            }
        }

        /// <summary>Reads a string from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public string ReadString(bool _moveReadPos = true)
        {
            try
            {
                int _length = ReadInt(); // Get the length of the string
                string _value = Encoding.ASCII.GetString(readableBuffer, readPos, _length); // Convert the bytes to a string
                if (_moveReadPos && _value.Length > 0)
                {
                    // If _moveReadPos is true string is not empty
                    readPos += _length; // Increase readPos by the length of the string
                }
                return _value; // Return the string
            }
            catch
            {
                throw new Exception("Could not read value of type 'string'!");
            }
        }
        
        /// <summary>
        /// Reads Header from Packet(
        ///     0 - ClientId(int), 
        ///     1 - PacketType(string), 
        ///     2 - packetValue(int))
        /// </summary>
        /// <returns></returns>
        public int ReadHeader()
        {
            int clientHeaderBytes = ReadInt();
            int clientMessageCount = ReadInt();
            int clientId = ReadInt();
            string packetType = ReadString();
            m_packetValue = ReadInt();
            switch (packetType)
            {
                case nameof(ServerPackets):
                    m_serverPacket = (ServerPackets)m_packetValue;
                    m_clientPacket = ClientPackets.none;
                    break;
                case nameof(ClientPackets):
                    m_serverPacket = ServerPackets.none;
                    m_clientPacket = (ClientPackets)m_packetValue;
                    break;
                default:
                    throw new Exception("Problem reading header from packet.");
            }
            return clientId;
        }
        #endregion

        private bool disposed = false;

        protected virtual void Dispose(bool _disposing)
        {
            if (!disposed)
            {
                if (_disposing)
                {
                    buffer = null;
                    readableBuffer = null;
                    readPos = 0;
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public class PacketHandler<T>
    {
        public bool IsServer => typeof(T) == typeof(ClientPackets);
        public bool IsClient => typeof(T) == typeof(ServerPackets);
        public delegate void PacketHandlerEvent(int _fromClient, Packet _packet);
        private static Dictionary<int, List<PacketHandlerEvent>> m_packetHandler = new Dictionary<int, List<PacketHandlerEvent>>();
        public static Dictionary<int, List<PacketHandlerEvent>> PacketHandlerDict
        {
            get
            {
                if(m_packetHandler.Count == 0)
                {
                    //If IsServer
                    if (typeof(T) == typeof(ClientPackets))
                    {
                        InitializeServerPacketData();
                    }
                    //If IsClient
                    if (typeof(T) == typeof(ServerPackets))
                    {
                        InitializeClientPacketData();
                    }
                }
                return m_packetHandler;
            }
            private set { m_packetHandler = value; }
        }

        public PacketHandler()
        {

        }

        public static void InitializeServerPacketData()
        {
            m_packetHandler = new Dictionary<int, List<PacketHandlerEvent>>()
            {
                {(int) ClientPackets.welcomeReceived, new List<PacketHandlerEvent>(){ ServerHandle.WelcomeReceived } },
                {(int) ClientPackets.updTestReceived, new List<PacketHandlerEvent>(){ ServerHandle.UDPTestReceived } },
                {(int) ClientPackets.loginReceived, new List<PacketHandlerEvent>(){ ServerHandle.LoginRequested } },
                {(int) ClientPackets.disconnect, new List<PacketHandlerEvent>(){ ServerHandle.DisconnectUser } }
            };
        }

        public static void InitializeClientPacketData()
        {
            m_packetHandler = new Dictionary<int, List<PacketHandlerEvent>>()
            {
                { (int) ServerPackets.welcome, new List<PacketHandlerEvent>(){ ClientHandle.ServerWelcome} },
                { (int) ServerPackets.login, new List<PacketHandlerEvent>(){  ClientHandle.ServerLoginResponse} },
                { (int) ServerPackets.disconnect, new List<PacketHandlerEvent>(){ ClientHandle.DisconnectFromServer} },
                { (int) ServerPackets.udpTest, new List<PacketHandlerEvent>(){ ClientHandle.ServerWelcome} }
            };
        }

        /// <summary>
        /// Add Event to be called for packets. Server Response
        /// </summary>
        /// <param name="a_serverPacket"></param>
        /// <param name="a_event"></param>
        public static void AddPacketHandleEvent(ServerPackets a_serverPacket, PacketHandlerEvent a_event)
        {
            List<PacketHandlerEvent> events;
            PacketHandlerDict.TryGetValue((int)a_serverPacket, out events);
            if (events != null)
            {
                if (!events.Contains(a_event)){
                    events.Add(a_event);
                }
            }
            else
            {
                PacketHandlerDict.Add((int)a_serverPacket, new List<PacketHandlerEvent>() { a_event });
            }
        }

        /// <summary>
        /// Add Event to be called for packets. Clients Response
        /// </summary>
        /// <param name="a_clientPacket"></param>
        /// <param name="a_event"></param>
        public static void AddPacketHandleEvent(ClientPackets a_clientPacket, PacketHandlerEvent a_event)
        {
            List<PacketHandlerEvent> events;
            PacketHandlerDict.TryGetValue((int)a_clientPacket, out events);
            if (events != null)
            {
                if (!events.Contains(a_event))
                {
                    events.Add(a_event);
                }
            }
            else
            {
                PacketHandlerDict.Add((int)a_clientPacket, new List<PacketHandlerEvent>() { a_event });
            }
        }
    }
}
