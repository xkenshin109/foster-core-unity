using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FosterServer.Core.Models
{
    public class GameParameters
    {
        public string Name => this.GetType().Name;

        [Required(ErrorMessage ="Game Entity Missing")]
        public GameEntity Entity { get; set; }

        public bool IsValid
        {
            get
            {
                var valid = true;
                var props = this.GetType().GetProperties();
                foreach(var prop in props)
                {
                    if (Attribute.IsDefined(prop, typeof(RequiredAttribute)))
                    {
                        Type type = prop.PropertyType;
                        if (prop.GetValue(this) == null)
                        {
                            valid = false;
                        }
                    }
                }
                return valid;
            }
        }
    }
}
