using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FosterServer.Core.Pathfinding;
using FosterServer.Core.Models;
using FosterServer.Core.Manager;
using UnityEngine;
using FosterServer.Core.Utilities;
using FosterServer.Core.Enumerations;

namespace FosterServerUdp
{
    public class Program
    {
        static void Main(string[] args)
        {
            EventManager m = new EventManager();
            m.Init();
            m.StartListening("", testMethod);
            GameParameters para = new GameParameters();
            m.TriggerEvent("", para);
        }
        private static void testMethod(GameParameters param)
        {
            Console.WriteLine("Testing method");
        }

    }
}
