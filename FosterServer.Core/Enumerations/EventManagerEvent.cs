using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FosterServer.Core.Enumerations
{
    public enum EventManagerEvent
    {
        [Display(Name = "Entity Position")]
        Position = 0,
        [Display(Name = "Entity Created")]
        Created = 1,
        [Display(Name = "Entity Removed")]
        Removed = 2,
        [Display(Name = "Entity Action")]
        Action = 3
    }


}
