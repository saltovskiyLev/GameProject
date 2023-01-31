using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using GameMaps;

namespace Морской_Бой
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random r = new Random();
        CellMapInfo mapInfo = new CellMapInfo(10, 10, 50, 0);
        UniversalMap_Wpf mapPL1;
        UniversalMap_Wpf mapPL2;
        int[,] map1 = new int[10, 10];
        int[,] map2 = new int[10, 10];

        public MainWindow()
        {
            InitializeComponent();
            mapPL1 = MapCreator.GetUniversalMap(this, mapInfo);
            mapPL2 = MapCreator.GetUniversalMap(this, mapInfo);
            GridMap.Children.Add(mapPL1.Canvas);
            GridMap.Children.Add(mapPL2.Canvas);
            Grid.SetColumn(mapPL1.Canvas, 0);
            Grid.SetColumn(mapPL2.Canvas, 2);
            AddPictures();

            mapPL1.Canvas.HorizontalAlignment = HorizontalAlignment.Right;

            mapPL1.DrawGrid();
            mapPL2.DrawGrid();

            map1 = CreateMap();
            map2 = CreateMap();
        }

        void AddPictures()
        {
            mapPL1.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            mapPL1.Library.AddPicture("ship", "ship.png");
            mapPL1.Library.AddPicture("DestroyedShip", "DestroesShip.png");
            mapPL1.Library.AddPicture("miss", "miss.png");

            mapPL2.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            mapPL2.Library.AddPicture("ship", "ship.png");
            mapPL2.Library.AddPicture("DestroyedShip", "DestroesShip.png");
            mapPL2.Library.AddPicture("miss", "miss.png");

        }


        int[,] CreateMap()
        {
            int[,] map = new int[10, 10];
            SetShip(map, 4);
            SetShip(map, 3);
            SetShip(map, 3);
            SetShip(map, 2);
            SetShip(map, 2);
            SetShip(map, 2);
            SetShip(map, 1);
            SetShip(map, 1);
            SetShip(map, 1);
            SetShip(map, 1);

            return map;
        }

        void DrawMap(int[,] map, UniversalMap_Wpf MapVisial)
        {
            for (int i = 0; i < map.GetLength(1); i++)
            {
                for (int j = 0; j < map.GetLength(0); j++)
                {
                    if (map[j, i] == 1)
                    {
                        mapPL1.DrawInCell("ship", j, i);
                    }
                    else if(map[j, i] == 2)
                    {
                        mapPL1.DrawInCell("DestroyedShip", j, i);
                    }
                    else
                    {
                        mapPL1.DrawInCell("miss", j, i);
                    }
                }
            }
        }

        void SetShip(int[,] map, int Length)
        {
            int count = 0;

            int dX = 0;
            int dY = 0;
            //Должен быть объявлен генератор случайныъ чисел на уровне класса
            // Нужно объявить две целочисленных переменных
            // Записаит в них случайные значения
            int x;
            int y;

            while (true)
            {
                count++;
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
            for (int i = 0; i < Length; i++)
            {
                map[x, y] = 1;
                x += dX;
                y += dY;
            }
            Debug.WriteLine(count);
        }
    }
}
