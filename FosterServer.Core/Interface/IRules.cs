﻿using FosterServer.Core.DataModels;
using FosterServer.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FosterServer.Core.Interface
{
    public interface IRules
    {
        string RuleName { get; set; }
        int Id { get; set; }
        ExecutionType ExecutionType { get; set; }
        Priority Priority { get; set; }

        Result<bool> Validate(object data);

        Result<bool> Execute(object data);
    }
}