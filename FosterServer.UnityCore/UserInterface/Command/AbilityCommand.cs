using FosterServer.Core.Enumerations;
using FosterServer.Core.Logging;
using FosterServer.Core.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FosterServer.UnityCore.UserInterface.Command
{
    public abstract class AbilityCommand : MonoBehaviour
    {
        public string AbilityName;
        private bool requiresDiceRole => DiceToUse != Dices.NoDice;
        public Dices DiceToUse = Dices.NoDice;

        public void Awake()
        {
        }
        public void Run()
        {
            if (requiresDiceRole)
            {
                var result = DiceManager.RollDice(DiceToUse);
                PerformAction(result);
            }
            else
            {
                PerformAction();
            }
        }

        public abstract void PerformAction();

        public abstract void PerformAction(float a_dcCheck);
    }
}
