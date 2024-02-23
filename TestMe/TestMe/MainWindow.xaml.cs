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

namespace TestMe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CellMapInfo inf = new CellMapInfo(100, 100, 10, 50);
            UniversalMap_Wpf map = MapCreator.GetUniversalMap(this, inf);

            SPmap.Children.Add(map.Canvas);
   

            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\", Type = PathType.Relative };
            map.Library.AddPicture("cube", "cbig1.png");

            map.DrawLine(200, 200, 400, 200, Brushes.Black);


            map.Library.AddContainer("1st", "cube", ContainerType.AutosizedSingleImage);
            map.ContainerSetMaxSide("1st", 200);
            map.ContainerSetCoordinate("1st", 300, 130);

            map.ContainerSetAngle("1st", 60);
            if(map.CollisionLineContainer(200, 200, 400, 200, "1st"))
            {
                MessageBox.Show("collision");
            }
            
        }

    }
}
