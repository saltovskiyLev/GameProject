using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace labirint
{
    class Program
    {
        static char[,] Map;
        static void Main(string[] args)
        {
            Map = ReadMap(@"C:\Users\Admin\Documents\GitHub\GameProject\labirint\labirint\map.txt");
            Cell main = new Cell(8, 8);
            CheckFill(main);
            Console.ReadKey();
        }

        static void CheckFill(Cell cell)
        {
            for(int i = cell.X - 1; i <= cell.X + 1; i++)
            {
                for(int j = cell.Y - 1; j <= cell.Y + 1; j++)
                {
                    if(Map[i, j] == ' ')
                    {
                        Cell c = new Cell(i, j);
                        cell.children.Add(c);
                        Map[i, j] = 'y';
                        Console.SetCursorPosition(i, j);
                        Console.Write('y');
                        Thread.Sleep(500);
                    }
                }
            }

            for(int i = 0; i < cell.children.Count; i++)
            {
                CheckFill(cell.children[i]);
            }
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

    class Cell
    {
        public int X;
        public int Y;
        public List<Cell> children = new List<Cell>();

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
