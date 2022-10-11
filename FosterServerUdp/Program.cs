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
            GameEntity a = new GameEntity(0, 0);
            foreach(var prop in a.GameProperties)
            {
                Console.WriteLine($"{prop} = {a.GetGameProperty(prop.Key)}");
            }

            GameEngine.AddGameProperty("Health", typeof(float), 100);
            GameEngine.AddGameProperty("Mana", typeof(float), 100);
            GameEngine.AddGameProperty("Gold", typeof(int), 0);
            GameEngine.AddGameProperty("Strength", typeof(int), 20);
            GameEngine.AddGameProperty("Defense", typeof(int), 10);
            GameEngine.AddGameProperty("Intellect", typeof(int), 10);
            GameEngine.AddGameProperty("Dexterity", typeof(int), 10);
            GameEngine.AddGameProperty("Wisdom", typeof(int), 10);
            GameEngine.AddGameProperty("Charisma", typeof(int), 10);
            GameEntity a1 = new GameEntity(0, 0);
            foreach (var prop in a.GameProperties)
            {
                Console.WriteLine($"{prop} = {a1.GetGameProperty(prop.Key)}");
            }
        }

        static void RollDice()
        {
            Console.WriteLine("D4 = " + DiceManager.RollDice(Dices.D4));
            Console.WriteLine("D6 = " + DiceManager.RollDice(Dices.D6));
            Console.WriteLine("D8 = " + DiceManager.RollDice(Dices.D8));
            Console.WriteLine("D10 = " + DiceManager.RollDice(Dices.D10));
            Console.WriteLine("D10P = " + DiceManager.RollDice(Dices.D10P));
            Console.WriteLine("D12 = " + DiceManager.RollDice(Dices.D12));
            Console.WriteLine("D20 = " + DiceManager.RollDice(Dices.D20));
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
