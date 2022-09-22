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
using GameMaps.Layouts;

namespace SimpleLabyrinth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static public UniversalMap_Wpf map;
        CellMapInfo mapInfo = new CellMapInfo(20, 15, 40, 0);
        GameObject Player;
        char[,] Cells;
        Dictionary<string, int> Items = new Dictionary<string, int>();
        InventoryPanel ipan;
        IGameScreenLayout lay;
        bool l1, l2, l3;
        bool isDebug = true;

        public MainWindow()
        {
            InitializeComponent();

            int x = 1, y = 5, z = 6;

            bool b1 = (x > 2 || x < 0) && y > 0 || !((z + y) == 10);
            bool b2 = !(x == 2 && y > 0) || y > 0 && z < 0;
            bool b3 = (!b1 || b2) && !(y > 12 || !(x - y == 4));

            lay = LayoutsFactory.GetLayout(LayoutType.Vertical, mContainer);
            map = MapCreator.GetUniversalMap(this, mapInfo);
            map.SetGridColor(Brushes.RosyBrown);
            lay.Attach(map, 0);
            //map.DrawGrid();
            try
            {
                Cells = GetMap(@"..\\..\\map.txt");
            }
            catch
            {
                Cells = GetMap("map.txt");
                isDebug = false;
            }
            AddPictures();
            map.Keyboard.SetSingleKeyEventHandler(CheckKey);
            GameObject.map = map;
            DrawMap(Cells);
            ipan = new InventoryPanel(map.Library, 40);
            lay.Attach(ipan, 1);
            ipan.AddItem("gem", "gem", "0");
            Items.Add("gem", 0);     

            for (int i = 0; i < map.XCells; i++)
            {
                for (int j = 0; j < map.YCells; j++)
                {
                    map.DrawInCell("unknown", i, j);
                }
            }
            RemoveFog();

        }
        void AddPictures()
        {
            if (isDebug) map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            else map.Library.ImagesFolder = new PathInfo { Path = "\\images", Type = PathType.Relative };
            
            map.Library.AddPicture("stone", "stone.png");
            map.Library.AddPicture("gem", "gem_red.png");
            map.Library.AddPicture("wall", "wall.png");
            map.Library.AddPicture("pick", "pick.png");
            map.Library.AddPicture("box", "box.png");
            map.Library.AddPicture("player", "smile1.png");
            map.Library.AddPicture("lever_up", "lever_up.png");
            map.Library.AddPicture("lever_down", "lever_down.png");
            map.Library.AddPicture("gate_closed", "gate_closed.png");
            map.Library.AddPicture("gate_opened", "gate_opened.png");
            map.Library.AddPicture("portal", "portal.png");
            map.Library.AddPicture("quest_hut", "quest_hut.png");
            map.Library.AddPicture("nothing", "nothing.png");
            map.Library.AddPicture("key", "key.png");
            map.Library.AddPicture("unknown", "unknown.png");
            map.Library.AddPicture("rip", "rip1.png");

            AnimationDefinition a = new AnimationDefinition();
            for (int i = 1; i < 11; i++)
            {
                map.Library.AddPicture("exp" + i, "exp" + i + ".png");
                a.AddFrame(100, "exp" + i);
            }
            a.AddFrame(1, "nothing");
            a.LastFrame = "nothing";
            map.Library.AddAnimation("exp", a);

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
            switch (Cells[x, y])
            {
                case 'G':
                    ItemName = "gem";
                    break;
                case 'P':
                    ItemName = "pick";
                    break;
            }
            if (ItemName == "") return;
            map.RemoveFromCell(ItemName, x, y);
            Cells[x, y] = ' ';
            if (Items.Keys.Contains(ItemName))
            {
                Items[ItemName]++;
                ipan.SetText(ItemName, Items[ItemName].ToString());
            }
            else
            {
                Items.Add(ItemName, 1);
                ipan.AddItem(ItemName, ItemName, "1");
            }
        }

        void DrawMap(char[,] cells)
        {
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    switch (cells[x, y])
                    {
                        case '*':
                            map.DrawInCell("stone", x, y);
                            break;
                        case 'W':
                            map.DrawInCell("wall", x, y);
                            break;
                        case 'Y':
                            Player = new GameObject("player", x, y);
                            break;
                        case 'G':
                            map.DrawInCell("gem", x, y);
                            break;
                        case 'B':
                            map.DrawInCell("box", x, y);
                            break;
                        case 'T':
                            map.DrawInCell("box", x, y);
                            break;
                        case 'D':
                            map.DrawInCell("gate_closed", x, y);
                            break;
                        case 'E':
                            map.DrawInCell("portal", x, y);
                            break;
                        case 'L':
                            map.DrawInCell("lever_up", x, y);
                            break;
                        case 'M':
                            map.DrawInCell("lever_up", x, y);
                            break;
                        case 'N':
                            map.DrawInCell("lever_up", x, y);
                            break;
                        case 'P':
                            map.DrawInCell("pick", x, y);
                            break;
                        case 'Q':
                            map.DrawInCell("quest_hut", x, y);
                            break;
                    }
                }
            }
        }

        /*
         *  * камень
            W стена
            T ящик ловушка
            B ящик с рандомным сокровищем
            G кристалл
            Y игрок (старт)
            D дверь1
            E выход
            # решетка, открывающаяся рычагом
            K ключ
            LMN рычаг
            Q квестхат, продающий ключ
            P кирка
         */

        void CheckKey(Key k)
        {
            int nx = Player.X, ny = Player.Y;
            switch (k)
            {
                case Key.Up:
                    ny = Player.Y - 1;
                    break;
                case Key.Down:
                    ny = Player.Y + 1;
                    break;
                case Key.Left:
                    nx = Player.X - 1;
                    break;
                case Key.Right:
                    nx = Player.X + 1;
                    break;
                case Key.Space:
                    if (Cells[nx, ny] == 'L')
                    {
                        l1 = !l1;
                        if (l1)
                        {
                            map.RemoveFromCell("lever_up", nx, ny);
                            map.DrawInCell("lever_down", nx, ny);
                        }
                        else
                        {
                            map.RemoveFromCell("lever_down", nx, ny);
                            map.DrawInCell("lever_up", nx, ny);
                        }
                    }
                    if (Cells[nx, ny] == 'M')
                    {
                        l2 = !l2;
                        if (l2)
                        {
                            map.RemoveFromCell("lever_up", nx, ny);
                            map.DrawInCell("lever_down", nx, ny);
                        }
                        else
                        {
                            map.RemoveFromCell("lever_down", nx, ny);
                            map.DrawInCell("lever_up", nx, ny);
                        }
                    }
                    if (Cells[nx, ny] == 'N')
                    {
                        l3 = !l3;
                        if (l3)
                        {
                            map.RemoveFromCell("lever_up", nx, ny);
                            map.DrawInCell("lever_down", nx, ny);
                        }
                        else
                        {
                            map.RemoveFromCell("lever_down", nx, ny);
                            map.DrawInCell("lever_up", nx, ny);
                        }
                    }
                    if (l1 && !l2 && l3)
                    {
                        map.RemoveFromCell("gate_closed", 17, 6);
                        map.DrawInCell("gate_opened", 17, 6);
                        Cells[17, 6] = ' ';
                    }
                    if(Cells[nx, ny] == 'Q')
                    {
                        if(!Items.Keys.Contains("gem") || Items["gem"] < 7)
                        {
                            MessageBox.Show("Чтобы получить ключ, вы должны отдать 7 кристаллов.");
                        }
                        else
                        {
                            MessageBox.Show("Вы обменяли 7 кристаллов на ключ.");
                            Items["gem"] -= 7;
                            ipan.SetText("gem", Items["gem"].ToString());
                            Items.Add("key", 1);
                            ipan.AddItem("key", "key");
                        }
                    }
                    break;
            }
            if (CheckPassability(nx, ny)) { Player.MoveTo(nx, ny);
                RemoveFog();
            }
            CheckDestroy(nx, ny);
            CheckExitDoor(nx, ny);
            Collect(Player.X, Player.Y);
            CheckVictory(nx, ny);
        }

        void RemoveFog()
        {
            for(int i = Player.X - 1; i <= Player.X + 1; i++)
            {
                for (int j = Player.Y - 1; j <= Player.Y + 1; j++)
                {
                    map.RemoveFromCell("unknown", i, j);
                }
            }
        }

        void CheckExitDoor(int x, int y)
        {
            if (x == 1 && y == 12 && Items.ContainsKey("key"))
            {
                map.RemoveFromCell("gate_closed", 1, 12);
                map.DrawInCell("gate_opened", 1, 12);
                Cells[1, 12] = ' ';
            }
        }
        void CheckVictory(int x, int y)
        {
            if (Cells[x,y] == 'E')
            {
                MessageBox.Show("Вы выбрались!");
                this.Close();
            }
        }

        void CheckDestroy(int x, int y)
        {
            bool isRemoved = false;
            if (!Items.ContainsKey("pick")) return;
            if (Cells[x, y] == 'W') { map.RemoveFromCell("wall", x, y); isRemoved = true; }
            if (Cells[x, y] == 'B')
            {
                map.RemoveFromCell("box", x, y);
                isRemoved = true;
                map.DrawInCell("gem", x, y);
                Cells[x, y] = 'G';
            }
            if (Cells[x, y] == 'T')
            {
                map.RemoveFromCell("box", x, y);
                map.AnimationInCell("exp", x, y, 1);
                map.DrawInCell("rip", Player.X, Player.Y);
                map.Keyboard.DisableSingleKeyEventHandler();
                isRemoved = true;
            }
            if(isRemoved && Cells[x,y] != 'G') Cells[x, y] = ' ';
        }

        bool CheckPassability(int x, int y)
        {
            if (Cells[x, y] == '*' ||
                Cells[x, y] == 'W' ||
                Cells[x, y] == 'D' ||
                Cells[x, y] == 'B' ||
                Cells[x, y] == 'T')
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
