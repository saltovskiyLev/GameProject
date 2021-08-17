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
        Int64 state = 0;
        int MaxCounter;
        static int counter = 0;
        public bool NeedToRotate = false;
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
        public int mode; // 0 - Обьект стоит на месте   // 1 - Обьект движется в точку на карте // 2 - Обьект движется к другому игровому обьекту;
        public GameObject TargetObject;
        Rectangle HPLINE = new Rectangle();
        public bool NeedToMove = false;
        //public int TargetX;
        //public int TargetY;
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
            map.ContainerSetZIndex(ContainerName + "Anime", 86);
            //map.DrawRectangle((int)X - 35, (int)Y + 25, hp, 10, Brushes.Green, Brushes.Green);
        }

        public GameObject()
        {
            ////
        }
        public GameObject(string PICTURE, string Container, string type, int x, int y, int size): this(PICTURE, Container, type)
        {
            map.ContainerSetMaxSide(ContainerName, size);
            SetCoordinate(x, y);
            map.ContainerSetAngle(ContainerName, 180);
            map.ContainerSetZIndex(ContainerName, 87);
        }

        public void CheckObject()
        {
            CheckMaxCounter();
        }

        public void CheckMaxCounter()
        {
            if(MaxCounter >= 2000)  
            {
                MaxCounter = -900000000;
                map.ContainerSetFrame(ContainerName + "mark", "nothing");
            }
            else
            {
                MaxCounter = MaxCounter + 10;
            }
        }

        public void SetCoordinate(double x, double y)
        {
            map.ContainerMovePreview(ContainerName, x, y, Angle);
            // Этот цикл должен быть в отдельной функции проверки столкновения обьектов.
            // /та функция вызывается в GameCycle.
            for (int i = 0; i < MainWindow.obstacle.Count; i++)
            {
                if (map.CollisionContainers(ContainerName, MainWindow.obstacle[i].ContainerName, true) &&
                   ContainerName != MainWindow.obstacle[i].ContainerName)
                {
 /*                   if(Type == "Shell")
                    {

                    }*/
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
                    //MainWindow.SelectedUnit.HPLINE.Height = 0;99
                }
                MainWindow.SelectedUnit = this;
                IsMarked = true;
                map.ContainerSetMaxSide(ContainerName + "mark", 62);
            }
            else
            {
                map.ContainerSetFrame(ContainerName + "mark", "MarkEnemy");
                map.ContainerSetMaxSide(ContainerName + "mark", 70);
                IsMarked = true;
                MaxCounter = 0;
                MainWindow.SelectedEnemyUnit = this;
                MainWindow.ClickCount = 5;
                MainWindow.SelectedUnit.mode = 2;
            }
        }

        public void Move()
        {
            if(NeedToMove)
            {
                SetCoordinate(X + SpeedX, Y + SpeedY);
                if (Math.Abs(X - TargetObject.X) < Range && Math.Abs(Y - TargetObject.Y) < Range)
                {
                    NeedToMove = false;
                    NeedToRotate = false;
                }
            }
            // Debug.WriteLine("{0}, {1}", X, Y);
        }

        bool CheckAim()
        {
            bool IsAimed = false;
            double AngleToVector = GameMath.GetAngleOfVector(TargetObject.X - X, TargetObject.Y - Y);
            if(Math.Abs(Angle - AngleToVector) < 3)
            {
                IsAimed = true;
            }

            return IsAimed;
        }

        public void PerformAction()
        {
            if(mode == 2 && CheckAim())
            {
                // выполнение выстрела.                                 
                // Мы дожны созать переменную типа GameObject.           
                // Координаты и угол как у игрокау этого GameObject(а). 
                // Картинка снаряда взовисемонсти от того кто стрелял.   
                // Добавить переменную к списку снарядов.                 
                GameObject Shell = new GameObject("BulletAllies", "Bullet" + counter.ToString(), "BulletAllies");
                counter++;
                // задать координаты, угол поворота и установить скорость движение снаряда.
                /*Shell.X = X;
                Shell.Y = Y;
                Shell.Angle = Angle;*/
                Shell.Type = "Shell";
                Shell.SubdivisionNumber = SubdivisionNumber;
                Shell.SetCoordinate(X, Y);
                Shell.SetAngle(Angle);
                // задаём размер контейнера.
                map.ContainerSetSize(Shell.ContainerName, 12);
                // отрисовываем контейнер с нужным углом;
            }
        }

        public void Rotate()
        {
            if (NeedToRotate)
            {
                double TargetAngle = GameMath.GetAngleOfVector(TargetObject.X - X, TargetObject.Y - Y);
                if (TargetAngle > Angle)
                {
                    SetAngle(Angle + 2);
                }
                else
                {
                    SetAngle(Angle - 2);
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
                NeedToRotate = false;
                map.AnimationStop(ContainerName + "Anime", "Fire1");
                map.ContainerSetFrame(ContainerName + "Anime", "nothing");
            }
            HPLINE.Width = HP / 2;
        }
    }
}