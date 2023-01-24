using System;

namespace homevork
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] ccels = new string[11];
            int[] c = new int[11];

            int count = 0;

            int conuntCe = 1;

            for (int i = 5; i >= 5; i--)
            {
                for (int j = 5; j < c.Length; j++)
                {
                    ccels[i] = c[j].ToString();
                    Console.Write(ccels[i]);
                    count++;
                    if (count == 5)
                    {
                        Console.Write(c[i + conuntCe] + conuntCe);
                        conuntCe++;
                        for (int k = 0; k < 4; k++)
                        {
                            Console.Write("0");
                        }
                    }
                }
            }






            Console.ReadKey();
        }
    }
}
