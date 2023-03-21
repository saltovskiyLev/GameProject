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

namespace LevRPG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameEngine MainEngine;

        static public UniversalMap_Wpf map;
        CellMapInfo MapInfo = new CellMapInfo(10, 10, 50, 0);

        NPC player = new NPC();

        TimerController timer = new TimerController();
        public MainWindow()
        {
            InitializeComponent();
            map = MapCreator.GetUniversalMap(this, MapInfo);
            SPCanMap.Children.Add(map.Canvas);

            AddPictures();

            map.SetMapBackground(Brushes.Navy);
            MainEngine = new GameEngine(map);

            player.id = "npc1";
            player.Image = "pers";
            player.Name = "npssh";
            player.MaxSide = 123;
            player.CenterX = 153;
            player.CenterY = 124;
            player.Speed = 2;
            MapData MapData;

            MapData = MapData.GetMapFromeFile(@"C:\Users\Admin\Documents\GitHub\GameProject\LevRPG\maps\map1.json");
            MainEngine.ReadMap(MapData);

            

            MainEngine.AddNPCObject(player);

            timer.AddAction(GameCycle, 14);
        }

        void AddPictures()
        {
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            map.Library.AddPicture("wall1", "wall1.png");
            map.Library.AddPicture("pers", "pers.png");
        }

        void Move()
        {
            double dX, dY;
            if(map.Keyboard.IsKeyPressed(Key.W))
            {
                dX = player.Speed * Math.Cos(GameMath.DegreesToRad(player.Angle));
                dY = player.Speed * Math.Sin(GameMath.DegreesToRad(player.Angle));

                player.CenterX += dX;
                player.CenterY += dY;
                MainEngine.Move(player);

            }

            if(map.Keyboard.IsKeyPressed(Key.A))
            {
                player.Angle--;
            }

            if (map.Keyboard.IsKeyPressed(Key.D))
            {
                player.Angle++;
            }
        }

        void GameCycle()
        {
            Move();
        }
    }

}
