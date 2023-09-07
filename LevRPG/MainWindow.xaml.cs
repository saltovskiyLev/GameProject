using System;
using System.Collections.Generic;
using System.IO.Packaging;
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

        NPC npc = new NPC();

        static public UniversalMap_Wpf map;
        CellMapInfo MapInfo = new CellMapInfo(100, 100, 50, 0);

        NPC player = new NPC();

        List<NPC> nPCs = new List<NPC>();

        static string ProjectPath = "E:\\GameProject\\GameProject\\LevRPG\\";

        double ScrollLeft;
        double ScrollRight;

        double ScrollTop;
        double ScrollBottom;

        double MarginTop;
        double MarginLeft;

        double MarginLeftObjective;
        double MarginToptObjective;

        int ScrollSpeed = 5;

        double Height = System.Windows.SystemParameters.PrimaryScreenHeight;
        double Width = System.Windows.SystemParameters.PrimaryScreenWidth;

        double DeltaMarginX;
        double DeltaMarginY;


        TimerController timer = new TimerController();
        public MainWindow()
        {
            InitializeComponent();
            map = MapCreator.GetUniversalMap(this, MapInfo);
            SPCanMap.Children.Add(map.Canvas);

            DeltaMarginX = Width / 10;
            DeltaMarginY = Height / 10;

            AddPictures();

            SetScroll();

            MainEngine = new GameEngine(map);

            player.id = "npc1";
            player.Image = "player";
            player.Name = "npssh";
            player.MaxSide = 123;
            player.CenterX = 553;
            player.CenterY = 524;
            player.Speed = 2;
            MapData MapData;

            npc.Angle = 30;
            npc.CenterX = 290;
            npc.CenterY = 300;
            npc.id = "0";
            npc.Image = "pers";
            npc.MaxSide = 20;
            npc.Name = "npc";
            npc.Speed = 1;
            npc.ContainerName = "nPc";

            npc.Target = player;
            map.Library.AddContainer(npc.ContainerName, npc.Image, ContainerType.AutosizedSingleImage);
            map.ContainerSetMaxSide(npc.ContainerName, 40);
            map.ContainerSetCoordinate(npc.ContainerName, npc.CenterX, npc.CenterY);




            MapData = MapData.GetMapFromeFile(ProjectPath + @"maps\map1.json");
            MainEngine.ReadMap(MapData);

            

            MainEngine.AddNPCObject(player);

            timer.AddAction(GameCycle, 14);
            timer.AddAction(Scroll, 14);
        }

        void AddPictures()
        {
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            map.Library.AddPicture("wall1", "wall1.png");
            map.Library.AddPicture("wall2", "wall2.png");
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

            if(player.CenterY < ScrollTop)
            {
                MarginToptObjective += DeltaMarginY;
                ScrollTop -= DeltaMarginY;
                ScrollBottom -= DeltaMarginY;
            }

            if (player.CenterY > ScrollBottom)
            {
                MarginToptObjective -= DeltaMarginY;
                ScrollBottom += DeltaMarginY;
                ScrollTop += DeltaMarginY;
            }

            if (player.CenterX < ScrollLeft)
            {
                MarginLeftObjective += DeltaMarginX;
                ScrollLeft -= DeltaMarginX;
                ScrollRight -= DeltaMarginX;
            }

            if (player.CenterX > ScrollRight)
            {
                MarginLeftObjective -= DeltaMarginX;
                ScrollRight += DeltaMarginX;
                ScrollLeft += DeltaMarginX;
            }
        }


        void GameCycle()
        {
            Move();
            npc.Move();
        }

        void SetScroll()
        {
            double screenWidth = Width;

            double screenHeight = Height;

            ScrollTop = DeltaMarginY;
            ScrollBottom = Height - ScrollTop;

            ScrollLeft = DeltaMarginY;
            ScrollRight = Width - ScrollLeft;
        }

        void Scroll()
        {
            bool IsChanged = false;

            if(MarginTop > MarginToptObjective)
            {
                MarginTop = MarginTop - ScrollSpeed;
                IsChanged = true;
            }

            else if(MarginTop < MarginToptObjective)
            {
                MarginTop = MarginTop + ScrollSpeed;
                IsChanged = true;
            }

            if (MarginLeft > MarginLeftObjective)
            {
                MarginLeft = MarginLeft - ScrollSpeed;
                IsChanged = true;
            }

            else if (MarginLeft < MarginLeftObjective)
            {
                MarginLeft = MarginLeft + ScrollSpeed;
                IsChanged = true;
            }

            if(IsChanged)
            {
                map.Canvas.Margin = new Thickness(MarginLeft, MarginTop, 0, 0);
            }
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
