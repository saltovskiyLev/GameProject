using System;
using System.IO;

namespace AdvinchersCalculator
{
    class Program
    {
        int A = 0;
        int B;
        int C;
        int D;
        int F;
        string name;

        static void Main(string[] args)
        {
            string[] des;
            des = ReadFile(@"C:\Users\Admin\Documents\GitHub\GameProject\AdvinchersCalculator\AdvinchersCalculator\описание.txt");
            string s = "123";
            int result;
            bool Isnumber;
            Isnumber = int.TryParse(s, out result);
            string[] Files = Directory.GetFiles(@"C:\");
            Console.ReadKey();

        }

        static string[] ReadFile(string path)
        {
            string[] file = File.ReadAllLines(path);
            for (int i = 0; i < file.Length; i++)
            {
                Console.WriteLine(file[i]);
                
            }
            return file;
        }

        static void CreateGame(string[] description)
        {
            /*
            int A = 0;
            int B = 0;
            int C = 0;
            int D = 0;
            string name = "";
            description[0] = name;
            
                for(int i = 0; i < 5; i++)
                {
                    if (i == 0)
                    {
                        Console.WriteLine(description[i]);
                    }

                    if (i == 1)
                    {
                        Console.WriteLine(description[i]);
                    }

                    if (i == 2)
                    {
                        Console.WriteLine(description[i]);
                    }

                    if (i == 3)
                    {
                        Console.WriteLine(description[i]);
                    }

                    if (i == 4)
                    {
                        Console.WriteLine(description[i]);
                    }
            
            }
        */
        }
    }
}
