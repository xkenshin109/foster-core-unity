﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FosterServer.Core.Models
{
    public class GameParameters
    {
        public string Name => this.GetType().Name;
    }
}
