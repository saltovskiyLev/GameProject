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
        UniversalMap_Wpf mapLeft;
        UniversalMap_Wpf mapRight;
        int Player = 1;
        bool IsWaiting = false;
        bool CanShoot = true;
        int[,] map1 = new int[10, 10];
        int[,] map2 = new int[10, 10];
        List<ShipCoordinate>[] coordinates1 = new List<ShipCoordinate>[10];
        List<ShipCoordinate>[] coordinates2 = new List<ShipCoordinate>[10];


        public MainWindow()
        {
            InitializeComponent();
            mapLeft = MapCreator.GetUniversalMap(this, mapInfo);
            mapRight = MapCreator.GetUniversalMap(this, mapInfo);
            GridMap.Children.Add(mapLeft.Canvas);
            GridMap.Children.Add(mapRight.Canvas);
            Grid.SetColumn(mapLeft.Canvas, 0);
            Grid.SetColumn(mapRight.Canvas, 2);
            AddPictures();

            mapLeft.Canvas.HorizontalAlignment = HorizontalAlignment.Right;

            mapLeft.DrawGrid();
            mapRight.DrawGrid();

            CreateMap(out map1, out coordinates1);
            CreateMap(out map2, out coordinates2);

            DrawMap(map1, mapLeft, false);
            DrawMap(map2, mapRight, true);

            mapLeft.Canvas.Tag = "123";

            mapRight.Mouse.SetMouseSingleLeftClickHandler(GetCellsShoot);

        }

        void AddPictures()
        {
            mapLeft.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            mapLeft.Library.AddPicture("ship", "ship.png");
            mapLeft.Library.AddPicture("DestroyedShip", "DestroesShip.png");
            mapLeft.Library.AddPicture("miss", "miss.png");

            mapRight.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            mapRight.Library.AddPicture("ship", "ship.png");
            mapRight.Library.AddPicture("DestroyedShip", "DestroesShip.png");
            mapRight.Library.AddPicture("miss", "miss.png");
            mapRight.Library.AddPicture("o", "o.png");

        }

        bool CheckShipDestroyed(int[,] map, List<ShipCoordinate>[] coordinates, int X, int Y, out int index)
        {
            bool IsShipDestroyed = true;
            index = -1;

            for(int i = 0; i < coordinates.Length; i++)
            {
                IsShipDestroyed = true;
                for (int j = 0; j < coordinates[i].Count; j++)
                {
                    if(coordinates[i][j].X == X && coordinates[i][j].Y == Y)
                    {
                        index = i;

                        coordinates[i][j].IsDestroyed = true;
                    }

                    if(coordinates[i][j].IsDestroyed == false)
                    {
                        IsShipDestroyed = false;
                    }

                }

                if(index >= 0)
                {
                    break;
                }
            }
            return IsShipDestroyed;
        }

        void GetCellsShoot(int X, int Y, int cX, int cY)
        {
            if (!CanShoot) return;
            int[,] map;
            List<ShipCoordinate>[] coordinates;
            if(Player == 1)
            {
                map = map2;
                coordinates = coordinates2;
            }
            else
            {
                map = map1;
                coordinates = coordinates1;
            }

            if (map[cX, cY] == 0)
            {
                map[cX, cY] = 3;
                mapRight.DrawInCell("miss", cX, cY);
                BTNnextTurn.IsEnabled = true;

                CanShoot = false;
            }

            if(map[cX, cY] == 1)
            {
                map[cX, cY] = 2;
                mapRight.DrawInCell("DestroyedShip", cX, cY);
                BTNnextTurn.IsEnabled = true;
                CanShoot = false;

                int index;
                bool IsDestroyed = CheckShipDestroyed(map, coordinates, cX, cY, out index);

                if(IsDestroyed)
                {
                    DrawShipBorder(coordinates[index]);
                    MessageBox.Show("...");
                }
            }
        }


        void CreateMap(out int[,] map, out List<ShipCoordinate>[] coordinates)
        {
            coordinates = new List<ShipCoordinate>[10];
            map = new int[10, 10];
            SetShip(map, 4, coordinates, 0);
            SetShip(map, 3, coordinates, 1);
            SetShip(map, 3, coordinates, 2);
            SetShip(map, 2, coordinates, 3);
            SetShip(map, 2, coordinates, 4);
            SetShip(map, 2, coordinates, 5);
            SetShip(map, 1, coordinates, 6);
            SetShip(map, 1, coordinates, 7);
            SetShip(map, 1, coordinates, 8);
            SetShip(map, 1, coordinates, 9);
        }

        void DrawMap(int[,] map, UniversalMap_Wpf MapVisial, bool IsEnemy)
        {
            for (int i = 0; i < map.GetLength(1); i++)
            {
                for (int j = 0; j < map.GetLength(0); j++)
                {
                    if (map[j, i] == 1 && !IsEnemy)
                    {
                        MapVisial.DrawInCell("ship", j, i);
                    }
                    else if(map[j, i] == 2)
                    {
                        MapVisial.DrawInCell("DestroyedShip", j, i);
                    }
                    else if(map[j, i] == 3)
                    {
                        MapVisial.DrawInCell("miss", j, i);
                    }
                }
            }
        }

        void DrawShipBorder(List<ShipCoordinate> coords)
        {
            for(int i = 0; i < coords.Count; i++)
            {
                for(int x = coords[i].X - 1; x <= coords[i].X + 1; x++)
                {
                    for (int y = coords[i].Y - 1; y <= coords[i].Y + 1; y++)
                    {

                        if(x < 0 || y < 0 || x > 9 || y > 9)
                        {
                            continue;
                        }
                        string[] images = mapRight.GetImagesInCell(x, y);

                        if(images.Length == 0)
                        {
                            mapRight.DrawInCell("miss", x, y);
                        }
                    }
                }
            }
        }

        void SetShip(int[,] map, int Length, List<ShipCoordinate>[] coordinates, int index)
        {
            coordinates[index] = new List<ShipCoordinate>();

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
                                if (i != 10 && j != 10 && i != -1 && j != -1 && map[i, j] != 0)
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
                                if (i != 10 && j != 10 && i != -1 && j != - 1 && map[i, j] != 0)
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
                ShipCoordinate c = new ShipCoordinate(x, y);
                coordinates[index].Add(c);
                map[x, y] = 1;
                x += dX;
                y += dY;
            }
            Debug.WriteLine(count);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(IsWaiting)
            {
                CanShoot = true;
                BTNnextTurn.Content = "Передать ход";
                BTNnextTurn.IsEnabled = false;
                if (Player == 1)
                {
                    DrawMap(map1, mapLeft, false);
                    DrawMap(map2, mapRight, true);

                }
                else
                {
                    DrawMap(map2, mapLeft, false);
                    DrawMap(map1, mapRight, true);
                }
            }
            else
            {
                BTNnextTurn.Content = "Сделать ход";
                mapLeft.RemoveAllImages();
                mapRight.RemoveAllImages();
                if(Player == 1)
                {
                    Player = 2;
                    TBTurn.Text = "Игрок 1";
                }
                else
                {
                    Player = 1;
                    TBTurn.Text = "Игрок 2";
                }

            }


            IsWaiting = !IsWaiting;
        }
    }
}
