using FosterServer.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace FosterServer.Core.Manager
{
    public static class DiceManager
    {
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();
        public static float RollDice(Dices a_dice)
        {
            byte[] randomNumber = new byte[1];
            _generator.GetBytes(randomNumber);
            double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);
            // We are using Math.Max, and substracting 0.00000000001, 
            // to ensure "multiplier" will always be between 0.0 and .99999999999
            // Otherwise, it's possible for it to be "1", which causes problems in our rounding.
            double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);
            // We need to add one to the range, to allow for the rounding done with Math.Floor
            int range = (int)a_dice;
            double randomValueInRange = Math.Floor(multiplier * range) + 1;
            return (float)randomValueInRange;
        }

    }
}
