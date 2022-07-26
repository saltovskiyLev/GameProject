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

namespace Квест_2022
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static public UniversalMap_Wpf map;
        CellMapInfo mapInfo = new CellMapInfo(200, 200, 30, 0);
        GameObject Player;
        char[,] Cells;
        Dictionary<string, int> Items = new Dictionary<string, int>();
        public MainWindow()
        {
            InitializeComponent();
            map = MapCreator.GetUniversalMap(this, mapInfo);
            MapContainer.Children.Add(map.Canvas);
            map.SetGridColor(Brushes.RosyBrown);
            //map.DrawGrid();
            Cells = GetMap(@"C:\Users\Admin\Documents\GitHub\GameProject\Квест2022\Документация\карта.txt");
            AddPictures();
            map.Keyboard.SetSingleKeyEventHandler(CheckKey);
            GameObject.map = map;
            DrawMap(Cells);
        }
        void AddPictures()
        {
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            map.Library.AddPicture("hero", "Hero.png");
            map.Library.AddPicture("money", "Money.png");
            map.Library.AddPicture("enemy", "enemy.png");
            map.Library.AddPicture("tree", "tree.png");
            map.Library.AddPicture("map", "map.jpg");
        }
        string[] GetLines(string FileName)
        {
            string[] Lines = File.ReadAllLines(FileName);
            for (int i = 0; i < Lines.Length; i++)
            {
                Console.WriteLine(Lines[i]);
            }
            return Lines;
        }

        int GetMaxLength(string[] map)
        {
            int max = 0;
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i].Length > max)
                {
                    max = map[i].Length;
                }
            }
            return max;
        }

        char[,] GetMap(string FileName)
        {
            string[] MapLines = GetLines(FileName);
            int MaxLength = GetMaxLength(MapLines);
            char[,] map = new char[MaxLength, MapLines.Length];
            for (int i = 0; i < MapLines.Length; i++)
            {
                for (int j = 0; j < MapLines[i].Length; j++)
                {
                    map[j, i] = MapLines[i][j];
                }
            }
            return map;
        }

        void Collect(int x, int y)
        {
            string ItemName = "";
            switch(Cells[x, y])
            {
                case '$':
                    map.RemoveFromCell("money", x, y);
                    ItemName = "coin";
                    break;
            }
            if (ItemName == "") return;
            if(Items.Keys.Contains(ItemName))
            {
                Items[ItemName]++;
            }
            else
            {
                Items.Add(ItemName, 1);
            }
        }

        void DrawMap(char[,] cells)
        {
            for(int x = 0; x < cells.GetLength(0); x++)
            {
                for(int y = 0; y < cells.GetLength(1); y++)
                {
                    switch(cells[x, y])
                    {
                        case '1':
                            map.DrawInCell("tree", x, y);
                            break;

                        case '*':
                            map.DrawInCell("enemy", x, y);
                            break;
                        case 'Y':
                            Player = new GameObject("hero", x, y);
                            break;
                        case '$':
                            map.DrawInCell("money", x, y);
                            break;
                    }
                }
            }
        }

        void CheckKey(Key k)
        {
            switch(k)
            {
                case Key.W:
                    if(CheckPassability(Player.X, Player.Y - 1)) Player.MoveTo(Player.X, Player.Y - 1);
                    break;

                case Key.S:
                    if (CheckPassability(Player.X, Player.Y + 1)) Player.MoveTo(Player.X, Player.Y + 1);
                    break;

                case Key.A:
                    if (CheckPassability(Player.X - 1, Player.Y)) Player.MoveTo(Player.X - 1, Player.Y);
                    break;

                case Key.D:
                    if (CheckPassability(Player.X + 1, Player.Y)) Player.MoveTo(Player.X + 1, Player.Y);

                    break;
            }
            Collect(Player.X, Player.Y);
        }

        bool CheckPassability(int x, int y)
        {
            if (Cells[x, y] == '1')
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
