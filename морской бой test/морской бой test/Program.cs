using System;
using System.Threading;

namespace морской_бой_test
{
    class Program
    {
        static Random r = new Random();
        static void Main(string[] args)
        {
            int[,] map = new int[10, 10];

            map[0, 2] = 1;
            map[1, 2] = 1;
            map[2, 2] = 1;
            SetShip(map, 4);
            SetShip(map, 3);
            SetShip(map, 3);
            SetShip(map, 2);
            SetShip(map, 2);
            SetShip(map, 2);
            //SetShip(map, 1);
            //SetShip(map, 1);
            //SetShip(map, 1);
            //SetShip(map, 1);

            DrawMap(map);
            Console.ReadKey();
        }

        static void DrawMap(int[,] map)
        {
            int MaxX = map.GetLength(1);
            int MaxY = map.GetLength(0);
            for(int i = 0; i < map.GetLength(1); i++)
            {
                for(int j = 0; j < map.GetLength(0); j++)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write(map[j, i]);
                }
            }
        }

        static void SetShip(int[,] map, int Length)
        {
            int dX = 0;
            int dY = 0;
            //Должен быть объявлен генератор случайныъ чисел на уровне класса
            // Нужно объявить две целочисленных переменных
            // Записаит в них случайные значения
            int x;
            int y;

            while(true)
            {
                try
                {

                
                while (true)
                {
                    x = r.Next(0, map.GetLength(1));
                    y = r.Next(0, map.GetLength(0));

                    if (map[x, y] == 0)
                    {
                        break;
                    }
                }

                    dX = 0;
                    dY = 0;
                    if (r.Next(0, 100) > 50)
                    {
                        //Горизонтальный корабль dX
                        /*if (r.Next(0, 100) > 50)
                        {
                            //Рисуем вправо
                            dX = 1;
                        }
                        else
                        {
                            dX = -1;
                        }*/
                        dX = 1;
                        bool IsOkey = true;

                        for (int i = x - 1; i <= x + Length + 1; i++)
                        {
                            for (int j = y - 1; j <= y + 1; j++)
                            {
                                if (map[i, j] != 0)
                                {
                                    IsOkey = false;
                                }
                            }
                        }

                        if (IsOkey) break;
                    }

                    else
                    {
                        //Вертикальный корабль
                        /*if (r.Next(0, 100) > 50)
                        {
                            dY = 1;
                        }

                        else
                        {
                            dY = -1;
                        }*/
                        dY = 1;

                        bool IsOkey = true;

                        for (int i = x - 1; i <= x + 1; i++)
                        {
                            for (int j = y - 1; j <= y + 1 + Length; j++)
                            {
                                if (map[i, j] != 0)
                                {
                                    IsOkey = false;
                                }
                            }
                        }

                        if (IsOkey) break;
                    }
                }
                catch
                {

                }
            }
            for(int i = 0; i < Length; i++)
            {
                map[x, y] = 2;
                x += dX;
                y += dY;
            }
        }
    }
}

