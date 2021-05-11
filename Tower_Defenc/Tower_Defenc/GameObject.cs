using GameMaps;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;
namespace Tower_Defenc
{
    public class GameObject
    {
        static public UniversalMap_Wpf map;
        // ПОДСКАЗАКА ПОДСКАЗАКА ПОДСКАЗАКА ПОДСКАЗАКА ПОДСКАЗАКА ПОДСКАЗАКА ПОДСКАЗАКА
        // если state = 0, то с танком все ОК.
        // если state = 1, то танк ДЫМИТСЯ.
        // если state = 2, то танк ГОРИТ.
        // есди state = 3, то танк УНИЧТОЖЕН.
        // Ну можно ещё что то дописать.


        // если SubdivisionNumber = 0, НАШ.
        // если SubdivisionNumber = 1, ВРАГ самаходка LOW.
        int state = 0;
        public int SubdivisionNumber; 
        public int TargetRadiusEnemy = 700;
        public int Range = 5;
        int DamageCounter = 2000;
        public double X;
        public double Y;
        public int HP;
        public bool IsMarked = false;       
        double SpeedX;
        double SpeedY;
        Rectangle HPLINE = new Rectangle();
        public bool NeedToMove = false;
        public int TargetX;
        public int TargetY;
        public int Angle;
        public double Speed;
        public string Type;
        public string ContainerName;
        /// <summary>
        /// Пошли функции
        /// </summary>
        public GameObject(string PICTURE, string Container, string type)
        {
            map.Library.AddContainer(Container, PICTURE, ContainerType.AutosizedSingleImage);
            ContainerName = Container;
            map.ContainerSetLeftClickHandler(ContainerName, ClickType.Left, LeftClick);
            map.Library.AddContainer(ContainerName + "top", "nothing", ContainerType.AutosizedSingleImage);
            map.Library.AddContainer(ContainerName + "mark", "AllyScope", ContainerType.AutosizedSingleImage);
            map.Library.AddContainer(ContainerName + "Anime", "nothing", ContainerType.AutosizedSingleImage);
            Type = type;
            HPLINE.Height = 10;
            HPLINE.Fill = Brushes.Green;
            map.Canvas.Children.Add(HPLINE);
            map.ContainerSetZIndex(ContainerName + "Anime",86);
            //map.DrawRectangle((int)X - 35, (int)Y + 25, hp, 10, Brushes.Green, Brushes.Green);
        }
        public GameObject(string PICTURE, string Container, string type, int x, int y, int size): this(PICTURE, Container, type)
        {
            map.ContainerSetMaxSide(ContainerName, 70);
            SetCoordinate(x, y);
            map.ContainerSetAngle(ContainerName, 180);
            map.ContainerSetZIndex(ContainerName, 87);
        }

        public void SetCoordinate(double x, double y)
        {
            map.ContainerMovePreview(ContainerName, x, y, Angle);
            for (int i = 0; i < MainWindow.obstacle.Count; i++) 
            {
                if (map.CollisionContainers(ContainerName, MainWindow.obstacle[i].ContainerName, true) &&
                   ContainerName != MainWindow.obstacle[i].ContainerName)
                {
                    CheckDamageCounter();
                    MainWindow.obstacle[i].CheckDamageCounter();
                    return;
                }
            }
            //Debug.WriteLine("Name = {0}, x = {1}, y = {2}", ContainerName, x, y);
            X = x;
            Y = y;
            Canvas.SetTop(HPLINE, Y + 35);
            Canvas.SetLeft(HPLINE, X - 35);
            map.ContainerSetCoordinate(ContainerName, x, y);
            if (IsMarked)
            {
                map.ContainerSetCoordinate(ContainerName + "mark", X, Y);
            }
            map.ContainerSetCoordinate(ContainerName + "Anime", X, Y);
        }

        public void CheckDamageCounter()
        {
            if (DamageCounter >= 2000)
            {
                //MainWindow.Allies[i].SetHp(MainWindow.Allies[i].HP - 1);
                map.ContainerSetFrame(ContainerName + "top", "exp1");
                map.ContainerSetMaxSide(ContainerName + "top", 42);
                map.ContainerSetCoordinate(ContainerName + "top", X, Y);
                map.AnimationStart(ContainerName + "top", "Explosion_Collision", 1, StopAnimation);
                SetHp(HP - 20);
                DamageCounter = 0;
            }
            else
            {
                DamageCounter = DamageCounter + 20;
            }
        }

        void StopAnimation()
        {
            map.ContainerSetFrame(ContainerName + "top", "nothing");
        }

        public void LeftClick()
        {
            if (SubdivisionNumber == 0)
            {
                if (HP == 0) return;
                if (Type == "basa") return;
                if (MainWindow.SelectedUnit != null)
                {
                    MainWindow.SelectedUnit.IsMarked = false;
                    map.ContainerSetCoordinate(MainWindow.SelectedUnit.ContainerName + "mark", -1000, -1000);
                    //MainWindow.SelectedUnit.HPLINE.Height = 0;
                }
                MainWindow.SelectedUnit = this;
                IsMarked = true;
                map.ContainerSetMaxSide(ContainerName + "mark", 62);
            }
        }

        public void Move()
        {
            if(NeedToMove)
            {
                SetCoordinate(X + SpeedX, Y + SpeedY);
                double TargetAngle = GameMath.GetAngleOfVector(TargetX - X, TargetY - Y);
                if (TargetAngle > Angle)
                {
                    SetAngle(Angle + 2);
                }
                else
                {
                    SetAngle(Angle - 2);
                }
                if(Math.Abs(X - TargetX) < Range && Math.Abs(Y - TargetY) < Range)
                {
                    NeedToMove = false;
                }
            }
        }

        public void SetAngle(int angle) {
            Angle = angle;
            map.ContainerSetAngle(ContainerName, angle);
            SpeedX = Speed * Math.Cos(GameMath.DegreesToRad(angle));
            SpeedY = Speed * Math.Sin(GameMath.DegreesToRad(angle));
            map.ContainerSetAngle(ContainerName + "Anime", angle);
        }

        public void SetHp(int hp)
        {
            HP = hp;
            if(HP < 0)
            {
                HP = 0;
            }
            if (state == 0 && HP >= 20 && HP < 50)
            {
                
            }
            else if (state != 2 && HP < 20 && HP > 0)
            {
                map.ContainerSetFrame(ContainerName + "Anime", "Fire Bolt1");
                map.ContainerSetMaxSide(ContainerName + "Anime", 100);
                map.AnimationStart(ContainerName + "Anime", "Fire1", -1);
                state = 2;
            }
            else if (state != 3 && HP == 0)
            {
                map.ContainerSetFrame(ContainerName, "Destroyed_Tank_Low_ALLY");
                map.ContainerSetFrame(ContainerName + "mark", "nothing");
                NeedToMove = false;
                map.AnimationStop(ContainerName + "Anime", "Fire1");
                map.ContainerSetFrame(ContainerName + "Anime", "nothing");
            }
            HPLINE.Width = HP / 2;
        }
    }
}