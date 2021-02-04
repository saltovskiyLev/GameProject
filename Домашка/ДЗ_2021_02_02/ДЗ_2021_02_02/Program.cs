using System;

namespace ДЗ_2021_02_02
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int x = 3, y = 5;
            int z = x + y;
            for (int i = 1; i <= 10; i++)
            {
                z = z + i;
            }
            Console.WriteLine(z);
            Console.ReadKey();
        }
    }
}
