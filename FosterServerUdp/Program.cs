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
        static void Properties()
        {
            GameEntity a = new GameEntity(0, 0);
            foreach (var prop in a.GameProperties)
            {
                Console.WriteLine($"{prop} = {a.GetGameProperty(prop.Key)}");
            }

            GameEngine.AddGameProperty("Health", typeof(double), 100.0f);
            GameEngine.AddGameProperty("Mana", typeof(float), 100);
            GameEngine.AddGameProperty("Gold", typeof(int), 0);
            GameEngine.AddGameProperty("Strength", typeof(int), 20);
            GameEngine.AddGameProperty("Defense", typeof(int), 10);
            GameEngine.AddGameProperty("Intellect", typeof(int), 10);
            GameEngine.AddGameProperty("Dexterity", typeof(int), 10);
            GameEngine.AddGameProperty("Wisdom", typeof(int), 10);
            GameEngine.AddGameProperty("Charisma", typeof(int), 10);
            GameEntity a1 = new GameEntity(0, 0);
            foreach (var prop in a1.GameProperties)
            {
                Console.WriteLine($"{prop} = {a1.GetGameProperty(prop.Key).GetType().Name}");
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
    }
}
