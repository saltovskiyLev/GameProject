using System;
using System.Threading;

namespace coordinate
{
    class Program
    {
        static void Main(string[] args)
        {



            for(int y = 0; y <= 5; y++)
            {
                for (int x = 5 - y; x <= y + 5; x++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(y + 1);
                    Console.SetCursorPosition(x, 10 - y);
                    Console.Write(y + 1);


                    Thread.Sleep(200);
                }
            }
            Console.ReadKey();
        }
    }
}
