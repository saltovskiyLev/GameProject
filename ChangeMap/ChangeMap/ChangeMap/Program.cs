using System;
using System.Threading;

namespace ChangeMap
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Thread.Sleep(200);
                    Console.SetCursorPosition(i, j);
                    Console.Write("*");
                }
            }
        }
    }
}
