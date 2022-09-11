using FosterServer.Core.Networking;
using FosterServer.Core.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace FosterServerUdp
{
    public class FosterServer
    {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        
        /// <summary>
        /// Constructor:
        /// FosterServer with specified MAX_PLAYERS on a specific PORT
        /// </summary>
        /// <param name="a_maxPlayers"></param>
        /// <param name="a_port"></param>
        public FosterServer(int a_maxPlayers, int a_port)
        {
            MaxPlayers = a_maxPlayers;
            Port = a_port;

            Server.Start(MaxPlayers, Port);
        }

        /// <summary>
        /// Constructor:
        /// FosterServer defaults:
        ///     MAX_PLAYERS = 4 
        ///     PORT = 11000
        /// </summary>
        public FosterServer()
            :this(4, Constants.LISTENING_PORT)
        {
            
        }

    }
}
