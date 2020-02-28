using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace массивы
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] cites = new string[10];
            cites[0] = "Madrid";
            cites[1] = "Moscow";
            cites[2] = "Dubna";
            cites[3] = "Berlin";
            cites[4] = "Sakura";
            for (int i = 0; i < cites.Length; i++)
            {
               Console.WriteLine(cites[i]);
            }
            Console.ReadKey();
        }
    }
}
