using System;

namespace TestКвесе
{
    class Program
    {
        static void Main(string[] args)
        {
            int x = 1;
            int y = 2;
            //Console.WriteLine("{0} {1}", y, x);
            for(int i = 0; i < args.Length; i++)
            {
                Console.WriteLine("{0} {1}", i, args[i]);
            }
            Console.ReadKey();
        }
    }
}
