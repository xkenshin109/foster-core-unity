using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FosterServer.Core.Pathfinding;
using FosterServer.Core.Models;
using FosterServer.Core.Manager;

namespace FosterServerUdp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var startingPoint = new GridPoint(-4, -15);
            var endingPoint = new GridPoint(10, 15);
            var result = Calculation.RunPathfindingWorkflow(startingPoint, endingPoint, 2);
            Console.WriteLine($"Starting Point: [{startingPoint.X},{startingPoint.Y}]");
            foreach(var point in result)
            {
                Console.WriteLine($"Moved to Point: [{point.X}, {point.Y}]");
            }
            Console.WriteLine($"Ending Point: [{endingPoint.X},{endingPoint.Y}]");
            Console.ReadLine();
            FosterServer f = new FosterServer();
            //Console.ReadKey();
        }
    }
}
