using System;

namespace Eraze2DMap
{
    class Program
    {
        static void Main(string[] args)
        {
            Task1();
            Console.ReadKey();
        }
        static void Task1()
        {
            for(int y = 0; y < 4; y++)
            {
                for(int x = 3 - y; x <= y + 3; x++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write("*");
                    Console.SetCursorPosition(x, 6 - y);
                    Console.Write("*");
                }
            }
        }
    }
}
