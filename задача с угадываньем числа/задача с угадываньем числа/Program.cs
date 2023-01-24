using System;

namespace задача_с_угадываньем_числа
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int min = 0;
            int max = 100;
            int M = (min + max) / 2;


            while (true)
            {
                int middle = ((max + min) / 2);
                Console.WriteLine("Ваше число больше " + middle.ToString() + " y/n");
                string s = Console.ReadLine();

                if (s == "y")
                {
                    min = middle;
                    //Console.WriteLine()

                    if (max - min == 1)
                    {
                        if(max > M)
                        {
                            Console.WriteLine("Это ваше число: " + min.ToString());
                            break;
                        }
                        else
                        {

                            Console.WriteLine("Это ваше число: " + max.ToString());
                            break;
                        }

                    }
                }
                else
                {
                    max = middle;
                    if (max - min == 1)
                    {
                        if (max < M)
                        {
                            Console.WriteLine("Это ваше число: " + min.ToString());
                            break;
                        }
                        else
                        {

                            Console.WriteLine("Это ваше число: " + max.ToString());
                            break;
                        }
                    }
                }

                Console.WriteLine(min);
                Console.WriteLine(max);
            }

            Console.ReadKey();
        }
    }
}
