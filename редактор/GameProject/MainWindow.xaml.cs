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
using GameMaps;

namespace GameProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UniversalMap_Wpf map;
        public MainWindow()
        {
            InitializeComponent();
            map = MapCreator.CreateMap(this, 10, 10, 50);
            map.Keyboard.SetSingleKeyEventHandler(CheckKey);
            map.DrawGrid();
            map.Library.AddPicture("wall", "wall.png");
            map.Library.AddPicture("nota", "nota.png");
            // map.DrawInCell("wall", 1, 2);
            map.Library.AddContainer("nota", "nota");
            //map.ContainerSetSize("nota", 120, 240);
            //map.ContainerSetCoordinate("nota", 50, 50);
            map.DrawLine(0,map.YAbsolute / 2, map.XAbsolute, map.YAbsolute / 2, Brushes.Red);
            map.Mouse.SetMouseDoubleLeftClickHandler(LeftClick);
        }

        // В этой функции проверяем, какая клавиша была нажата
        void CheckKey(Key k)
        {

        }

        void LeftClick(int x, int y, int Xcell, int Ycell)
        {
            MessageBox.Show(Xcell.ToString());
        }
    }
}