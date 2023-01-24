using System;

namespace homevork_2
{
    class Program
    {
        static void Main(string[] args)
        {

            int MaxX = 11;
                
            for(int i = 0; i < MaxX; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("0");

                for(int j = 0; j < MaxX; j++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write("0");
                }
            }


//Решение задачи

            for(int i = 0; i < MaxX; i++)
            { 
                for(int j = 5 - i; j <= 5 + i; j++)
                {
                    Console.SetCursorPosition();
                }
            }







            //int[] massive_down = new int[12];
            //int[] massive_up = new int[11];
            //int position = 5;
            //int c = 1;
            //int count = 0;


            //for(int i = 0; i < 12; i++)
            //{
            //    massive_down[i] = 0;
            //    console.write(massive_down[i].tostring());
            //    console.setcursorposition(position, count);
            //    console.write(c.tostring());
            //}


            //for (int i = 0; i < 11; i++)
            //{
            //    console.setcursorposition(count, i);
            //    console.write(massive_up[i]);

            //    console.setcursorposition(count, position);
            //    console.write(c.tostring());

            //    if(i == 10)
            //    {
            //        count++;
            //        position++;
            //        c = c + 5;

            //        for (int j = 0; j < 3; j++)
            //        {
            //            console.setcursorposition(count, position - j);
            //            console.write(c.tostring());
            //        }

                    
            
            //    }
            //}

            

            Console.ReadKey();
        }
    }
}
