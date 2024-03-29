﻿using System;
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
using System.IO;
using GameMaps;

namespace Сукобана
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Box> Boxes = new List<Box>();
        GameObject player;
        CellMapInfo mapInf = new CellMapInfo(30, 20, 45, 0);
        static public UniversalMap_Wpf map;
        char[,] Smap = new char[30, 20];

        public MainWindow()
        {
            InitializeComponent();

            map = MapCreator.GetUniversalMap(this, mapInf);
            GridMap.Children.Add(map.Canvas);
            map.DrawGrid();
            AddPictures();
            map.Keyboard.SetSingleKeyEventHandler(CheckKey);

            Smap = ReadMap(@"C:\Users\Admin\Documents\GitHub\GameProject\Сукобана\Сукобана\maps\map Test.txt");
            DrawMap();
        }

        void AddPictures()
        {
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            map.Library.AddPicture("wall", "wall.png");
            map.Library.AddPicture("box", "stone.png");
            map.Library.AddPicture("player", "enemy.png");
            map.Library.AddPicture("finish", "Безымянный.png");
        }

        void DrawMap()
        {
            for(int y = 0; y < Smap.GetLength(1); y++)
            {
                for (int x = 0; x < Smap.GetLength(0); x++)
                {
                    switch(Smap[x, y])
                    {
                        case '*':
                            map.DrawInCell("wall", x, y);
                            break;

                        case '@':
                            map.DrawInCell("box", x, y);
                            Smap[x, y] = ' ';
                            break;

                        case '+':
                            map.DrawInCell("player", x, y);
                            player = new GameObject("player", x, y);
                            break;

                        case '!':
                            map.DrawInCell("finish", x, y);
                            break;
                    }
                }
            }
        }

        void CheckKey(Key k)
        {
            Box Found = null;
            int x = player.X;
            int y = player.Y;

            switch (k)
            {
                case Key.W:
                    y = player.Y - 1;
                    Found = GetBox(x, y);
                    if (Found == null)
                    {
                        player.Move(x, y);
                    }
                    else
                    {
                        if (Smap[x, y - 1] == ' ' || Smap[x, y - 1] == '!')
                        {
                            player.Move(x, y);
                            map.RemoveFromCell("box", x, y);
                            map.DrawInCell("box", x, y - 1);
                            Found.Y = y - 1;
                            
                            if(Smap[x, y - 1] == '!')
                            {
                                Found.IsOnPlace = true;
                            }
                            else
                            {
                                Found.IsOnPlace = false;
                            }
                        }
                    }
                    break;

                case Key.S:
                    y = player.Y + 1;
                    Found = GetBox(x, y);
                    if (Found == null)
                    {
                        player.Move(x, y);
                    }
                    else
                    {
                        if (Smap[x, y + 1] == ' ' || Smap[x, y + 1] == '!')
                        {
                            player.Move(x, y);
                            map.RemoveFromCell("box", x, y);
                            map.DrawInCell("box", x, y + 1);
                            Found.Y = y + 1;

                            if (Smap[x, y + 1] == '!')
                            {
                                Found.IsOnPlace = true;
                            }
                            else
                            {
                                Found.IsOnPlace = false;
                            }
                        }
                    }
                    break;

                case Key.D:
                    x = player.X + 1;
                    Found = GetBox(x, y);
                    if (Found == null)
                    {
                        player.Move(x, y);
                    }
                    else
                    {
                        if (Smap[x + 1,y] == ' ' || Smap[x + 1, y] == '!')
                        {
                            player.Move(x, y);
                            map.RemoveFromCell("box", x, y);
                            map.DrawInCell("box", x + 1, y);
                            Found.X = x + 1;

                            if (Smap[x + 1, y] == '!')
                            {
                                Found.IsOnPlace = true;
                            }
                            else
                            {
                                Found.IsOnPlace = false;
                            }
                        }
                    }

                    break;

                case Key.A:
                    x = player.X - 1;
                    Found = GetBox(x, y);
                    if (Found == null)
                    {
                        player.Move(x, y);
                    }
                    else
                    {
                        if (Smap[x - 1, y] == ' ' || Smap[x - 1, y] == '!')
                        {
                            player.Move(x, y);
                            map.RemoveFromCell("box", x, y);
                            map.DrawInCell("box", x - 1, y);
                            Found.X = x - 1;

                            if(Smap[x - 1, y] == '!')
                            {
                                Found.IsOnPlace = true;
                            }
                            else
                            {
                                Found.IsOnPlace = false;
                            }
                        }
                    }
                    break;
            }
            CheckVictori();

        }

        void CheckVictori()
        {
            bool IsWin = true;
            for(int i = 0; i < Boxes.Count; i++)
            {
                if(!Boxes[i].IsOnPlace)
                {
                    IsWin = false;
                    break;
                }
            }

            if(IsWin)
            {
                MessageBox.Show("Ура победа");
            }

        }

        Box GetBox(int x, int y)
        {
            Box box;
            box = null;
            for(int i = 0; i < Boxes.Count; i++)
            {
                if(Boxes[i].X == x && Boxes[i].Y == y)
                {
                    box = Boxes[i];
                }
            }

            return box;
        }



        char[,] ReadMap(string path)
        {
            int CountBoxes = 0;
            char[,] Nmap = new char[30, 20];
            string[] str = File.ReadAllLines(path);

            for(int i = 0; i < str.Length; i++)
            {
                for(int j = 0; j < str[i].Length; j++)
                {
                    Nmap[j, i] = str[i][j];

                    if(Nmap[j, i] == '@')
                    {
                        Box box = new Box();
                        box.X = j;
                        box.Y = i;
                        box.IsOnPlace = false;
                        Boxes.Add(box);
                    }
                }
            }

            return Nmap;
        }
    }
}
