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
using GameMaps;
using GameMaps.Layouts;
using System.Windows.Shapes;

namespace Tower_Defenc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TimerController timer = new TimerController();
        UniversalMap_Wpf map;
        CellMapInfo cellMap = new CellMapInfo(31, 21, 50, 0);
        InventoryPanel ipan;
        InventoryPanel UnitsPanel;
        int money = 150;
        IGameScreenLayout lay;
        public MainWindow()
        {
            InitializeComponent();
            map = MapCreator.GetUniversalMap(this, cellMap);
            ipan = new InventoryPanel(map.Library, 40, 16);
            UnitsPanel = new InventoryPanel(map.Library, 120, 16);
            lay = LayoutsFactory.GetLayout(LayoutType.Vertical, this.Content);
            lay.SetBackground(Brushes.Wheat);
            lay.Attach(map, 0);
            lay.Attach(ipan, 1);
            lay.Attach(UnitsPanel, 1);
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            map.Library.AddPicture("background", "Сзади.png");
            map.Library.AddPicture("money", "money.png");
            map.Library.AddPicture("Tank_Low_AllY", "Танк с башней без поворота(ДОБРЫЙ СЛАБЫЙ).png");
            map.Library.AddPicture("basa", "base.png");
            map.SetMapBackground("background");
            ipan.AddItem("money", "money", money.ToString());
            UnitsPanel.AddItem("ЧИСТОТАНК", "Tank_Low_AllY", "Слабый танк: 50");
            UnitsPanel.SetMouseClickHandler(BuildUnit);
            GameObject.map = map;
            GameObject basa = new GameObject("basa","BASA");
            map.ContainerSetSize(basa.ContainerName, 100, 100);
            basa.SetCoordinate(map.XAbsolute / 2, map.YAbsolute / 2);
            //map.Keyboard.SetSingleKeyEventHandler(CheckKey);
        }
        void BuildUnit(string UnitName)
        {
            if (UnitName == "ЧИСТОТАНК")
            {
                GameObject Tank = new GameObject("Tank_Low_AllY", "Tank_Low_ally", map.XAbsolute / 2, map.YAbsolute / 2, 60);
                
            }
        }
    }
}