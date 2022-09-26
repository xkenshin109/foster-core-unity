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
    public class AbilityCommand : MonoBehaviour
    {
        public string AbilityName;
        private bool requiresDiceRole => DiceToUse != Dices.NoDice;
        public Dices DiceToUse = Dices.NoDice;
        public delegate void Run_DC_Check(float a_diceRoll);
        public event Run_DC_Check RunDCCheck;
        public delegate void Run_No_DC_Check();
        public event Run_No_DC_Check RunNoDCCheck;

        public void Awake()
        {
            if(requiresDiceRole && RunDCCheck == null)
            {
                FosterLog.Error($"{AbilityName} Dice Roll required and no Event assigned to triggered");
            }
            if(!requiresDiceRole && RunNoDCCheck == null)
            {
                FosterLog.Error($"{AbilityName} no Event assigned to triggered");
            }
        }
        public void Run()
        {
            if (requiresDiceRole)
            {
                var result = DiceManager.RollDice(DiceToUse);
                RunDCCheck(result);
            }
            else
            {
                RunNoDCCheck();
            }
        }
    }
}
