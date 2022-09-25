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
            panelMap.Children.Add(map.Canvas);
            //map.DrawGrid();
            Key k = new Key();
            map.Keyboard.SetSingleKeyEventHandler(CheckKey);
            AddPictures();
            ReadScrolls(1);
            DrawMap();
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

        /*void M(Key k)
        {
            int x = Player.X;
            int y = Player.Y;
            switch (k)
            {
                case Key.G:
                    map.RemoveFromCell("money", x, y);
                    map.DrawInCell("down", x, y);
                    break;
            }
        }*/

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
                   
                case Key.G:
                    if (Cells[x, y] == 'W')
                    {
                        map.RemoveFromCell("lever_down", x, y);
                        map.DrawInCell("lever_up", x, y);
                    }
                    break;
            }
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
                            map.DrawInCell("lever_down", i, j);
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