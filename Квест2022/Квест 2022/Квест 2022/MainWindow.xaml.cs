using System;
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
using Newtonsoft.Json;
using GameMaps;
using LevJson;
using System.Net.Http.Headers;

namespace Квест_2022
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static public UniversalMap_Wpf map;
        CellMapInfo mapInfo = new CellMapInfo(500, 500, 30, 0);
        GameObject Player;
        char[,] Cells;
        int Level = 0;
        int PlayerLevel = 0;
        int PlayerExpereince = 0;
        string ResoursesFolderPath;
        List<LevJson.JsonGameObject> JsonMap;
        List<string> Maps = new List<string>();
        List<LevJson.JsonGameObject>[,] GameCells;
        List<Product> products = new List<Product>();
        Dictionary<char, string> Scrolls = new Dictionary<char, string>();
        Dictionary<string, int> Items = new Dictionary<string, int>();
        Dictionary<string, int> ShopItems;
        public MainWindow()
        {
            InitializeComponent();
            ResoursesFolderPath = GetResoursDirectory();
            map = MapCreator.GetUniversalMap(this, mapInfo);
            MapContainer.Children.Add(map.Canvas);
            map.SetGridColor(Brushes.RosyBrown);
            Items.Add("coin", 0);
            //map.DrawGrid();
            AddPictures();
            JsonMap = new List<JsonGameObject>();
            map.Keyboard.SetSingleKeyEventHandler(CheckKey);
            GameObject.map = map;
            Cells = GetMap(@"C:\Users\Admin\Documents\GitHub\GameProject\Квест2022\Квест 2022\Квест 2022\resourses\maps\001карта.txt");
            DrawMap(Cells);
            JsonMap = ReadJsonMap(ResoursesFolderPath + @"JsonMaps\MapJson.json");
            DrawJsonMap(JsonMap);
            CreateMapList();
            DrawNewMap(Maps[Level]);
        }

        void CreateGameCells(JsonMap Map)
        {
            GameCells = new List<JsonGameObject>[Map.XCells, Map.YCells];
            for(int i = 0; i < Map.XCells; i++)
            {
                for(int j = 0; j < Map.YCells; j++)
                {
                    GameCells[i, j] = new List<JsonGameObject>();
                }
            }

            for (int i = 0; i < Map.Objects.Count; i++)
            {
                var obj = Map.Objects[i];
                GameCells[obj.X, obj.Y].Add(obj);
            }
        }

        void CreateMapList()
        {
            Maps.Add(ResoursesFolderPath + @"maps\001карта.txt");
            Maps.Add(ResoursesFolderPath + @"maps\002карта.txt");
        }

        string GetResoursDirectory()
        {
            string path = Directory.GetCurrentDirectory();
            if(Directory.Exists(path + @"\resourses\"))
            {
                return path;
            }
            else
            {
                return path + @"\..\..\resourses\";
            }
        }
        List<JsonGameObject> ReadJsonMap(string path)
        {
            return JsonConvert.DeserializeObject<List<LevJson.JsonGameObject>>(File.ReadAllText(path));
        }

        void DrawJsonMap(List<LevJson.JsonGameObject> CellsObjects)
        {
            for (int i = 0; i < CellsObjects.Count; i++)
            {
                map.DrawInCell(CellsObjects[i].Name, CellsObjects[i].X, CellsObjects[i].Y);
            }
        }

        [Obsolete]
        void DrawNewMap(string FileName)
        {
            EraseMap();
            Cells = GetMap(FileName);
            DrawMap(Cells);
            ReadScrolls(Level + 1);
        }

        void CheckViktory()
        {
            if (Cells[Player.X, Player.Y] == 'W')
            {
                Level++;
                //DrawNewMap(Maps[Level]);
            }
        }

        /* Что нужно сделать чтобы перересовать карту для нового уровня?
         1) Стереть старую карту
         2) Получить из файла новую карту
         3) Отрисовать новую карту

         *** Что происходит с координатами игрока после перехода карты? ***
         
         */
        void AddPictures()
        {
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\resourses\\images", Type = PathType.Relative };
            //map.Library.ImagesFolder = new PathInfo { Path = ResoursesFolderPath + @"images\", Type = PathType.Absolute };
            map.Library.AddPicture("hero", "Hero.png");
            map.Library.AddPicture("money", "Money.png");
            map.Library.AddPicture("enemy", "enemy.png");
            map.Library.AddPicture("tree", "tree.png");
            map.Library.AddPicture("map", "map.jpg");
            map.Library.AddPicture("gate_opened", "gate_opened.png");
            map.Library.AddPicture("scroll", "i.png");
            map.Library.AddPicture("stone", "stone.png");
            map.Library.AddPicture("nothing", "nothing.png");
            map.Library.AddPicture("ЗАБОР", "ЗАБОР.png");
            map.Library.AddPicture("shop", "Shop.png");
            CreateAnimation("exp", 10);
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

        void EraseMap()
        {
            map.RemoveAllImages();
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

        void ReadScrolls(int MapNumber)
        {
            Scrolls.Clear();
            string[] s = File.ReadAllLines(ResoursesFolderPath + @"\scrolls\scrolls" + MapNumber + ".txt");

            for(int i = 0; i < s.Length; i++)
            {
                string[] str = s[i].Split('|');
                Scrolls.Add(str[0][0], str[1]);
            }
        }
        /// <summary>
        /// количество картинок начинается с нуля
        /// </summary>
        /// <param name="BaseName"></param>s
        /// <param name="count"></param>
        void CreateAnimation(string BaseName, int count)
        {
            AnimationDefinition a = new AnimationDefinition();
            string path;
            for (int i = 0; i < count; i++)
            {
                path = BaseName + i + ".png";
                map.Library.AddPicture(BaseName + i, path);
                a.AddFrame(100, BaseName + i);
            }
            a.AddFrame(1, "nothing");
            a.LastFrame = "nothing";
            map.Library.AddAnimation(BaseName, a);
        }
        string GetItemsText()
        {
            string Description = "";
            foreach (string key in Items.Keys)
            {
                Description += key + ": " + Items[key] + " ";

            }
            return Description;
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

        Dictionary<string, int> GetShopItems(string path)
        {
            string[] str = File.ReadAllLines(path);
            Dictionary<string, int> ItemsShop = new Dictionary<string, int>();
            for(int i = 0; i < str.Length; i++)
            {
                string[] subs = str[i].Split(',');
                ItemsShop.Add(subs[0], int.Parse(subs[1]));
            }
            return ItemsShop;
        }

        void Collect(int x, int y)
        {
            string ItemName = "";
            switch (Cells[x, y])
            {
                case '$':
                    map.RemoveFromCell("money", x, y);
                    ItemName = "coin";
                    break;

                case '!':
                    map.RemoveFromCell("scroll", x, y);
                MessageBox.Show(Scrolls['!']);
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
            string Itemstext = GetItemsText();
            tbItemText.Text = Itemstext;
        }

        void DrawMap(char[,] cells)
        {
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    switch (cells[x, y])
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

                        case 'W':
                            map.DrawInCell("gate_opened", x, y);
                            break;

                        case '!':
                            map.DrawInCell("scroll", x, y);
                            break;

                        case 'M':
                            map.DrawInCell("shop", x, y);
                            break;
                            
                    }
                }
            }
        }

        void CheckKey(Key k)
        {
            switch (k)
            {
                case Key.W:
                    if (CheckPassability(Player.X, Player.Y - 1))
                    {
                        Player.MoveTo(Player.X, Player.Y - 1);
                    }
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

            case Key.Space:
                if(Cells[Player.X, Player.Y] == 'M')
                {
                    ShopWindow w = new ShopWindow();
                    w.ShopItems = GetShopItems(@"C:\Users\Admin\Documents\GitHub\GameProject\Квест2022\Квест 2022\Квест 2022\resourses\Shop\товары.txt");
                    w.PlayerItems = Items;
                    w.Init();
                        // TODO: Реализовать функцию Init
                    w.ShowDialog();
                    tbItemText.Text = GetItemsText();
                }
                break;

            case Key.V:
                map.AnimationInCell("exp", Player.X, Player.Y, 1);
                    break;
            }
            Collect(Player.X, Player.Y);
            CheckViktory();
        }

        void CreateProducts()
        {
            Product product = new Product(@"C:\Users\Admin\Documents\GitHub\GameProject\Квест2022\Квест 2022\Квест 2022\resourses\images\stone.png", 2 , "Хороший камень, можно использовать для создания разного оружия");
            products.Add(product);
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
