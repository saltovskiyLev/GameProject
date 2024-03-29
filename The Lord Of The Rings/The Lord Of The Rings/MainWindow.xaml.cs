﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using GameMaps;

namespace The_Lord_Of_The_Rings
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CellMapInfo MapInfo;
        static public UniversalMap_Wpf map;
        char[,] Cells;
        int cellsX;
        GameObject Player;
        Dictionary<string, int> Items = new Dictionary<string, int>();
        Dictionary<char, string> Scrolls = new Dictionary<char, string>();
        List<Gates> gates = new List<Gates>();
        List<Changer> changers;
        public MainWindow()
        {
            InitializeComponent();
            string path = @"C:\Users\Admin\Documents\GitHub\GameProject\The Lord Of The Rings\The Lord Of The Rings\карта.txt";
            string[] lines = File.ReadAllLines(path);
            cellsX = MaxLenght(lines);
            Cells = new char[cellsX, lines.Length];
            GetCharMap(lines);
            MapInfo = new CellMapInfo(cellsX, lines.Length, 30, 0);
            map = MapCreator.GetUniversalMap(this, MapInfo);
            GameObject.Map = map;
            changers = new List<Changer>();
            panelMap.Children.Add(map.Canvas);
            //map.DrawGrid();
            Key k = new Key();
            map.Keyboard.SetSingleKeyEventHandler(CheckKey);
            AddPictures();
            DrawMap();
            NewMoving();
            DrawFogMap();
            ReadScrolls(1);
            //DrawMap();
            ///////////////////////////////////////////////
        }

        void ReadScrolls(int number)
        {
            Scrolls.Clear();
            string[] str = File.ReadAllLines(@"C:\Users\Admin\Documents\GitHub\GameProject\The Lord Of The Rings\The Lord Of The Rings\scrolls" + number + ".txt");
            for (int i = 0; i < str.Length; i++)
            {
                string[] s = str[i].Split('|');
                Scrolls.Add(s[0][0], s[1]);
            }
        }


        void GetCharMap(string[] str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                for (int j = 0; j < str[i].Length; j++)
                {
                    Cells[j, i] = str[i][j];
                }
            }
        }

        string GetText()
        {
            string items = "";
            foreach (string Key in Items.Keys)
            {
                items += Key + ":" + Items[Key] + " ";
                // ПРОБЛЕМА при сборе нескольких предметов предыдущий записывается на его место.
                // тут подсмотрел КВЕСТ 2022
            }
            return items;
        }

        void DrawFogMap()
        {
            for (int x = 0; x < Cells.GetLength(0); x++)
            {
                for (int y = 0; y < Cells.GetLength(1); y++)
                {
                    map.DrawInCell("fog", x, y);
                }
            }
        }

        void Collect(int x, int y)
        {
            string ItemName = "";
            switch (Cells[x, y])
            {
                case '#':
                    map.RemoveFromCell("money", x, y);
                    ItemName = "money";
                    break;

                case '3':
                    map.RemoveFromCell("scroll", x, y);
                    ItemName = "scroll";
                    MessageBox.Show(Scrolls['!']);

                    // Тут вывод сообщения, пока можно хардкод, но хотели сделать с помощью JSON
                    break;
            }
            if (ItemName == "") return;
            if (Items.Keys.Contains(ItemName))
            {
                Items[ItemName]++;
            }
            else
            {
                Items.Add(ItemName, 1);
            }

            string text = GetText();
            ItemsPanel.Text = text;
            // ???
        }


        void NewMoving()
        {
            int x = Player.X;
            int y = Player.Y;

            map.RemoveFromCell("fog", x - 1, y);
            map.RemoveFromCell("fog", x + 1, y);
            map.RemoveFromCell("fog", x, y - 1);
            map.RemoveFromCell("fog", x, y + 1);
        }

        void CreateAnimathion(int numbers, string baseName)
        {
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            AnimationDefinition anim = new AnimationDefinition();
            string BaseNamepng;
            string Name;
            for(int i = 0; i <= numbers; i++)
            {
                BaseNamepng = baseName + i + ".png";
                Name = baseName + i;
                map.Library.AddPicture(Name, BaseNamepng);
                anim.AddFrame(100, Name);
            }

            map.Library.AddAnimation("exp", anim);
        }
        void CheckKey(Key k)
            {
            // Создаём переменные для новых координат
            // Если нажата кнопка движения, вычисляем новые координаты
            // Внутри swich игрока не перерисовываем
            // После swich проверяем отсутствие в клетке с новыми координатами непроходимых объектов
            int x = Player.X;
            int y = Player.Y;
            switch (k)
            {
                case Key.W:
                    y = Player.Y - 1;
                    break;

                case Key.S:
                    y = Player.Y + 1;
                    break;

                case Key.D:
                    x = Player.X + 1;
                    break;

                case Key.A:
                    x = Player.X - 1;
                    break;

                case Key.R:
                    map.AnimationInCell("exp", Player.X, Player.Y, 2);
                    break;

                case Key.Space:
                    if (Cells[x, y] == 'W')
                    {
                        for (int i = 0; i < changers.Count; i++)
                        {
                            if(changers[i].X == x && changers[i].Y == y)
                            {
                                if(changers[i].Open == false)
                                {
                                    map.RemoveFromCell("lever_down", x, y);
                                    map.DrawInCell("lever_up", x, y);
                                    //map.DrawInCell("up", x, y);
                                    for(int j = 0; j < gates.Count; j++)
                                    {
                                        map.RemoveFromCell("gate_closed", gates[j].X, gates[j].Y);
                                        map.DrawInCell("gate_opened", gates[j].X, gates[j].Y);
                                    }
                                    changers[i].Open = true;
                                }
                                else
                                {
                                    map.RemoveFromCell("lever_up", x, y);
                                    map.DrawInCell("lever_down", x, y);
                                    changers[i].Open = false;
                                    for (int j = 0; j < gates.Count; j++)
                                    {
                                        map.RemoveFromCell("gate_opened", gates[j].X, gates[j].Y);
                                        map.DrawInCell("gate_closed", gates[j].X, gates[j].Y);
                                    }
                                }
                            }
                        }

                    }
                    break;
            }
            NewMoving();
            if (Cells[x, y] != '1')
            {
                Player.Move(x, y);
            }
            Collect(x, y);

            if(Cells[x, y] == '#')
            {
                // Тут с графикой какие то беды.
            }
        }

        void AddPictures()
        {
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            map.Library.AddPicture("enemy", "enemy.png");
            map.Library.AddPicture("Frodo", "frodo.png");
            map.Library.AddPicture("tree", "tree.png");
            map.Library.AddPicture("money", "Монета_01.png");
            map.Library.AddPicture("scroll", "i.png");
            map.Library.AddPicture("down", "down.png");
            map.Library.AddPicture("up", "up.PNG");
            map.Library.AddPicture("lever_up", "lever_up.png");
            map.Library.AddPicture("lever_down", "lever_down.png");
            map.Library.AddPicture("fog", "unknown.png");
            map.Library.AddPicture("gate_closed", "gate_closed.png");
            map.Library.AddPicture("gate_opened", "gate_opened.png");
            CreateAnimathion(9, "exp");
        }
        void DrawMap()
        {
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    switch (Cells[i, j])
                    {
                        case '1':
                            map.DrawInCell("tree", i, j);
                            break;

                        case '#':
                            map.DrawInCell("money", i, j);
                            //Переключатель(i, j, Player.X, Player.Y);
                            break;

                        case '2':
                            Player = new GameObject(i, j, "Frodo");
                            break;

                        case '3':
                            map.DrawInCell("scroll", i, j);
                            break;

                        case 'W':
                            Changer lever = new Changer();
                            lever.X = i;
                            lever.Y = j;
                            changers.Add(lever);
                            map.DrawInCell("lever_down", i, j);
                            break;

                        case 'G':
                            Gates gate = new Gates();
                            gate.X = i;
                            gate.Y = j;
                            gates.Add(gate);
                            map.DrawInCell("gate_closed", i, j);
                            break;
                    }
                }
            }
        }
        int MaxLenght(string[] path)
        {
            int number = 0;
            int maxLenght = path[number].Length;
            for (int i = 0; i < path.Length; i++)
            {
                if (path[i].Length > maxLenght)
                {
                    maxLenght = path[i].Length;
                    number = i;
                }
            }
            return maxLenght;
        }
    }
}