using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GameMaps;
using GameMaps.Layouts;
using System.Windows.Documents;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Railroad_Paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UniversalMap_Wpf map;
        CellMapInfo cellMap = new CellMapInfo(31, 21, 50, 0);
        InventoryPanel ipan;
        string SelectedItem = "";
        GameObjects[,] MapObjects = new GameObjects[31, 21];
        IGameScreenLayout lay;
        public MainWindow()
        {
            InitializeComponent();
            map = MapCreator.GetUniversalMap(this, cellMap);
            map.Mouse.SetMouseSingleLeftClickHandler(MapClck);
            ipan = new InventoryPanel(map.Library, 60, 16);
            ipan.SetMouseClickHandler(InventoriClick);
            lay = LayoutsFactory.GetLayout(LayoutType.Vertical, this.Content);
            lay.SetBackground(Brushes.Wheat);
            lay.Attach(map, 0);
            lay.Attach(ipan, 1);
            map.SetGridColor(Brushes.AliceBlue);
            map.DrawGrid();
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            //map.Library.AddPicture("rails_1", "ДЯДЯ ВАНЯ.png");
            //ipan.AddItem ("Rails_1", "rails_1");
            //yfqnb gtcy, many rain
            map.Library.AddPicture("rails_1", "РельcыN1.png");
            ipan.AddItem("Rails_1", "rails_1");
            map.Library.AddPicture("rails_2", "Рельса_90(градусов).png");
            ipan.AddItem("Rails_2", "rails_2");
            MapObjects[30, 0] = GameObjects.Vertical;
            ReadMap();
        }
        void ReadMap()
        {
            // N1 Cчитываем файл в массив строк
            // N2 В каждой строке проверить каждый символ, если это не пробел то выыисти на карту обьект соответствующий этому символу
            string[] rows = File.ReadAllLines(@"C:\Users\Admin\Documents\GitHub\GameProject\Railroad_Paint\Railroad_Paint\Начальная Карта\Депо.txt");
            for(int i = 0; i < rows.Length; i++)
            {
                for (int j = 0; j < rows[i].Length; j++)
                {
                    if (rows[i][j] == 'V')
                    {
                        map.DrawInCell("rails_1", j, i);
                    }
                }
            }
        }
        void InventoriClick(string ItemName)
        {
            if (SelectedItem != "")
            {
                ipan.SetItemBackground(SelectedItem, Brushes.Wheat);
            }
            SelectedItem = ItemName;
            ipan.SetItemBackground(ItemName, Brushes.Green);
        }
        void MapClck(int X, int Y, int cX, int cY)
        {
            if (SelectedItem == "Rails_1")
            {
                map.DrawInCell("rails_1", cX, cY);
            }
            if (SelectedItem == "Rails_2")
            {
                map.DrawInCell("rails_2", cX, cY);
            }
        }
    }
    public enum GameObjects
    {
              Horisontal,
              Vertical,
              LeftDown,
              RightDown,
              RightUp,
              LetfUp
    }
}