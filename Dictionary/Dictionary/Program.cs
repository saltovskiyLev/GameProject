using System;
using System.Collections.Generic;

namespace Dictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Dictionary<string, int> cities = new Dictionary<string, int>();
            cities.Add("Moscow", 12000000);
            cities.Add("Dubna", 60000);
            Console.WriteLine(cities["Moscow"]);
            cities["Dubna"] += 5000;
            Console.WriteLine(cities["Dubna"]);
            if (cities.ContainsKey("Obninsk")) 
            {
                Console.WriteLine(cities["Obninsk"]);
            }

            Console.ReadKey();
        }
    }
}
