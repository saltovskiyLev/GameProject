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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameMaps;
using GameMaps.Layouts;

namespace GameProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            menu.SetBackground(Brushes.GreenYellow);
            map.DrawGrid();
            AddPictures();
            map.Mouse.SetMouseSingleLeftClickHandler(MouseHandler);
            map.Mouse.SetMouseRightClickHandler(RightClick);
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
                SaveFile();
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
        void SaveFile()
        {
            //0,0,ЗАБОР
            string mapTXT = "";
            string[] pics; 
            for (int y = 0; y < map.YCells; y++)
            {
                for(int x = 0; x < map.XCells; x++)
                {
                    pics = map.GetImagesInCell(x,y);
                    for (int i = 0; i < pics.Length; i++)
                    {
                        string s = x.ToString() + "," + y.ToString() + "," + pics[i] + "\n";
                        mapTXT = mapTXT + s;
                    }
                }
            }
            File.WriteAllText(string.Format("Карта {0} год - {1} месяц - {2} день - {3} час - {4} минута.TXT",DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute),mapTXT);
            MessageBox.Show("карта сохранена");
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
