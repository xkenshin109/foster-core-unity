using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FosterServerClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            FosterClient client = new FosterClient();
            //client.Client.StartUp();
            
            Console.ReadKey();
        }
    }
}
