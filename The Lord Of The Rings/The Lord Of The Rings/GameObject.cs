using GameMaps;
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

namespace The_Lord_Of_The_Rings
{
    class GameObject
    {
        static public UniversalMap_Wpf Map;
        string Name;
        public int X { get; private set; }
        public int Y { get; private set; }
        Dictionary<int, string> Items = new Dictionary<int, string>();
        List<GameObject> S = new List<GameObject>();                                                                                              
        public GameObject(int X, int Y, string PictureName)
        {
            Name = PictureName;
            Move(X, Y);
        }
        public void Move(int x, int y)
        {
            MainWindow.map.RemoveFromCell(Name, X, Y);
            X = x;
            Y = y;
            MainWindow.map.DrawInCell(Name, x, y);
        }
    }
}