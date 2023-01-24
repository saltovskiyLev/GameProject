using System;
using System.Linq;

namespace сортировка
{
    class Program
    {
        static Random r = new Random();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int[] array = new int[r.Next(5, 1000000)];
            for(int i = 0; i < array.Length; i++)
            {
                array[i] = r.Next(0, 100);
            }

            //Array.Sort(array);
            Console.WriteLine(11 % 3);

            ShowArray(array);
            sort(array);
            Console.WriteLine("\n");
            ShowArray(array);
            Console.ReadKey();
        }

        static void ShowArray(int[] ar)
        {
            for(int i = 0; i < ar.Length; i++)
            {
                Console.Write(ar[i] + " ");
            }
            Console.WriteLine();
        }

        static void sort(int[] r)
        {
            int t;
            bool IsChanged = true;
            while(IsChanged)
            {
                IsChanged = false;
                for(int i = 0; i < r.Length - 1; i++)
                {
                    if(r[i] > r[i + 1])
                    {
                        t = r[i];
                        r[i] = r[i + 1];
                        r[i + 1] = t;
                        IsChanged = true;
                    }
                    else
                    {

                    }
                }
                ShowArray(r);
            }
        }
    }
}
