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
        TimerContainer timer = new TimerContainer(20); 
        int X = 50;
        int Y = 70;
        int GRADUS = 40;
        bool BmxRamp = false;

        //Функция по таймеру TimerController и действие подение на землю после прыжка
        //Заключительный фрейм анимации поставить базовую картинку велосипеда
        // Функция "полёта" игрока, проверка нажатия на кнопку, тогда скорость градус возростает на 1, а если нет, то на 3(функция по таймеру)


        public MainWindow()
        {
            InitializeComponent();
            map = MapCreator.GetUniversalMap(this, mapInfo);
            SpMap.Children.Add(map.Canvas);
            AddPictures();
            CreateContainers();
            timer.AddAction(Cycle);
        }

        void AddPictures()
        {
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            map.Library.AddPicture("ramp", "ramp.png");
            map.Library.AddPicture("BMX", "bmx.png");
            map.Library.AddPicture("nothing", "nothing.png");
            CreateAnimathion("bmxA", 3);
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
        }


        void CollisionBmx(string Container1, string Container2, int Angle)
        {
            if (map.CollisionContainers(Container1, Container2))
            {
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
        }

        void CreateAnimathion(string main, int count)
        {
            string str;
            AnimationDefinition anim = new AnimationDefinition();
            for(int i = 0; i < count; i++)
            {
                str = main + count + ".png";
                map.Library.AddPicture(main + i, str);
                anim.AddFrame(70, main + i);
            }
            anim.AddFrame(1, "nothing");
            anim.LastFrame = "nothing";
            map.Library.AddAnimation(main, anim);
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
                }
            }

            if(map.Keyboard.IsKeyPressed(Key.A))
            {
                if(BmxRamp == true)
                {
                    return;
                }
                else
                {
                    X = X - 1;
                    map.ContainerSetCoordinate("bmx", X, Y);
                    map.AnimationStart("bmx", "bmxA", 1);
                }
            }

            if (map.Keyboard.IsKeyPressed(Key.Down))
            {
                if (GRADUS <= 20) return;
                else
                {
                    GRADUS = GRADUS - 1;
                    map.ContainerSetAngle("bmx", GRADUS);
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
                }
            }

        }
    }
}
