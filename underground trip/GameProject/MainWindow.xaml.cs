using GameMaps.Layouts;
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
        Random r = new Random();
        int EnergyPlayer = 600;
        int EnergyEvil = 6;
        bool Delete;
        bool isEvilElive = true;
        bool isPlayerAlive = true;
        int portals = 5;
        int holes = 5;
        int swordX = 9,swordY =5;
        int potionX = 12;
        int potionY = 2;
        bool PlayerTurn = true;
        TimerController timer = new TimerController();
        UniversalMap_Wpf map;
        CellMapInfo cellMap = new CellMapInfo(20, 10, 50, 0);
        InventoryPanel ipan;
        InventoryPanel EnergyPanel;
        IGameScreenLayout lay;
        int hasTNT = 5;
        bool hasSword = false;
        bool hasPick = false;
        bool hasBow = false;
        int arrow = 0;
        int[] portalX;
        int[] portalY;
        int Player2X, Player2Y;
        int PlayerX, PlayerY;
        public MainWindow()
        {
            InitializeComponent();
            map = MapCreator.GetUniversalMap(this, cellMap);
            ipan = new InventoryPanel(map.Library, 40, 16);
            EnergyPanel = new InventoryPanel(map.Library, 120, 16);
            lay = LayoutsFactory.GetLayout(LayoutType.Vertical, this.Content);
            lay.SetBackground(Brushes.Wheat);
            lay.Attach(map, 0);
            lay.Attach(ipan, 1);
            lay.Attach(EnergyPanel,1);
            map.Keyboard.SetSingleKeyEventHandler(CheckKey);
            //map.DrawGrid();
            portalX = new int[portals];
            portalY = new int[portals];
            for (int i = 0; i < portalX.Length; i++)
            {
                portalX[i] = r.Next(1,map.XCells - 1);
            }
            for(int i = 0; i < portalY.Length; i++)
            {
                portalY[i] = r.Next(1, map.YCells - 1);
            }
            AddPictures();
              DrawMaps();

            EnergyPanel.AddItem("PlayerEnergy", "green6", "");
            EnergyPanel.AddItem("EvilEnergy", "red6", "");
        
            map.Library.AddContainer("fell", "fell");
        
            PlayerX = 3;
            PlayerY = 4;
            Player2X = 1; Player2Y = 4;
           
            ipan.AddItem("arrow", "arrow","0");

            ipan.AddItem("hole", "hole", "");
            PrintHolesNum();      
            int x = 10, y = 9;
            while (x <= 19)
            {
                map.DrawInCell("wall", x, 9);
                map.DrawInCell("wall", x, 0);
                map.DrawInCell("wall", x, 7);
                map.DrawInCell("wall", x, 4);
                x = x + 1;
            }
            x = 10;
            while (x <= 19)
            {
                map.DrawInCell("wall", x, 8);
                x = x + 1;
            }
            while (y >= 0)
            {
                map.DrawInCell("wall", 11, y);
                y = y - 1;
            }
            map.DrawInCell("pick", 7, 2);
            AnimationDefinition a = new AnimationDefinition();
            ipan.AddItem("DINAMIT", "DINAMIT", "динамит");
            ipan.SetText("DINAMIT", hasTNT.ToString());
            a.AddFrame(50, "exep");
            a.AddFrame(50, "exep1");
            a.AddFrame(50, "exep2");
            a.AddFrame(50, "exep3");
            a.AddFrame(50, "exep4");
            a.AddFrame(50, "exep5");
            a.AddFrame(50, "exep6");
            a.AddFrame(50, "exep7");
            a.AddFrame(50, "exep8");
            a.AddFrame(50, "exep9");
            a.AddFrame(50, "exep10");
            a.AddFrame(50, "exep12");
            a.AddFrame(50, "exep13");
            a.AddFrame(50, "exep14");
            a.AddFrame(50, "exep15");
            a.AddFrame(50, "exep16");
            a.AddFrame(0, "nothing");
            a.LastFrame = "nothing";
            map.Library.AddAnimation("exep", a);
            a = new AnimationDefinition();
            a.AddFrame(100, "fire1");
            a.AddFrame(100, "fire2");
            a.AddFrame(100, "fire3");
            a.AddFrame(100, "fire4");
            a.AddFrame(100, "fire5");
            a.AddFrame(0, "nothing");
            a.LastFrame = "nothing";
            map.Library.AddAnimation("fire", a);
            for (int i = 0; i < map.YCells; i++)
            {
                map.RemoveFromCell("wall",0,i);
                map.DrawInCell("fence", 0, i);
                map.RemoveFromCell("wall",map.XCells - 1 ,i);
                map.DrawInCell("fence",map.XCells - 1, i);
            }
            
            for (int i = 0; i < map.XCells; i++)
            {
                map.DrawInCell("fence", i,0);
                map.RemoveFromCell("wall",i,0);
                map.RemoveFromCell("wall", i, 0);
                map.DrawInCell("fence", i, map.YCells - 1);
                map.RemoveFromCell("wall", i, map.YCells - 1);
                map.RemoveFromCell("wall", i, map.YCells - 1);
                map.RemoveFromCell("vorota", i, map.YCells - 1);
            }

            for (int j = 1; j < map.YCells - 1; j++)
            {
                for (int i = 1; i < map.XCells - 1; i++)
                {
                    map.DrawInCell("unknown", i, j);
                }
            }
            map.Mouse.SetMouseSingleLeftClickHandler(LeftClick);
        }
        /// <summary>
        /// удаляет "туман войны"
        /// </summary>
        /// <param name="x"> координата по x </param>
        /// <param name="y"> координата по y </param>
        void RemoveFog(int x, int y)
        {
            if (x < map.XCells - 1 && x > 0 && y < map.YCells - 1 && y > 0)
            {
                for (int i = y - 1; i <= y + 1; i++)
                {
                    for (int j = x - 1; j <= x + 1; j++)
                    {
                        map.RemoveFromCell("unknown", j, i);
                    }
                }
            }
        }
        void AddPictures()
        {
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            map.Library.AddPicture("portal", "portal.png");
            map.Library.AddPicture("green0", "green0.png");
            map.Library.AddPicture("green1", "green1.png");
            map.Library.AddPicture("green2", "green2.png");
            map.Library.AddPicture("green3", "green3.png");
            map.Library.AddPicture("green4", "green4.png");
            map.Library.AddPicture("green5", "green5.png");
            map.Library.AddPicture("green6", "green6.png");
            map.Library.AddPicture("red0", "red0.png");
            map.Library.AddPicture("red1", "red1.png");
            map.Library.AddPicture("red2", "red2.png");
            map.Library.AddPicture("red3", "red3.png");
            map.Library.AddPicture("red4", "red4.png");
            map.Library.AddPicture("red5", "red5.png");
            map.Library.AddPicture("fell", "упал.png");
            map.Library.AddPicture("got", "попал.png");
            map.Library.AddPicture("missed", "не попал.png");
            map.Library.AddPicture("undermined", "подорвал.png");
            map.Library.AddPicture("stabbed", "ЗАКАЛОЛ.png");
            map.Library.AddPicture("red6", "red6.png");
            map.Library.AddPicture("clock", "clock.png");
            map.Library.AddPicture("fire1", "fire1.png");
            map.Library.AddPicture("fire2", "fire2.png");
            map.Library.AddPicture("hole", "hole.png");
            map.Library.AddPicture("arrow", "untitled.png");
            map.Library.AddPicture("bow", "untitled2.png");
            map.Library.AddPicture("fire3", "fire3.png");
            map.Library.AddPicture("fire4", "fire4.png");
            map.Library.AddPicture("pick", "pick.png");
            map.Library.AddPicture("DINAMIT", "143108_800x355_creeper_tnt_wallpaper_by_shadowe56b0cb7b.png");
            map.Library.AddPicture("fire5", "fire5.png");
            map.Library.AddPicture("rip2", "rip2.png");
            map.Library.AddPicture("wall", "wall.png");
            map.Library.AddPicture("exep", "exp4-0.png");
            map.Library.AddPicture("exep1", "exp4-1.png");
            map.Library.AddPicture("exep2", "exp4-2.png");
            map.Library.AddPicture("exep3", "exp4-3.png");
            map.Library.AddPicture("exep4", "exp4-4.png");
            map.Library.AddPicture("exep5", "exp4-5.png");
            map.Library.AddPicture("exep6", "exp4-6.png");
            map.Library.AddPicture("exep7", "exp4-7.png");
            map.Library.AddPicture("exep8", "exp4-8.png");
            map.Library.AddPicture("exep9", "exp4-9.png");
            map.Library.AddPicture("exep10", "exp4-10.png");
            map.Library.AddPicture("exep12", "exp4-12.png");
            map.Library.AddPicture("exep13", "exp4-13.png");
            map.Library.AddPicture("exep14", "exp4-14.png");
            map.Library.AddPicture("exep15", "exp4-15.png");
            map.Library.AddPicture("exep16", "exp4-16.png");
            map.Library.AddPicture("nothing", "nothing.png");
            map.Library.AddPicture("potionRed", "pt1.png");
            map.Library.AddPicture("potionBlue", "pt2.png");
            map.Library.AddPicture("potionGreen", "pt3.png");
            map.Library.AddPicture("potionYellow", "pt4.png");
            map.Library.AddPicture("unknown", "unknown.png");
            map.Library.AddPicture("fence", "ЗАБОР.png");
            map.Library.AddPicture("smile", "smile1.png");
            map.Library.AddPicture("green", "gem_green.png");
            map.Library.AddPicture("rip", "rip1.png");
            map.Library.AddPicture("camen", "stone.png");
            map.Library.AddPicture("sword", "sword.png");
            map.Library.AddPicture("evil", "evil1.png");
            map.Library.AddPicture("gem", "gem_red.png");
            map.Library.AddPicture("vorota", "gate_opened.png");
        }
        void DrawMaps()
        {
            for(int i = 0;i < portals; i++)
            {
                map.DrawInCell("portal",portalX[i],portalY[i]);
            }
            map.DrawInCell("potionRed", potionX, potionY);
            map.DrawInCell("wall", 1, 2);
            map.DrawInCell("wall", 0, 0);
            map.DrawInCell("wall", 1, 3);
            map.DrawInCell("wall", 1, 9);
            map.DrawInCell("wall", 9, 5);
            map.DrawInCell("wall", 9, 3);
            map.DrawInCell("wall", 9, 4);
            map.DrawInCell("wall", 9, 6);
            map.DrawInCell("wall", 9, 7);
            map.DrawInCell("wall", 9, 8);
            map.DrawInCell("wall", 9, 9);
            map.DrawInCell("wall", 9, 2);
            map.DrawInCell("wall", 9, 1);
            map.DrawInCell("wall", 9, 0);
            map.DrawInCell("wall", 0, 1);
            map.DrawInCell("wall", 0, 2);
            map.DrawInCell("wall", 0, 3);
            map.DrawInCell("wall", 0, 4);
            map.DrawInCell("wall", 0, 5);
            map.DrawInCell("wall", 0, 6);
            map.DrawInCell("wall", 0, 7);
            map.DrawInCell("wall", 0, 8);
            map.DrawInCell("wall", 0, 9);
            map.DrawInCell("wall", 1, 0);
            map.DrawInCell("wall", 1, 2);
            map.DrawInCell("wall", 1, 3);
            map.DrawInCell("wall", 3, 1);
            map.DrawInCell("wall", 3, 5);
            map.DrawInCell("wall", 2, 5);
            map.DrawInCell("wall", 1, 3);
            map.DrawInCell("wall", 2, 6);
            map.DrawInCell("wall", 2, 4);
            map.DrawInCell("wall", 2, 2);
            map.DrawInCell("wall", 2, 0);
            map.DrawInCell("wall", 2, 1);
            map.DrawInCell("green", r.Next(1, map.XCells - 1), r.Next(1, map.YCells - 1));
            map.DrawInCell("arrow", r.Next(1, map.XCells - 1), r.Next(1, map.YCells - 1));
            map.DrawInCell("bow", r.Next(1, map.XCells - 1), r.Next(1, map.YCells - 1));
            map.DrawInCell("vorota", 9, 6);
            map.DrawInCell("vorota", 9, 7);
            map.DrawInCell("vorota", 9, 8);
            map.DrawInCell("vorota", 9, 9);
            map.DrawInCell("camen", 9, 5);
            map.DrawInCell("gem", 1, 1);
            map.DrawInCell("wall", 4, 1);
            map.DrawInCell("wall", 5, 1);
            map.DrawInCell("wall", 6, 1);
            map.DrawInCell("wall", 7, 1);
            map.DrawInCell("wall", 8, 1);
            map.DrawInCell("wall", 8, 3);
            map.DrawInCell("wall", 8, 4);
            map.DrawInCell("wall", 8, 5);
            map.DrawInCell("wall", 8, 6);
            map.DrawInCell("wall", 8, 7);
            map.DrawInCell("wall", 7, 5);
            map.DrawInCell("wall", 7, 6);
            map.DrawInCell("wall", 7, 7);
            map.DrawInCell("wall", 7, 8);
            map.DrawInCell("sword", swordX, swordY);
            map.DrawInCell("smile", PlayerX, PlayerY);
            map.DrawInCell("clock", 1, 4);
            map.DrawInCell("wall", 1, 9);
            map.DrawInCell("wall", 7, 9);
            map.DrawInCell("wall", 8, 2);
            map.DrawInCell("wall", 2, 9);
            map.DrawInCell("wall", 3, 9);
            map.DrawInCell("wall", 6, 9);
            map.DrawInCell("wall", 5, 8);
            map.DrawInCell("wall", 6, 8);
            map.DrawInCell("wall", 4, 6);
            map.DrawInCell("wall", 5, 6);
            map.DrawInCell("wall", 6, 3);
            map.DrawInCell("wall", 5, 3);
            map.DrawInCell("camen", 9, 4);
            map.DrawInCell("DINAMIT", 2, 3);
        }
        void DrawPlayerEnergy(int Energy)
        {
            if (Energy <= 0)
            {
                EnergyPanel.SetImage("PlayerEnergy", "green0");
            }
            else if (Energy == 1)
            {
                EnergyPanel.SetImage("PlayerEnergy", "green1");
            }
            else if (Energy == 2)
            {
                EnergyPanel.SetImage("PlayerEnergy", "green2");
            }
            else if (Energy == 3)
            {
                EnergyPanel.SetImage("PlayerEnergy", "green3");
            }
            else if (Energy == 4)
            {
                EnergyPanel.SetImage("PlayerEnergy", "green4");
            }
            else if (Energy == 5)
            {
                EnergyPanel.SetImage("PlayerEnergy", "green5");
            }
            else
                EnergyPanel.SetImage("PlayerEnergy", "green6");
        }
        void SetPlayerEnergy(int Energy)
        {
            EnergyPlayer = Energy;
            DrawPlayerEnergy(EnergyPlayer);
        }
        void DrawEvilEnergy(int Energy)
        {
            if (Energy <= 0)
            {
                EnergyPanel.SetImage("EvilEnergy", "red0");
            }
            else if (Energy == 1)
            {
                EnergyPanel.SetImage("EvilEnergy", "red1");
            }
            else if (Energy == 2)
            {
                EnergyPanel.SetImage("EvilEnergy", "red2");
            }
            else if (Energy == 3)
            {
                EnergyPanel.SetImage("EvilEnergy", "red3");
            }
            else if (Energy == 4)
            {
                EnergyPanel.SetImage("EvilEnergy", "red4");
            }
            else if (Energy == 5)
            {
                EnergyPanel.SetImage("EvilEnergy", "red5");
            }
            else 
            {
                EnergyPanel.SetImage("EvilEnergy", "red6");
            }
        }
        void SetEvilEnergy(int Energy)
        {
            EnergyEvil = Energy;
            DrawEvilEnergy(EnergyEvil);
        }
        void DeletePicture()
        {
            if(Delete)
            {
                map.ContainerSetFrame("fell", "nothing");
                timer.RemoveAction(DeletePicture, 2000);
            }
            Delete = true;
        }
        //-------------------------------------------------------------
        void drawHorizontalWall(int x, int y, int length)
        { 
            int i = x ;
            while (i < x + length) 
            {
                map.DrawInCell("wall",i,y);
              i = i + 1;
            }
        }
        //------------------------------------------------
        void PrintHolesNum()
        {
            ipan.SetText("hole","пропасть: "+holes);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        // В этой функции проверяем, какая клавиша была нажата
        void CheckKey(Key k)
        {
            int PlayerLastX = PlayerX, PlayerLastY = PlayerY;

            int PlayerLast2X = Player2X, PlayerLast2Y = Player2Y;
            if (isPlayerAlive && PlayerTurn)
            {
               /* if (k == Key.D0)
                {
                    EnergyPlayer = EnergyPlayer - 1;
                    ipan.SetText("fatigue", EnergyPlayer.ToString());
                }*/
                if (k == Key.Up)
                {
                    if (EnergyPlayer > 0)
                    {
                        if (PlayerY > 0)
                        {
                            PlayerY = PlayerY - 1;
                            SetPlayerEnergy(EnergyPlayer - 1);
                        }
                    }
                }
                if (k == Key.Down)
                {
                    if (EnergyPlayer > 0)
                    {
                        if (PlayerY < map.YCells - 1)
                        {
                            PlayerY = PlayerY + 1;
                            SetPlayerEnergy(EnergyPlayer - 1);
                        }
                    }
                }
                if (k == Key.Left)
                {
                    if (EnergyPlayer > 0)
                    {
                        if (PlayerX > 0)
                        {
                            PlayerX = PlayerX - 1;
                            SetPlayerEnergy(EnergyPlayer - 1);
                        }
                    }
                }
                if (k == Key.Right)
                {
                    if (EnergyPlayer > 0)
                    {
                        if (PlayerX < map.XCells - 1)
                        {
                            PlayerX = PlayerX + 1;
                            SetPlayerEnergy(EnergyPlayer - 1);
                        }
                    }
                }
                if (map.HasImageInCell("wall", PlayerX, PlayerY) || map.HasImageInCell("fence", PlayerX, PlayerY))
                {
                    if (hasPick)
                    {
                        map.RemoveFromCell("wall", PlayerX, PlayerY);
                        SetPlayerEnergy(EnergyPlayer - 2);
                    }
                    PlayerX = PlayerLastX;
                    PlayerY = PlayerLastY;
                }
                if (map.HasImageInCell("pick", PlayerX, PlayerY))
                {
                    map.RemoveFromCell("pick", PlayerX, PlayerY);

                    ipan.AddItem("pick", "pick", "кирка");
                    hasPick = true;
                }
                if (map.HasImageInCell("sword", PlayerX, PlayerY))
                {
                    map.RemoveFromCell("sword", PlayerX, PlayerY);
                    ipan.AddItem("sword", "sword", "меч");
                    hasSword = true;
                }
                if (map.HasImageInCell("gem", PlayerX, PlayerY))
                {
                    map.RemoveFromCell("gem", PlayerX, PlayerY);
                    ipan.AddItem("gem", "gem", "сокровище");
                    hasPick = true;
                }
                map.DrawInCell("smile", PlayerX, PlayerY);

                RemoveFog(PlayerX, PlayerY);

                map.RemoveFromCell("smile", PlayerLastX, PlayerLastY);
                if (k == Key.L && potionX == PlayerX && potionY == PlayerY)
                {
                    if (map.HasImageInCell("potionRed", potionX, potionY))
                    {
                        map.RemoveFromCell("potionRed", potionX, potionY);
                        map.DrawInCell("potionBlue", potionX, potionY);
                    }
                    else
                        if (map.HasImageInCell("potionBlue", potionX, potionY))
                        {
                            map.RemoveFromCell("potionBlue", potionX, potionY);
                            map.DrawInCell("potionGreen", potionX, potionY);
                        }
                        else
                            if (map.HasImageInCell("potionGreen", potionX, potionY))
                            {
                                map.RemoveFromCell("potionGreen", potionX, potionY);
                                map.DrawInCell("potionYellow", potionX, potionY);
                                EnergyPlayer = EnergyPlayer + 10;
                                map.AnimationInCell("fire", potionX, potionY, 4);
                            }
                }
                if (EnergyPlayer <= 0)
                {
                    PlayerTurn = false;
                    SetEvilEnergy(6);
                    map.DrawInCell("clock",PlayerX,PlayerY);
                    map.RemoveFromCell("clock", Player2X, Player2Y);
                    map.RemoveFromCell("smile", PlayerX, PlayerY);
                    map.DrawInCell("evil", Player2X, Player2Y);
                }
                if (map.HasImageInCell("bow", PlayerX, PlayerY))
                {
                    map.RemoveFromCell("bow", PlayerX, PlayerY);
                    ipan.AddItem("bow", "bow", "лук");
                    hasBow = true;
                } 
                if (map.HasImageInCell("arrow", PlayerX, PlayerY))
                {
                    map.RemoveFromCell("arrow", PlayerX, PlayerY);
                    arrow = arrow + 1;
                    ipan.SetText("arrow", arrow.ToString());
                }
            }
            //----------------------------------------------------------------------------------------------------------//
            //----------------------------------------------------------------------------------------------------------//
            if (isEvilElive && PlayerTurn == false)
            {
                if (EnergyEvil > 0)
                {
                    if (k == Key.W)
                    {
                        if (Player2Y > 0)
                        {
                            Player2Y = Player2Y - 1;
                            SetEvilEnergy(EnergyEvil - 1);
                        }
                    }
                    if (k == Key.S)
                    {
                        if (Player2Y < map.YCells - 1)
                        {
                            Player2Y = Player2Y + 1;
                            SetEvilEnergy(EnergyEvil - 1);
                        }
                    }
                    if (k == Key.A)
                    {
                        if (Player2X > 0)
                        {
                            Player2X = Player2X - 1;
                            SetEvilEnergy(EnergyEvil - 1);
                        }
                    }
                    if (k == Key.D)
                    {
                        if (Player2X < map.XCells - 1)
                        {
                            Player2X = Player2X + 1;
                            SetEvilEnergy(EnergyEvil - 1);
                        }
                    }
                    if (map.HasImageInCell("wall", Player2X, Player2Y) || map.HasImageInCell("fence", Player2X, Player2Y))
                    {
                        Player2X = PlayerLast2X;
                        Player2Y = PlayerLast2Y;
                    }
                    if (k == Key.Space)
                    {
                        if (holes > 0)
                        {
                            SetEvilEnergy(EnergyEvil - 3);
                            map.DrawInCell("hole", Player2X, Player2Y);
                            holes = holes - 1;
                            PrintHolesNum();
                        }
                    }

                    if (k == Key.LeftShift && hasTNT > 0)
                    {
                        SetEvilEnergy(EnergyEvil - 6);
                        map.AnimationInCell("exep", Player2X, Player2Y, 1);
                        hasTNT = hasTNT - 1;
                        ipan.SetText("DINAMIT", hasTNT.ToString());
                        if (Player2Y < map.YCells - 1)
                        {
                            map.RemoveFromCell("wall", Player2X, Player2Y + 1);
                            if (PlayerX == Player2X && PlayerY == Player2Y + 1)
                            {
                                map.DrawInCell("rip2",Player2X,Player2Y + 1);
                                isPlayerAlive = false;                                 
                            }
                        }
                        if (Player2Y > 0)
                        {
                            map.RemoveFromCell("wall", Player2X, Player2Y - 1);
                            if (PlayerX == Player2X && PlayerY == Player2Y - 1)
                            {
                                map.DrawInCell("rip2", Player2X, Player2Y - 1);
                                isPlayerAlive = false;
                            }
                        }
                        if (Player2X < map.XCells - 1)
                        {
                            map.RemoveFromCell("wall", Player2X + 1, Player2Y);
                            if (PlayerX == Player2X + 1 && PlayerY == Player2Y)
                            {
                                map.DrawInCell("rip2", Player2X + 1, Player2Y);
                                isPlayerAlive = false;
                            }
                        }
                        if (Player2X > 0)
                        {
                            map.RemoveFromCell("wall", Player2X - 1, Player2Y);
                            if (PlayerX == Player2X - 1 && PlayerY == Player2Y)
                            {
                                map.DrawInCell("rip2", Player2X - 1, Player2Y);
                                isPlayerAlive = false;
                            }
                        }
                    }
                }
                else
                {
                    PlayerTurn = true;
                    SetPlayerEnergy(6);
                    map.DrawInCell("clock", Player2X, Player2Y);
                    map.RemoveFromCell("clock", PlayerX, PlayerY);
                    map.RemoveFromCell("evil", Player2X, Player2Y);
                    map.DrawInCell("smile", PlayerX, PlayerY);
                }
                if (map.HasImageInCell("green", Player2X, Player2Y))
                {
                    map.RemoveFromCell("green", Player2X, Player2Y);
                    ipan.AddItem("green", "green", "алмаз2");
                }
                if (map.HasImageInCell("DINAMIT", Player2X, Player2Y))
                {
                    map.RemoveFromCell("DINAMIT", Player2X, Player2Y);
                    hasTNT = hasTNT + 1;
                    ipan.SetText("DINAMIT", hasTNT.ToString());
                }
                map.DrawInCell("evil", Player2X, Player2Y);
                map.RemoveFromCell("evil", PlayerLast2X, PlayerLast2Y);
                RemoveFog(Player2X, Player2Y);
            }
            if (Player2X == PlayerX && Player2Y == PlayerY && hasSword)
            {
                // SetPlayerEnergy(6000);
                isEvilElive = false;
                map.RemoveFromCell("evil", Player2X, Player2Y);
                map.DrawInCell("rip", Player2X, Player2Y);
                map.ContainerSetFrame("fell", "stabbed");
                map.ContainerSetSize("fell",800,600);
                map.ContainerSetAngle("fell",30);
                map.ContainerSetCoordinate("fell", map.XAbsolute / 2, map.YAbsolute / 2);
            }
            if (map.HasImageInCell("hole", PlayerX, PlayerY))
            {
                map.RemoveFromCell("smile", PlayerX, PlayerY);
                map.DrawInCell("rip2", PlayerX, PlayerY);
                isPlayerAlive = false;
                map.ContainerSetSize("fell",800,600);
                map.ContainerSetAngle("fell",30);
                map.ContainerSetCoordinate("fell", map.XAbsolute / 2, map.YAbsolute / 2);
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        void LeftClick(int x, int y, int Xcell, int Ycell)
        {
            for (int i = 0; i < portals; i++)
            {
                if (PlayerX == portalX[i] && PlayerY == portalY[i])
                {
                    DrawPlayer(Xcell, Ycell);
                }
            }
           // MessageBox.Show(Xcell.ToString());
                if (hasBow == true && arrow >= 1)
            {
                arrow = arrow - 1;
                ipan.SetText("arrow",arrow.ToString());
                if(Xcell == Player2X && Ycell == Player2Y)
                {
                    if (r.Next(0, 2) == 1)
                    {
                        isEvilElive = false;
                        map.DrawInCell("rip", Player2X, Player2Y);
                        map.ContainerSetFrame("fell", "got");
                        map.ContainerSetSize("fell", 800, 600);
                        map.ContainerSetAngle("fell", 30);
                        map.ContainerSetCoordinate("fell", map.XAbsolute / 2, map.YAbsolute / 2);
                    }
                    else
                    {
                        map.ContainerSetFrame("fell", "missed");
                        map.ContainerSetSize("fell", 800, 600);
                        map.ContainerSetAngle("fell", 30);
                        map.ContainerSetCoordinate("fell", map.XAbsolute / 2, map.YAbsolute / 2);
                        Delete = false;
                        timer.AddAction(DeletePicture, 2000);
                    }
                }
            }
        }
        void  DrawPlayer(int x, int y)
        {
            map.RemoveFromCell("smile",PlayerX,PlayerY);
            PlayerX = x; PlayerY = y;
            map.DrawInCell("smile",x,y);
        }
    }
}