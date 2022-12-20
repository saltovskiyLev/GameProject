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

namespace Бомбер
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CellMapInfo MapInfo = new CellMapInfo(20, 20, 40, 0);
        static public UniversalMap_Wpf map;
        int X = 90;
        int Y = 120;
        TimerController timer = new TimerController();
        Random r = new Random();
        public MainWindow()
        {
            InitializeComponent();
            map = MapCreator.GetUniversalMap(this, MapInfo);
            SPmap.Children.Add(map.Canvas);
            AddPictires();

            map.SetMapBackground("забор");

            map.Library.AddContainer("b", "gem", ContainerType.AutosizedSingleImage);
            map.ContainerSetCoordinate("b", 150, 200);

            map.Library.AddContainer("a", "забор", ContainerType.AutosizedSingleImage);
            map.ContainerSetAngle("a", 0);
            map.ContainerSetSize("a", 60, 29);

            timer.AddAction(TimerVoid, 50);
            map.ContainerSetSize("a", 20, 20);
            map.ContainerSetMaxSide("a", 150);
            map.ContainerSetCoordinate("a", 90, 120);
            map.ContainerSetAngle("a", 90);

        }

        void AddPictires()
        {
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            map.Library.AddPicture("забор", "ЗАБОР.png");
            map.Library.AddPicture("gem", "gem_red.png");
        }

        void TimerVoid()
        {
            if(map.Keyboard.IsKeyPressed(Key.D))
            {
                X = X + 3;
                map.ContainerSetCoordinate("a", X, Y);
            }

            if (map.Keyboard.IsKeyPressed(Key.A))
            {
                X = X - 3;
                map.ContainerSetCoordinate("a", X, Y);
            }

            if (map.Keyboard.IsKeyPressed(Key.W))
            {
                Y = Y - 3;
                map.ContainerSetCoordinate("a", X, Y);
            }

            if (map.Keyboard.IsKeyPressed(Key.S))
            {
                Y = Y + 3;
                map.ContainerSetCoordinate("a", X, Y);
            }

            if(map.CollisionContainers("a", "b"))
            {
                map.ContainerSetCoordinate("b", r.Next(0, 800), r.Next(0, 800));
            }
        }
    }
}
