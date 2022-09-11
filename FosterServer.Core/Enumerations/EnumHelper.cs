using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FosterServer.Core.Enumerations
{
    public static class EnumHelper
    {
        /// <summary>
        ///     A generic extension method that aids in reflecting 
        ///     and retrieving any attribute that is applied to an `Enum`.
        /// </summary>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
                where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }

        /// <summary>
        /// Generic Enum Extension method to return name
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string Name(this Enum enumValue)
        {
            return enumValue.ToString();
        }
    }
}
