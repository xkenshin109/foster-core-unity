using FosterServer.Core.DataModels;
using FosterServer.Core.Enumerations;
using FosterServer.Core.Interface;
using FosterServer.Core.Logging;
using FosterServer.Core.Manager;
using FosterServer.Core.Models;
using FosterServer.UnityCore.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FosterServer.UnityCore.Models
{
    public abstract class AbilityCommand : MonoBehaviour, ICommandAction
    {
        private GameRule m_rule;
        public GameRule Rule
        {
            get
            {
                if (m_rule == null)
                {
                    FosterLog.Error("Game Rule has not been assigned");
                    return null;
                }
                return m_rule;
            }
            set
            {
                if (m_rule == null)
                {
                    m_rule = value;
                }
            }
        }

        public void Awake()
        {

        }
        public abstract void Execute();

    }
}
