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
        CellMapInfo MapInfo = new CellMapInfo(64, 35, 30, 0);
        static public UniversalMap_Wpf map;
        int X;
        int Y;
        //char[,] Chelz = new char[];
        public string[] path = new string[2];
        public MainWindow()
        {
            InitializeComponent();
            map = MapCreator.GetUniversalMap(this, MapInfo);
            panelMap.Children.Add(map.Canvas);
            map.DrawGrid();
            File.ReadAllLines(path[0]);
            MaxLenght();
            path[0] = @"C:\Users\Admin\Documents\GitHub\GameProject\The Lord Of The Rings\The Lord Of The Rings\карта.txt"
            AddPictures();
        }
        void AddPictures()
        {
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            map.Library.AddPicture("enemy", "enemy.png");
            map.Library.AddPicture("Frodo", "frodo.png");
        }
        int maxLenght = 0;
        int number = path[maxLenght].Length;
        void MaxLenght()
        {
            for(int i = 0; i < maxLenght; i++)
            {
                if(path[i].Length > maxLenght)
                {
                    maxLenght = path[i].Length;
                    number = i;
                }
            }
        }
    }
}
