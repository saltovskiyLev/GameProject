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
        CellMapInfo MapInfo = new CellMapInfo(100, 100, 50, 0);

        NPC player = new NPC();

        List<NPC> nPCs = new List<NPC>();

        TimerController timer = new TimerController();
        public MainWindow()
        {
            InitializeComponent();
            map = MapCreator.GetUniversalMap(this, MapInfo);
            SPCanMap.Children.Add(map.Canvas);

            AddPictures();

            MainEngine = new GameEngine(map);

            player.id = "npc1";
            player.Image = "player";
            player.Name = "npssh";
            player.MaxSide = 123;
            player.CenterX = 553;
            player.CenterY = 524;
            player.Speed = 2;
            MapData MapData;

            NPC npc = new NPC();
            npc.Angle = 30;
            npc.CenterX = 290;
            npc.CenterY = 300;
            npc.id = "0";
            npc.Image = "pers";
            npc.MaxSide = 20;
            npc.Name = "npc";
            npc.Speed = 1;




            MapData = MapData.GetMapFromeFile(@"C:\Users\Admin\Documents\GitHub\GameProject\LevRPG\maps\map1.json");
            MainEngine.ReadMap(MapData);

            

            MainEngine.AddNPCObject(player);

            timer.AddAction(GameCycle, 14);
        }

        void AddPictures()
        {
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            map.Library.AddPicture("wall1", "wall1.png");
            map.Library.AddPicture("wall2", "wall2.png");
            map.Library.AddPicture("pers", "pers.png");
            map.Library.AddPicture("player", "enemy.png");
            map.Library.AddPicture("pers", "pers.png");

        }

        void Move()
        {
            double dX, dY;
            if(map.Keyboard.IsKeyPressed(Key.W))
            {
                dX = player.Speed * Math.Cos(GameMath.DegreesToRad(player.Angle));
                dY = player.Speed * Math.Sin(GameMath.DegreesToRad(player.Angle));
                if (MainEngine.CanMove(player, dX, dY, 0))
                {

                    player.CenterX += dX;
                    player.CenterY += dY;
                    MainEngine.Move(player);
                }

            }

            if (map.Keyboard.IsKeyPressed(Key.S))
            {
                dX = -player.Speed * Math.Cos(GameMath.DegreesToRad(player.Angle));
                dY = -player.Speed * Math.Sin(GameMath.DegreesToRad(player.Angle));
                if (MainEngine.CanMove(player, dX, dY, 0))
                {

                    player.CenterX += dX;
                    player.CenterY += dY;
                    MainEngine.Move(player);
                }

            }

            if (map.Keyboard.IsKeyPressed(Key.A))
            {
                if (MainEngine.CanMove(player, 0, 0, -1))
                {
                    player.Angle--;

                    MainEngine.Move(player);

                }

            }

            if (map.Keyboard.IsKeyPressed(Key.D))
            {
                if (MainEngine.CanMove(player, 0, 0, +1))
                {
                    player.Angle++;

                    MainEngine.Move(player);

                }

            }
        }


        void GameCycle()
        {
            Move();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            MessageWindow w = new MessageWindow();
            w.SetMessageText("Здравствуйте");
            w.SetButtonVisibility(1, Visibility.Collapsed);
            w.SetButtonVisibility(3, Visibility.Collapsed);
            w.SetButtonVisibility(2, Visibility.Visible);

            w.SetButtonText(2, "Начать игру");



            w.ShowDialog();
        }
    }

}
