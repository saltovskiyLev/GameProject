using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace массивы
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test4();
            //Test3();
            //Test2();
            //Test();
            /*string[] cites = new string[10];
            cites[0] = "Madrid";
            cites[1] = "Moscow";
            cites[2] = "Dubna";
            cites[3] = "Berlin";
            cites[4] = "Sakura";
            for (int i = 0; i < cites.Length; i++)
            {
               Console.WriteLine(cites[i]);
            }*/
            Console.ReadKey();
        }
        static void Test()
        {
            int[] a = new int[4];
            Random r = new Random();
            for(int i = 0; i < a.Length; i++)
            {
                a[i] = r.Next(0,5);
                Console.WriteLine(a[i]);
            }
            int sum = 0;
            for(int j = 0; j < a.Length; j++)
            {
                sum = sum + a[j];
            }
            Console.WriteLine(sum);
        }
        static void Test2()
        {
            string s = "Hello World";
            for (int i = 0; i <  s.Length; i++)
            {
                Console.WriteLine(s[i]);
            }
        }
        static void Test3()
        {
            File.WriteAllText("Test.TXT", "Hello World  \n Im Leo");
        }
        static void Test4()
        {
           string[] s;
            s = File.ReadAllLines("Test.TXT");
            for (int i = 0; i < s.Length; i++)
            {
                Console.WriteLine(s[i]);
            }
        }
    }
}
