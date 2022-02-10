using System;
using System.Collections.Generic;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> Hello = new List<string>();
            Hello.Add("Moskow");
            Hello.Add("PiterBurg");
            Hello.Add("PiterBurg");
            Hello.Add("Dubna");
            int iMOGUS = 0;
            while (iMOGUS < Hello.Count)
            {
                if(Hello[iMOGUS] == "PiterBurg")
                {
                    Hello.RemoveAt(iMOGUS);
                    iMOGUS--;
                }
                iMOGUS++;
            }
            Console.WriteLine("1000 - 7");
            Console.ReadKey();
        }
    }
}
