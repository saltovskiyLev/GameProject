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
        string Picture = "";
        InventoryPanel ipan;
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
            lay.Attach(ipan, 1);

            map.Keyboard.SetSingleKeyEventHandler(CheckKey);
            map.DrawGrid();
            AddPictures();
            map.Mouse.SetMouseSingleLeftClickHandler(MouseHandler);
            map.Mouse.SetMouseRightClickHandler(RightClick);
            //map.Library.AddAnimation("fire", a);
            /*a = new AnimationDefinition();
            a.AddFrame(50, "fire1");
            a.AddFrame(50, "fire2");
            a.AddFrame(50, "fire3");
            a.AddFrame(50, "fire4");
            a.AddFrame(50, "fire5");
            map.Library.AddAnimation("fire", a);*/
        }
        void AddPictures()
        {
            int point;
            string name;
            string[] Files = File.ReadAllLines("..//..//objects.txt"); 
            for(int i = 0; i < Files.Length; i++)
            {
                point = Files[i].IndexOf('.');
                name = Files[i].Substring(0,point);
                map.Library.AddPicture(name,Files[i]);
                ipan.AddItem(name,name,name);
            }
            ipan.SetMouseClickHandler(CheckInventory);
            /*map.Library.AddPicture("ЗАБОР", "ЗАБОР.png");
            map.Library.AddPicture("wall", "wall.png");
            map.Library.AddPicture("stone", "stone.png");
            map.Library.AddPicture("gem_blue", "gem_blue.png");
            map.Library.AddPicture("gate_closed", "gate_closed.png");
            ipan.AddItem("stone", "stone", "2");
            ipan.AddItem("wall", "wall", "1");
            ipan.AddItem("ЗАБОР", "ЗАБОР", "3");
            ipan.AddItem("gem_blue", "gem_blue", "4");
            ipan.AddItem("gate_closed", "gate_closed", "5");
            map.Library.AddPicture("fire1", "fire1.png");
            map.Library.AddPicture("fire2", "fire2.png");
            map.Library.AddPicture("fire3", "fire3.png");
            map.Library.AddPicture("fire4", "fire4.png");
            map.Library.AddPicture("fire5", "fire5.png");*/
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
        // В этой функции проверяем, какая клавиша была нажата
        void CheckKey(Key k)
        {
            if (k == Key.D1)
            {
                Picture = "wall";
            }
            if (k == Key.D2)
            {
                Picture = "stone";
            }
            if (k == Key.D3)
            {
                Picture = "ЗАБОР";
            }
            if (k == Key.D4)
            {
                Picture = "gem_blue";
            }
            if (k == Key.D5)
            {
                Picture = "gate_closed";
            }
        }
        void MouseHandler(int x, int y, int xCell, int yCell)
        {
            if (!string.IsNullOrEmpty(Picture))
            {
                map.DrawInCell(Picture, xCell, yCell);
            }
        }
        void RightClick (int x, int y, int xCell, int yCell)
        {
            map.RemoveFromCell("wall", xCell, yCell);
            map.RemoveFromCell("ЗАБОР", xCell, yCell);
            map.RemoveFromCell("stone", xCell, yCell);
            map.RemoveFromCell("gem_blue", xCell, yCell);
            map.RemoveFromCell("gate_closed", xCell, yCell);
          // map.RemoveAllImagesInCells(xCell,yCell);
        }
    }
}
