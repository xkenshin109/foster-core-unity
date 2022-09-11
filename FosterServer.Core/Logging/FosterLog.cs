using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FosterServer.Core.Logging
{
    public static class FosterLog
    {
        public static void Log(string a_message)
        {
            Debug.Log(a_message);
        }

        public static void Error(string a_message)
        {
            Debug.LogError(a_message);
        }
    }
}
