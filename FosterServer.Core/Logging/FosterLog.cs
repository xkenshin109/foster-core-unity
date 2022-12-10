using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FosterServer.Core.Logging
{
    public static class FosterLog
    {
        public static void Log(string a_message)
        {
            try
            {
                Debug.Log(a_message);
            }catch(Exception ie)
            {
                Console.WriteLine(ie.Message);
            }
            
        }

        public static void Error(string a_message)
        {
            try
            {
                Debug.LogError(a_message);
            }catch(Exception ie)
            {
                Console.WriteLine(ie.Message);
            }
            
        }
    }
}
