using System;
using System.IO;
using System.Collections.Generic;

namespace labirint
{
    class Program
    {
        static char[,] Map;
        static void Main(string[] args)
        {
            Map = ReadMap(@"C:\Users\Admin\Documents\GitHub\GameProject\labirint\labirint\map.txt");
            Console.ReadKey();
        }

        static char[,] ReadMap(string path)
        {
            char[,] map = new char[10, 10];
            string[] str = File.ReadAllLines(path);
            for(int i = 0; i < str.Length; i++)
            {
                for(int j = 0; j < str[i].Length; j++)
                {
                    map[j, i] = str[i][j];
                    Console.SetCursorPosition(j, i);
                    Console.Write(map[j, i]);
                }
            }
            return map;
        }
    }
}
