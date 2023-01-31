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

namespace rider
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CellMapInfo mapInfo = new CellMapInfo(50, 25, 40, 0);
        static public UniversalMap_Wpf map;
        TimerController timer = new TimerController();
        //int coor = 50;
        int X = 50;
        int Y = 70;
        int GRADUS = 40;
        int speedFly;
        bool tr = false;
        bool BmxRamp = false;
        bool Isfly = false;

        //Функция по таймеру TimerController и действие подение на землю после прыжка
        //Заключительный фрейм анимации поставить базовую картинку велосипеда
        //Функция "полёта" игрока, проверка нажатия на кнопку, тогда скорость градус возростает на 1, а если нет, то на 3(функция по таймеру)


        public MainWindow()
        {
            InitializeComponent();
            map = MapCreator.GetUniversalMap(this, mapInfo);
            SpMap.Children.Add(map.Canvas);
            AddPictures();
            CreateContainers();
            timer.AddAction(Cycle, 20);
            //timer.AddAction(FlyMove, 10);

            Uri res = new Uri(@"C:\Users\Admin\Documents\GitHub\GameProject\rider\rider\images\nac.jpg");
            BitmapImage img = new BitmapImage(res);
            ImageBrush br = new ImageBrush(img);
            SpMap.Background = br;
            // Проблема с картой
            map.SetMapBackground("backG");
        }


        void AddPictures()
        {
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            map.Library.AddPicture("ramp", "ramp.png");
            map.Library.AddPicture("BMX", "bmx.png");
            map.Library.AddPicture("nothing", "nothing.png");
            map.Library.AddPicture("coll", "Collision.png");
            map.Library.AddPicture("trick1", "trick0.png");
            map.Library.AddPicture("trick2", "trick2.png");
            map.Library.AddPicture("trickGG", "trickGG.png");
            map.Library.AddPicture("trickGG2", "trickGG2.png");
            map.Library.AddPicture("backG", "nac.jpg");
            CreateAnimation("bmxA", 3);
            CreateAnimation("trick", 0);
            CreateAnimation("anim", 3);
        }

        void CreateContainers()
        {
            /*map.Library.AddContainer("ContMap", "ramp", ContainerType.SingleImage);
            map.ContainerSetSize("ContMap", 30, 30);
            map.ContainerSetCoordinate("ContMap", 11, 10);*/

            map.Library.AddContainer("bmx", "BMX", ContainerType.AutosizedSingleImage);
            map.ContainerSetSize("bmx", 80);
            map.ContainerSetCoordinate("bmx", 50, 70);
            map.ContainerSetAngle("bmx", 40);

            map.Library.AddContainer("ramp", "ramp", ContainerType.AutosizedSingleImage);
            map.ContainerSetSize("ramp", 120);
            map.ContainerSetCoordinate("ramp", 500, 70);

            map.Library.AddContainer("coll", "coll", ContainerType.AutosizedSingleImage);
            map.ContainerSetSize("coll", 1);
            map.ContainerSetCoordinate("coll", 600, 20);

            map.Library.AddContainer("trick1", "trick1", ContainerType.AutosizedSingleImage);
            map.ContainerSetSize("trick1", 1);

            map.Library.AddContainer("trickGG", "trickGG", ContainerType.AutosizedSingleImage);
            map.ContainerSetSize("trickGG", 1);
            map.ContainerSetCoordinate("trickGG", 700, 300);

            map.Library.AddContainer("animS", "anim2", ContainerType.AutosizedSingleImage);
            map.ContainerSetSize("animS", 100);
            map.ContainerSetCoordinate("animS", 100, 900);
        }


        void CollisionBmx(string Container1, string Container2, int Angle)
        {
            if (map.CollisionContainers(Container1, Container2))
            {
                tr = map.CollisionContainers("bmx", "coll");
                X++;
                map.ContainerSetAngle(Container1, Angle);
                BmxRamp = true;
                GRADUS = Angle;
            }
        }

        void Restart()
        {
            if (map.Keyboard.IsKeyPressed(Key.O))
            {
                map.ContainerSetSize("bmx", 80);
                map.ContainerSetCoordinate("bmx", 50, 70);
                map.ContainerSetAngle("bmx", 40);
            }
        }

        void Cycle()
        {
            Moving();
            CollisionBmx("bmx", "ramp", 20);
            Restart();
            Fly();
            gg();
            //FlyMove();
        }

        /*void FlyMove()
        {
            coor++;
            map.ContainerSetCoordinate("bmx", coor, Y);
        }*/

        /*void CreateAnimathion(string main, int count)
        {
            string str;
            AnimationDefinition anim = new AnimationDefinition();
            for(int i = 0; i < count; i++)
            {
                str = main + count + ".png";
                map.Library.AddPicture(main + i, str);
                anim.AddFrame(70, main + i);
            }
            anim.AddFrame(1, "BMX");
            anim.LastFrame = "BMX";
            map.Library.AddAnimation(main, anim);
        }*/

        void gg()
        {
            if(tr == true)
            {
                if (Y < 70) Y = Y + 2;
            }
        }

        void Fly()
        {
            if(map.CollisionContainers("bmx", "coll"))
            {
                X = X + 2;
                map.ContainerSetCoordinate("bmx", X, Y);
                map.ContainerSetAngle("bmx", 50);
                BmxRamp = false;
                Isfly = true;
                if (tr == true)
                {
                    /*if (map.Keyboard.IsKeyPressed(Key.G))
                    {
                        if(Y < 70)
                        {
                            Y = Y + 5;
                        }
                    }
                    else
                    {
                        if (Y < 70) Y = Y + 2;
                    }*/
                }
            }
        }

        void CheckFly()
        {
            if(map.CollisionContainers("bmx", "ramp"))
            {
                if (map.CollisionContainers("bmx", "coll"))
                {

                }
            }
        }

        void CreateAnimation(string BaseName, int count)
        {
            AnimationDefinition a = new AnimationDefinition();
            string path;
            for (int i = 0; i < count; i++)
            {
                path = BaseName + i + ".png";
                map.Library.AddPicture(BaseName + i, path);
                a.AddFrame(100, BaseName + i);
            }
            a.AddFrame(1, "nothing");
            a.LastFrame = "nothing";
            map.Library.AddAnimation(BaseName, a);
        }









        void Moving()
        {
            if(map.Keyboard.IsKeyPressed(Key.D))
            {
                X = X + 3;
                if(BmxRamp == true)
                {
                    Y = Y - 1;
                    map.ContainerSetCoordinate("bmx", X, Y);
                }
                else
                {
                    map.ContainerSetCoordinate("bmx", X, Y);
                    //map.AnimationStart("bmx", "bmxA", 1);
                }
            }

            if(map.Keyboard.IsKeyPressed(Key.M))
            {
                map.AnimationStart("animS", "anim", 200);
            }

            if (map.Keyboard.IsKeyPressed(Key.A))
            {
                if(BmxRamp == true)
                {
                    return;
                }
                else
                {
                    X = X - 1;
                    map.ContainerSetCoordinate("bmx", X, Y);
                    //map.AnimationStart("bmx", "bmxA", 1);
                }
            }

            if(map.Keyboard.IsKeyPressed(Key.H))
            {
                speedFly++;
            }

            if (map.Keyboard.IsKeyPressed(Key.Down))
            {
                if (GRADUS <= 20) return;
                else
                {
                    GRADUS = GRADUS - 1;
                    map.ContainerSetAngle("bmx", GRADUS);
                    map.AnimationStart("bmx", "bmxA", 1);
                }
            }

            if (map.Keyboard.IsKeyPressed(Key.Up))
            {
                if (GRADUS >= 41) return;
                else
                {
                    GRADUS = GRADUS + 3;
                    map.ContainerSetAngle("bmx", GRADUS);
                }
            }

            if (map.Keyboard.IsKeyPressed(Key.Space))
            {
                if(Y >= 70)
                {
                    Y = Y - 20;
                    map.ContainerSetCoordinate("bmx", X, Y);
                    map.AnimationStart("bmx", "bmxA", 1);
                }
            }

            if(map.Keyboard.IsKeyPressed(Key.L))
            {
                if(Isfly == true)
                {
                    map.ContainerSetFrame("bmx", "trick1");
                    map.ContainerSetSize("bmx", 80);
                    Isfly = false;

                    CreateGGs trick1 = new CreateGGs("trickGG2", 1, 700, 350, "trickGG2");
                    trick1.GGTricks("trickGG2");
                }
            }
            
            if(map.Keyboard.IsKeyPressed(Key.B))
            {
                if (Isfly == true)
                {
                    map.ContainerSetFrame("bmx", "trick2");
                    map.ContainerSetSize("bmx", 80);
                    Isfly = false;
                    timer.AddAction(GGTricks, 10);
                    GGTricks();
                }
            }
        }

        void GGTricks()
        {
            for(int i = 0; i < 100; i++)
            {
                map.ContainerSetSize("trickGG", i);
            }
        }
    }
}