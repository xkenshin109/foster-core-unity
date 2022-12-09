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
            object tet = new {test = 1 };
            m.TriggerEvent("", tet);
        }
        private static void testMethod(object param)
        {
            Console.WriteLine("Testing method");
        }

    }
}
