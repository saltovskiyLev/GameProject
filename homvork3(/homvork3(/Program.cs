using System;

namespace homvork3_
{
    class Program
    {
        static Random r = new Random();

        static void Main(string[] args)
        {
            int[,] cells = new int[7, 7];


            for(int y = 0; y < 7; y++)
            {
                for (int x = y; x < 7; x++)
                {
                    cells[x, y] = y + 1;
                    cells[y, x] = y + 1;
                    //Console.SetCursorPosition(x, y);
                    //Console.Write(y + 1);

                    //Console.SetCursorPosition(y, x);
                    //Console.Write(y + 1);
                }

            }

            RandomizeMap(cells);
            PrintArray(cells);
            Console.ReadKey();
        }

        static void PrintArray(int[,] integer)
        {
            for(int y = 0; y < integer.GetLength(0); y++)
            {
                for(int x = 0; x < integer.GetLength(1); x++)
                {
                    int t = integer[x, y];
                    Console.SetCursorPosition(x, y);
                    Console.Write(integer[x, y]);
                }
            }
        }

        static int RandomizeMap(int[,] Map)
        {
            int result = 0;

            int random = r.Next(0, 100);

            if (random < 50)
            {
                result = 0;
            }
            else if (random >= 50 && random < 75)
            {
                result = 1;
            }
            else
            {
                result = -1;
            }

            return result;
        }
    }
}
