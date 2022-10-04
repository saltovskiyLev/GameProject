using System;
using System.IO;
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
using Newtonsoft.Json;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LevJson;
using GameMaps;
using GameMaps.Layouts;

namespace GameProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TextArea_Vertical Help = new TextArea_Vertical();
        SimpleTextBox FileName = new SimpleTextBox();
        SimpleTextBox MapName = new SimpleTextBox();
        string[] PictureNames;
        string Picture = "";
        InventoryPanel ipan,menu;
        CellMapInfo cellMap = new CellMapInfo(100, 100, 50,0);
        IGameScreenLayout lay;
        UniversalMap_Wpf map;
        public MainWindow()
        {
            InitializeComponent();
            map = MapCreator.GetUniversalMap(this, cellMap);
            lay = LayoutsFactory.GetLayout(LayoutType.Vertical, placeForMap);
            lay.Attach(map, 0);
            ipan = new InventoryPanel(map.Library, 40, 16);
            menu = new InventoryPanel(map.Library, 40, 16);
            lay.Attach(ipan, 1);
            lay.Attach(menu, 1);
            lay.Attach(Help, 1);
            lay.Attach(FileName, 1);
            lay.Attach(MapName, 1);
            menu.SetBackground(Brushes.GreenYellow);
            map.DrawGrid();
            AddPictures();
            map.Mouse.SetMouseSingleLeftClickHandler(MouseHandler);
            map.Mouse.SetMouseRightClickHandler(RightClick);
            Help.AddTextBlock("FileName","Имя файла:");
        }
        void AddPictures()
        {
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            map.Library.AddPicture("save", "save.png");
            map.Library.AddPicture("load", "load.png");
            menu.AddItem("save","save","сохранить");
            menu.AddItem("load", "load","загрузить");
            int point;
            string name;
            string[] Files = File.ReadAllLines("..//..//objects.txt");
            PictureNames = new string[Files.Length]; 
            for (int i = 0; i < Files.Length; i++)
            {
                point = Files[i].IndexOf('.');
                name = Files[i].Substring(0,point);
                PictureNames[i] = name;
                map.Library.AddPicture(name,Files[i]);
                ipan.AddItem(name,name,name);
            }
            ipan.SetMouseClickHandler(CheckInventory);
            menu.SetMouseClickHandler(CheckMenu);
        }
        void CheckMenu(string element)
        {
            if (element == "save")
            {
                //MessageBox.Show("введите названия файла для сохранения");
                SaveFile();
                SaveJsonFile();
            }
            if(element == "load")
            {
                //MessageBox.Show("введите названия файла для загрузки");
                ReadJsonFile();
            }
        }
        void CheckInventory(string element)
        {
            if (Picture != "")
            {
                ipan.SetItemBackground(Picture,Brushes.Wheat);
            }
            ipan.SetItemBackground(element,Brushes.Blue);
            Picture = element;
        }

        void SaveJsonFile()
        {
            JsonMap JMap = new JsonMap();
            //List<JsonGameObject> Obj = new List<JsonGameObject>();
            for (int i = 0; i < map.XCells; i++)
            {
                for (int j = 0; j < map.YCells; j++)
                {
                    string[] str = map.GetImagesInCell(i, j);
                    for (int k = 0; k < str.Length; k++)
                    {
                        JsonGameObject json = new JsonGameObject();
                        json.X = i;
                        json.Y = j;
                        json.Name = str[k];
                        JMap.Objects.Add(json);
                    }
                }
            }
            JMap.MapName = MapName.TextBox.Text;
            JMap.XCells = map.XCells;
            JMap.YCells = map.YCells;
            string Json = JsonConvert.SerializeObject(JMap);
            File.WriteAllText("..\\..\\Maps\\" + FileName.TextBox.Text + ".json", Json);
        }

        void SaveFile()
        {
            //0,0,ЗАБОР
            string mapTXT = "";
            string[] pics;
            for (int y = 0; y < map.YCells; y++)
            {
                for (int x = 0; x < map.XCells; x++)
                {
                    pics = map.GetImagesInCell(x, y);
                    for (int i = 0; i < pics.Length; i++)
                    {
                        string s = x.ToString() + "," + y.ToString() + "," + pics[i] + "\n";
                        mapTXT = mapTXT + s;
                    }
                }
            }
            if(FileName.TextBox.Text != "")
            {
                File.WriteAllText("..\\..\\Maps\\" + FileName.TextBox.Text + ".TXT", mapTXT);
                MessageBox.Show("карта сохранена");
            }
            else
            {
                MessageBox.Show("укажите имя файла");
            }
            //File.WriteAllText(string.Format("..\\..\\Карта {0} год - {1} месяц - {2} день - {3} час - {4} минута.TXT",DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute), mapTXT);
        }
        void ReadJsonFile()
        {
            string s;
            string name;
            int y;
            int x;
            int p1;
            int p2;
            if (FileName.TextBox.Text != "" && File.Exists(("..\\..\\Maps\\" + FileName.TextBox.Text + ".json")))
            {
                /*string[] Read = File.ReadAllLines("..\\..\\Maps\\" + FileName.TextBox.Text + ".TXT");
                for (int i = 0; i < Read.Length; i++)
                {
                    // В этом цыкле мы ищем запятые,
                    // Определяем три блока с данными.
                    p1 = Read[i].IndexOf(',');
                    p2 = Read[i].LastIndexOf(',');
                    s = Read[i].Substring(0, p1);
                    x = int.Parse(s);
                    s = Read[i].Substring(p1 + 1, p2 - p1 - 1);
                    y = int.Parse(s);
                    name = Read[i].Substring(p2 + 1);
                    map.DrawInCell(name, x, y);
                }*/
                string StringJson = File.ReadAllText("..\\..\\Maps\\" + FileName.TextBox.Text + ".json");
                JsonMap JsonMap = JsonConvert.DeserializeObject<JsonMap>(StringJson);
                for(int i = 0; i < JsonMap.Objects.Count; i++)
                {
                    map.DrawInCell(JsonMap.Objects[i].Name, JsonMap.Objects[i].X, JsonMap.Objects[i].Y);
                }
                MapName.TextBox.Text = JsonMap.MapName;
            }
            else
            {
                MessageBox.Show("укажите имя файла");
            }
        }
        // В этой функции проверяем, какая клавиша была нажата
        void MouseHandler(int x, int y, int xCell, int yCell)
        {
            if (!string.IsNullOrEmpty(Picture))
            {
                map.DrawInCell(Picture, xCell, yCell);
            }
        }
        void RightClick (int x, int y, int xCell, int yCell)
        {
            for (int i = 0; i < PictureNames.Length; i++)
            {
                map.RemoveFromCell(PictureNames[i], xCell, yCell);
            }         
        }
    }
}