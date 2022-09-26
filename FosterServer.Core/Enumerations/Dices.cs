using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FosterServer.Core.Enumerations
{
    public enum Dices
    {
        [Display(Name = "No Dice Roll")]
        NoDice,
        [Display(Name = "4-Sided Dice")]
        D4 = 4,
        [Display(Name = "6-Sided Dice")]
        D6 = 6,
        [Display(Name = "8-Sided Dice")]
        D8 = 8,
        [Display(Name = "10-Sided Dice Percentage")]
        D10P = 10,
        [Display(Name = "10-Sided Dice")]
        D10 = 10,
        [Display(Name = "12-Sided Dice")]
        D12 = 10,
        [Display(Name = "20-Sided Dice")]
        D20 = 20
    }
}
