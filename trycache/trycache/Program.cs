using System;

namespace trycache
{
    class Program
    {
        static void Main(string[] args)
        {
            int x = 2;
            int y = 0;

            try
            {
                int b = x / y;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }
    }
}
