using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FosterServer.Core.Enumerations
{
    public enum GameEntityEnum
    {
        Building,
        Furniture,
        Ground,
        Terrain,
        Empty
    }

    public class CharacterClasses
    {
        public const string Warrior = "Warrior";
        public const string Villian = "Villian";

        public static List<string> ToList()
        {
            return new List<string>()
            {
                Warrior,
                Villian
            };
        }
    }    

    public enum CharacterClass
    {
        Warrior,
        Villian
    }
}
