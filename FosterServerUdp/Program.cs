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

namespace FosterServerUdp
{
    public class Program
    {
        static void Main(string[] args)
        {
            //PathingTest();
            //PathingTestWithEntities();
            //PathingTestSteps();
            PathingTestStepsWithEntities();
            //FosterServer f = new FosterServer();
            Console.ReadKey();
        }

        static void PathingTest()
        {
            var startingPoint = new GridPoint(-4, -15);
            var endingPoint = new GridPoint(10, 15);
            var result = startingPoint.RunPathfindingWorkflow(endingPoint, 2);
            Console.WriteLine($"Starting Point: [{startingPoint.X},{startingPoint.Y}]");
            foreach (var point in result)
            {
                Console.WriteLine($"Moved to Point: [{point.X}, {point.Y}]");
            }
            Console.WriteLine($"Ending Point: [{endingPoint.X},{endingPoint.Y}]");
            Console.ReadLine();
        }
        static void PathingTestWithEntities()
        {
            var startingPoint = new GridPoint(0, 0);
            var endingPoint = new GridPoint(5, 5);
            Calculation.AddEntities(new List<Bounds> {
                new Bounds(new Vector3(1f,0f),new Vector3(1,1)),
                new Bounds(new Vector3(0f,1f),new Vector3(1,1)),
            });
            var result = startingPoint.RunPathfindingWorkflow(endingPoint, 5);
            foreach (var point in result)
            {
                Console.WriteLine($"Moved to Point: [{point.X}, {point.Y}]");
            }
            Console.WriteLine($"Ending Point: [{endingPoint.X},{endingPoint.Y}]");
            Console.ReadLine();
        }

        static void PathingTestSteps()
        {
            var startingPoint = new GridPoint(0, 0);
            var endingPoint = new GridPoint(5, 5);
            var result = startingPoint.RunPathfindingWorkflow(endingPoint, 20, 0.5f);
            foreach (var point in result)
            {
                Console.WriteLine($"Moved to Point: [{point.X}, {point.Y}]");
            }
            Console.WriteLine($"Ending Point: [{endingPoint.X},{endingPoint.Y}]");
            Console.ReadLine();
        }

        static void PathingTestStepsWithEntities()
        {
            var startingPoint = new GridPoint(0, 0);
            var endingPoint = new GridPoint(5, 5);
            Calculation.AddEntities(new List<Bounds> {
                new Bounds(new Vector3(1f,0f),new Vector3(1,1)),
                new Bounds(new Vector3(0f,1f),new Vector3(1,1)),
            });
            var result = startingPoint.RunPathfindingWorkflow(endingPoint, 20, .5f);
            foreach (var point in result)
            {
                Console.WriteLine($"Moved to Point: [{point.X}, {point.Y}]");
            }
            Console.WriteLine($"Ending Point: [{endingPoint.X},{endingPoint.Y}]");
            Console.ReadLine();
        }
    }
}
