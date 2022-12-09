﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace FosterServer.Core.Models
{
    public class EntityModel : GameParameters
    {
        public GameEntity Entity { get; set; }
    }

    [System.Serializable]
    public class EventManagerParameter : UnityEvent<GameParameters>
    {

    }
}
