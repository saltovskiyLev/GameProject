using System;

namespace homevork3
{
    class Program
    {
        static void Main(string[] args)
        {

            int MaxLenght = 7;

            for(int y = 0; y < MaxLenght; y++)
            {
                for(int x = 0; x < MaxLenght; x++)
                {
                    /*Console.SetCursorPosition(y, x);*/
                    /*Console.SetCursorPosition(x, y);
                    Console.Write(y + 1);*/
                    Console.SetCursorPosition(x, y);
                    Console.Write(1);
                }
            }

            Console.ReadKey();
        }
    }
}
