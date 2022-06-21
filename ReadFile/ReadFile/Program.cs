using System;
using System.IO;

namespace ReadFile
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] MapLines = GetLines(@"C:\Users\Admin\Documents\GitHub\GameProject\Квест2022\Документация\карта.txt");
            int MaxLength = GetMaxLength(MapLines);
            char[,] test = new char[MaxLength, MapLines.Length];
            Console.WriteLine("✅");
            TestPrintLine(MapLines[0]);
            Console.ReadKey();
        }
        static string [] GetLines(string FileName)
        {
            string[] Lines = File.ReadAllLines(FileName);
            for (int i = 0; i < Lines.Length; i++)
            {
                Console.WriteLine(Lines[i]);
            }
            return Lines;
        }
        static int GetMaxLength(string[] map)
        {
            int max = 0;
            for(int i = 0; i < map.Length; i++)
            {
                if(map[i].Length > max)
                {
                    max = map[i].Length;
                }
            }
            return max;
        }

        static void TestPrintLine(string s)
        {
            char[] str = new char[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                str[i] = s[i];
                Console.WriteLine(s[i]);
            }
            str[0] = 'O';
        }
        static char[,] GetMap(string FileName)
        {
            string[] MapLines = GetLines(FileName);
            int MaxLength = GetMaxLength(MapLines);
            char[,] map = new char[MaxLength, MapLines.Length];
            for(int i = 0; i < MapLines.Length; i++)
            {
                for(int j = 0; j < MapLines[i].Length; j++)
                {
                    map[j, i] = MapLines[i][j];
                }
            }
            return map;
        }
    }
}
